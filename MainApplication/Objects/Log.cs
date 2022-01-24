using MainApplication.Objects.Enums;

namespace MainApplication.Objects;

public class Log
{
    public string? Name { get; set; }
    public string? SourcePath { get; set; }
    public string? TargetPath { get; set; }

    public int? FileSize { get; set; }
    public double? FileTransferTime { get; set; }
    public DateTime? Date { get; set; }
}