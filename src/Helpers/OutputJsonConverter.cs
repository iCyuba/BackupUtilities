using System.Text.Json;
using System.Text.Json.Serialization;

namespace BackupUtility;

public class OutputJsonConverter : JsonConverter<BackupJob.BackupOutput>
{
    public override BackupJob.BackupOutput Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    ) =>
        reader.GetString() switch
        {
            "folder" => BackupJob.BackupOutput.Folder,
            "tar" => BackupJob.BackupOutput.Tar,
            "tar.gz" => BackupJob.BackupOutput.TarGz,
            "tar.bz2" => BackupJob.BackupOutput.TarBz2,
            "zip" => BackupJob.BackupOutput.Zip,
            _ => throw new JsonException("Invalid output type"),
        };

    public override void Write(
        Utf8JsonWriter writer,
        BackupJob.BackupOutput value,
        JsonSerializerOptions options
    ) => writer.WriteStringValue(value.GetDescription().ToLowerInvariant());
}
