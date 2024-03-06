using System.Text.Json;
using System.Text.Json.Serialization;

public static class Json
{
    /// <summary>
    /// Default serializer options
    /// </summary>
    public static readonly JsonSerializerOptions SerializerOptions =
        new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };

    public static readonly JsonSerializerOptions SerializerOptionsPretty =
        new(SerializerOptions) { WriteIndented = true };
}
