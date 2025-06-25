using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MamaFit.BusinessObjects.DTO;

public class DateTimeConverter : JsonConverter<DateTime>
{
    private readonly string _format = "yyyy-MM-dd HH:mm:ss";

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTime.ParseExact(reader.GetString(), _format, CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(_format));
    }
}
