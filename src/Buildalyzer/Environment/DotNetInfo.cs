namespace Buildalyzer.Environment;

/// <summary>Information about the .NET environment.</summary>
/// <remarks>
/// Retrieved via `dotnet --info`.
/// </remarks>
public sealed class DotNetInfo
{
    /// <summary>The version of the SDK.</summary>
    public Version? SdkVersion { get; set; }

    /// <summary>The name of the operating system.</summary>
    public string? OSName { get; set; }

    /// <summary>The platform of the operating system.</summary>
    public string? OSPlatform { get; set; }

    /// <summary>The version of the operating system.</summary>
    public Version? OSVersion { get; set; }

    /// <summary>The RID of the operating system.</summary>
    public string? RID { get; set; }

    /// <summary>The base path to the .NET environment.</summary>
    public string? BasePath { get; set; }

    /// <summary>The location of the global.json.</summary>
    public string? GlobalJson { get; set; }

    /// <summary>The installed SDK's.</summary>
    public ImmutableDictionary<string, string> SDKs { get; set; } = ImmutableDictionary<string, string>.Empty;

    /// <summary>The installed Runtimes.</summary>
    public ImmutableDictionary<string, string> Runtimes { get; set; } = ImmutableDictionary<string, string>.Empty;

    /// <summary>Parses the input.</summary>
    [Pure]
    public static DotNetInfo Parse(string? s)
        => Parse(s?.Split([System.Environment.NewLine], StringSplitOptions.RemoveEmptyEntries).Select(c => c.Trim())
            .ToArray() ?? []);

    /// <summary>Parses the input.</summary>
    [Pure]
    public static DotNetInfo Parse(IEnumerable<string>? lines)
        => DotNetInfoParser.Parse(lines ?? []);
}
