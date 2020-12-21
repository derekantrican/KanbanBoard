using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace KanbanBoard.Converters
{
    public class ColorHexConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Color color = ((Avalonia.Media.Color)value).Convert();
            writer.WriteValue(color.GetHexCode(true));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string hexString = reader.Value.ToString();
            if (hexString == null || !hexString.StartsWith("#")) 
                return Color.Empty.Convert();

            return ColorTranslator.FromHtml(hexString).Convert();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Avalonia.Media.Color);
        }
    }
}
