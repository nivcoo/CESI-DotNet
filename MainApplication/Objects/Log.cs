using System.Text.Json.Serialization;
using MainApplication.Storages.Converter;

namespace MainApplication.Objects;

public class Log
{
    public string? Name { get; set; }
    public Uri? SourcePath { get; set; }
    public Uri? TargetPath { get; set; }

    public int? FileSize { get; set; }
    public double? FileTransferTime { get; set; }
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime? Date { get; set; }
}