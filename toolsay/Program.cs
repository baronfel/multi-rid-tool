using Spectre.Console;
using System.Diagnostics;
using System.CommandLine;

class Program
{
    // Create an ActivitySource for tracing
    private static readonly ActivitySource ActivitySource = new("ToolSay.Application", "1.0.0");
    private static readonly Command rootCommand = new RootCommand("A simple tool to convert text to ASCII art using Figlet and OpenTelemetry for tracing.")
    {
        new Argument<string>("input")
    };

    static void Main(string[] args)
    {

        var result = rootCommand.Parse(args);
        string inputText;

        // Check if argument was provided
        if (args.Length > 0)
        {
            // Use the first argument as input
            inputText = string.Join(" ", args);
        }
        // Check if console input is redirected (piped input)
        else if (Console.IsInputRedirected)
        {
            inputText = Console.ReadLine() ?? string.Empty;
        }
        else
        {
            // Prompt user for input using Spectre.Console
            inputText = AnsiConsole.Ask<string>("Enter text to convert to [green]ASCII art[/]:");
        }

        // Check if we have any input
        if (string.IsNullOrWhiteSpace(inputText))
        {
            AnsiConsole.MarkupLine("[red]No input provided.[/]");
            AnsiConsole.MarkupLine("[white]Usage:[/]");
            AnsiConsole.MarkupLine("[white]\ttoolsay \"Your text here\"[/]");
            AnsiConsole.MarkupLine("[white]\techo \"Your text here\" | toolsay[/]");

            return;
        }

        var figlet = new FigletText(inputText)
            .Centered()
            .Color(Color.Green);

        AnsiConsole.Write(figlet);
    }
}