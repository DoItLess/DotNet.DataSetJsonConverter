using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace DotNet.JsonConverters
{
    public partial class DateTimeConverter
    {
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var jToken = JToken.Load(reader);

            if (jToken.Type == JTokenType.Null) return DBNull.Value;

            switch (_style)
            {
                case DateTimeFormatStyle.TimeStampMillisecond:
                    if(jToken.Type != JTokenType.Integer) throw new JsonException($"{nameof(DateTimeConverter)} Error : 错误的类型,需要 Integer");
                    return _originDateTime.AddMilliseconds(jToken.Value<long>());
                case DateTimeFormatStyle.Default:
                    return new IsoDateTimeConverter().ReadJson(reader, objectType, existingValue, serializer);
                default:
                    throw new JsonException($"{nameof(DateTimeConverter)} Error : 错误的 {nameof(DateTimeFormatStyle)}");
            }
        }
    }
}