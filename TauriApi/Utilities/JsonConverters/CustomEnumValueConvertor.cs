using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TauriApi.Utilities.JsonConverters;

/// <summary>
/// Attribute to specify a custom value for an enum field.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class CustomEnumValue : Attribute
{
    /// <summary>
    /// The custom value associated with the enum field.
    /// </summary>
    public string Value { get; }

    /// <inheritdoc />
    public CustomEnumValue(string value)
    {
        Value = value;
    }
}

/// <summary>
/// Custom JSON converter for enums that allows for custom string values.
/// </summary>
/// <typeparam name="T"></typeparam>
public class CustomEnumValueConverter<T> : JsonConverter<T> where T : struct, Enum
{
    /// <inheritdoc />
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();

        foreach (var field in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            var attribute = field.GetCustomAttribute<CustomEnumValue>();
            if (attribute != null && attribute.Value == value)
            {
                return (T)field.GetValue(null)!;
            }
        }

        // If no custom value matches, fall back to the default enum parsing
        return Enum.Parse<T>(value!, true);
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var field = typeof(T).GetField(value.ToString());
        var attribute = field?.GetCustomAttribute<CustomEnumValue>();

        writer.WriteStringValue(attribute != null ? attribute.Value : value.ToString());
    }
}