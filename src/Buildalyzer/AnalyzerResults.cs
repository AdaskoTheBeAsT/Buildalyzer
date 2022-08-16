using System.Collections.Concurrent;
using Microsoft.Build.Framework;

namespace Buildalyzer;

public class AnalyzerResults : IAnalyzerResults
{
    private readonly ConcurrentDictionary<string, IAnalyzerResult> _results = new();

    private bool? _overallSuccess = null;

    public bool OverallSuccess => _overallSuccess == true;

    internal void Add(IEnumerable<IAnalyzerResult> results, bool overallSuccess)
    {
        foreach (IAnalyzerResult result in results)
        {
            _results[result.TargetFramework ?? string.Empty] = result;
        }

        _overallSuccess = _overallSuccess.HasValue ? _overallSuccess.Value && overallSuccess : overallSuccess;
    }

    /// <inheritdoc />
    public ImmutableArray<BuildEventArgs> BuildEventArguments { get; set; } = [];

    public IAnalyzerResult this[string targetFramework] => _results[targetFramework];

    public IEnumerable<string> TargetFrameworks => _results.Keys.OrderBy(e => e, TargetFrameworkComparer.Instance);

    public IEnumerable<IAnalyzerResult> Results => TargetFrameworks.Select(e => _results[e]);

    public int Count => _results.Count;

    public bool ContainsTargetFramework(string targetFramework) => _results.ContainsKey(targetFramework);

    public bool TryGetTargetFramework(string targetFramework, out IAnalyzerResult result) => _results.TryGetValue(targetFramework, out result);

    public IEnumerator<IAnalyzerResult> GetEnumerator() => Results.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private sealed class TargetFrameworkComparer : IComparer<string>
    {
        public static TargetFrameworkComparer Instance { get; } = new TargetFrameworkComparer();

        public int Compare(string x, string y)
        {
                if (string.IsNullOrEmpty(x) && string.IsNullOrEmpty(y))
            {
                return 0;
            }

            if (string.IsNullOrEmpty(x) && !string.IsNullOrEmpty(y))
            {
                return +1;
            }

            if (!string.IsNullOrEmpty(x) && string.IsNullOrEmpty(y))
            {
                return -1;
            }

            return StringComparer.OrdinalIgnoreCase.Compare(x, y);
        }
    }
}