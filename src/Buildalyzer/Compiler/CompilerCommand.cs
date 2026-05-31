using System.IO;
using Buildalyzer.IO;
using Microsoft.CodeAnalysis;

namespace Buildalyzer;

[DebuggerDisplay("{Language.Display()}: {Text}")]
public abstract record CompilerCommand
{
    /// <summary>The compiler lanuague.</summary>
    public abstract CompilerLanguage Language { get; }

    /// <summary>The original text of the compiler command.</summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>The parsed command line arguments.</summary>
    public ImmutableArray<string> Arguments { get; set; } = [];

    /// <summary>The location of the used compiler.</summary>
    public FileInfo? CompilerLocation { get; set; }

    /// <inheritdoc  cref="CommandLineArguments.Errors" />
    public ImmutableArray<Diagnostic> Errors { get; set; } = [];

    /// <inheritdoc  cref="CommandLineArguments.SourceFiles" />
    public ImmutableArray<IOPath> SourceFiles { get; set; } = [];

    /// <inheritdoc  cref="CommandLineArguments.AdditionalFiles" />
    public ImmutableArray<IOPath> AdditionalFiles { get; set; } = [];

    /// <inheritdoc  cref="CommandLineArguments.EmbeddedFiles" />
    public ImmutableArray<IOPath> EmbeddedFiles { get; set; } = [];

    /// <inheritdoc  cref="CommandLineArguments.AnalyzerReferences" />
    public ImmutableArray<IOPath> AnalyzerReferences { get; set; } = [];

    /// <inheritdoc  cref="CommandLineArguments.AnalyzerConfigPaths" />
    public ImmutableArray<IOPath> AnalyzerConfigPaths { get; set; } = [];

    /// <inheritdoc  cref="ParseOptions.PreprocessorSymbolNames" />
    public ImmutableArray<string> PreprocessorSymbolNames { get; set; } = [];

    /// <inheritdoc cref="CommandLineArguments.MetadataReferences" />
    public ImmutableArray<string> MetadataReferences { get; set; } = [];

    /// <summary>
    /// The aliases used in the command line arguments.
    /// </summary>
    public ImmutableDictionary<string, ImmutableArray<string>> Aliases { get; set; } = ImmutableDictionary<string, ImmutableArray<string>>.Empty;

    /// <inheritdoc />
    [Pure]
    public override string ToString() => Text ?? string.Empty;
}
