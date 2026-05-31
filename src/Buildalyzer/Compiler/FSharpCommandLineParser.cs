namespace Buildalyzer;

internal static class FSharpCommandLineParser
{
    [Pure]
    public static string[]? SplitCommandLineIntoArguments(string? commandLine)
        => commandLine?.Split(Splitters, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray() is { Length: > 0 } args
        && First(args[0]).ToArray() is { Length: >= 1 } first
            ? [.. first, .. args.Skip(1)]
            : null;

    [Pure]
    private static IEnumerable<string> First(string arg)
        => Tokenize(arg)
        .SkipWhile(NotCompilerLocation)
        .Select(a => a.Trim())
        .Where(a => a.Length > 0);

    [Pure]
    private static IEnumerable<string> Tokenize(string arg)
    {
        var first = 0;
        var cursor = 0;
        var quote = false;

        foreach (var ch in arg)
        {
            if (ch == '"')
            {
                if (quote)
                {
                    quote = false;
                    if (cursor > first)
                        yield return arg.Substring(first, cursor - first);
                }
                else
                {
                    quote = true;
                }
                first = cursor + 1;
            }
            else if (ch == ' ' && cursor >= first && !quote)
            {
                if (cursor > first)
                    yield return arg.Substring(first, cursor - first);
                first = cursor + 1;
            }
            cursor++;
        }
        yield return arg.Substring(first);
    }

    [Pure]
    public static bool NotCompilerLocation(string s)
        => !s.IsMatchEnd("fsc.dll")
        && !s.IsMatchEnd("fsc.exe");

    private static readonly char[] Splitters = ['\r', '\n'];
}
