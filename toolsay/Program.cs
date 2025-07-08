using Library;
using Spectre.Console;

class Program
{
    static void Main(string[] args)
    {
        string inputText;        // Check if argument was provided
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
        }        // Check if we have any input
        if (string.IsNullOrWhiteSpace(inputText))
        {
            AnsiConsole.MarkupLine("[red]No input provided.[/]");
            AnsiConsole.MarkupLine("[white]Usage:[/]");
            AnsiConsole.MarkupLine(@"[white]\tmulti-rid-tool ""Your text here""[/]");
            AnsiConsole.MarkupLine(@"[white]\techo ""Your text"" | multi-rid-tool[/]");
            return;
        }

        // Create and display the ASCII art using FigletText
        AnsiConsole.Write(Figmatize.MakeGreenText(inputText));
    }
}