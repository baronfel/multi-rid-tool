using Spectre.Console;
using Spectre.Console.Rendering;

namespace Library;

public static class Figmatize
{
    public static IRenderable MakeGreenText(string inputText) =>
        new FigletText(inputText)
            .Centered()
            .Color(Color.Green);
}