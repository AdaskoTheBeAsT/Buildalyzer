namespace Buildalyzer;

/// <summary>Collects the <see cref="ProcessData"/> durring a <see cref="System.Diagnostics.Process"/>.</summary>
[DebuggerDisplay("ExitCode = {_process.ExitCode}, Output = {_output.Count}, Error = {_error.Count}")]
internal sealed class ProcessDataCollector : IDisposable
{
    private readonly Process _process;
    private readonly List<string> _output = [];
    private readonly List<string> _error = [];
    private bool _disposed;

    public ProcessDataCollector(Process process)
    {
        _process = process;
        _process.OutputDataReceived += OutputDataReceived;
        _process.ErrorDataReceived += ErrorDataReceived;
    }

    public ProcessData Data => new(
        [.. _output],
        [.. _error]);

    private void OutputDataReceived(object? sender, DataReceivedEventArgs e) => Add(e.Data, _output);

    private void ErrorDataReceived(object? sender, DataReceivedEventArgs e) => Add(e.Data, _error);

    private static void Add(string? value, List<string> buffer)
    {
        if (value is { Length: > 0 })
        {
            buffer.Add(value);
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (!_disposed)
        {
            _process.OutputDataReceived -= OutputDataReceived;
            _process.ErrorDataReceived -= ErrorDataReceived;
            _disposed = true;
        }
    }
}
