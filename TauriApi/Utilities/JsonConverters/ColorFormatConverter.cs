using System.Text.Json;
using System.Text.Json.Serialization;

namespace TauriApi.Utilities.JsonConverters;

/// <summary>
/// Custom JSON converter for Color class that converts to/from hexadecimal color strings.
/// </summary>
public class ColorFormatConverter : JsonConverter<TauriColor>
{
    /// <summary>
    /// Reads and converts the JSON to a Color object.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    /// <returns>The converted value.</returns>
    public override TauriColor Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();

        if (string.IsNullOrEmpty(value))
        {
            throw new JsonException("Color value cannot be null or empty");
        }

        // If it doesn't start with #, add it
        if (!value.StartsWith("#"))
        {
            value = "#" + value;
        }

        try
        {
            return new TauriColor(value);
        }
        catch (ArgumentException ex)
        {
            throw new JsonException($"Invalid color format: {value}", ex);
        }
    }

    /// <summary>
    /// Writes a Color object as a hexadecimal color string.
    /// </summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The value to convert to JSON.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    public override void Write(Utf8JsonWriter writer, TauriColor value, JsonSerializerOptions options)
    {
        // Convert to hex string
        var hexColor = value.A == 255
            // If alpha is 255 (fully opaque), use 6-digit format (#RRGGBB)
            ? $"#{value.R:X2}{value.G:X2}{value.B:X2}"
            // If alpha is not 255, use 8-digit format (#RRGGBBAA)
            : $"#{value.R:X2}{value.G:X2}{value.B:X2}{value.A:X2}";

        writer.WriteStringValue(hexColor);
    }
}