using Spectre.Console;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;

class Program
{
    // Create an ActivitySource for tracing
    private static readonly ActivitySource ActivitySource = new("ToolSay.Application", "1.0.0");

    static void Main(string[] args)
    {
        // Configure OpenTelemetry with OTLP exporter
            using var tracerProvider = Sdk.CreateTracerProviderBuilder()
                .SetResourceBuilder(ResourceBuilder.CreateDefault()
                    .AddService("toolsay", "1.0.0")
                    .AddAttributes(new Dictionary<string, object>
                    {
                        ["service.instance.id"] = Environment.MachineName,
                        ["deployment.environment"] = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"
                    }))
                .AddSource("ToolSay.Application")
                .AddOtlpExporter()
                .Build();

        // Use the OpenTelemetry Propagator to extract the parent activity context and kind. For some reason this isn't set by the OTel SDK like docs say it should be.
        Sdk.SetDefaultTextMapPropagator(new CompositeTextMapPropagator([
            new TraceContextPropagator(),
            new BaggagePropagator()
        ]));

        // Extract trace context from environment variables using W3C TraceContext propagator
        var propagator = Propagators.DefaultTextMapPropagator;
        var carrier = new Dictionary<string, string>();
        
        // Read TRACEPARENT and TRACESTATE from environment variables
        var traceParent = Environment.GetEnvironmentVariable("TRACEPARENT");
        var traceState = Environment.GetEnvironmentVariable("TRACESTATE");
        
        if (!string.IsNullOrEmpty(traceParent))
        {
            carrier["traceparent"] = traceParent;
        }
        
        if (!string.IsNullOrEmpty(traceState))
        {
            carrier["tracestate"] = traceState;
        }

        // Extract the parent context
        var parentContext = propagator.Extract(default, carrier, (dict, key) => 
            dict.TryGetValue(key, out var value) ? new[] { value } : Array.Empty<string>());

        // Start the main activity with the extracted parent context
        using var mainActivity = ActivitySource.StartActivity("toolsay.main", ActivityKind.Internal, parentContext.ActivityContext);
        mainActivity?.SetTag("args.count", args.Length);

        string inputText;
        
        // Check if argument was provided
        if (args.Length > 0)
        {
            using var argActivity = ActivitySource.StartActivity("process.arguments");
            // Use the first argument as input
            inputText = string.Join(" ", args);
            argActivity?.SetTag("input.source", "arguments");
            argActivity?.SetTag("input.length", inputText.Length);
        }
        // Check if console input is redirected (piped input)
        else if (Console.IsInputRedirected)
        {
            using var pipeActivity = ActivitySource.StartActivity("process.piped_input");
            inputText = Console.ReadLine() ?? string.Empty;
            pipeActivity?.SetTag("input.source", "pipe");
            pipeActivity?.SetTag("input.length", inputText.Length);
        }
        else
        {
            using var interactiveActivity = ActivitySource.StartActivity("process.interactive_input");
            // Prompt user for input using Spectre.Console
            inputText = AnsiConsole.Ask<string>("Enter text to convert to [green]ASCII art[/]:");
            interactiveActivity?.SetTag("input.source", "interactive");
            interactiveActivity?.SetTag("input.length", inputText.Length);
        }
        
        // Check if we have any input
        if (string.IsNullOrWhiteSpace(inputText))
        {
            using var errorActivity = ActivitySource.StartActivity("display.usage_error");
            AnsiConsole.MarkupLine("[red]No input provided.[/]");
            AnsiConsole.MarkupLine("[white]Usage:[/]");
            AnsiConsole.MarkupLine(@"[white]\ttoolsay ""Your text here""[/]");
            AnsiConsole.MarkupLine(@"[white]\techo ""Your text"" | toolsay[/]");
            
            mainActivity?.SetStatus(ActivityStatusCode.Error, "No input provided");
            errorActivity?.SetTag("error.type", "no_input");
            return;
        }

        // Create and display the ASCII art using FigletText
        using var renderActivity = ActivitySource.StartActivity("render.ascii_art");
        renderActivity?.SetTag("input.text", inputText);
        renderActivity?.SetTag("render.type", "figlet");
        
        var figlet = new FigletText(inputText)
            .Centered()
            .Color(Color.Green);

        AnsiConsole.Write(figlet);
        
        renderActivity?.SetStatus(ActivityStatusCode.Ok);
        mainActivity?.SetStatus(ActivityStatusCode.Ok);
        mainActivity?.SetTag("operation.success", true);
    }
}