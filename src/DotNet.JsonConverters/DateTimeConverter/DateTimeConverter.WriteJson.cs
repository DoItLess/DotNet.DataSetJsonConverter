using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DotNet.JsonConverters
{
    public partial class DateTimeConverter
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            #region 校验

            if (!(value is DateTime dateTime)) throw new JsonException($"{nameof(DateTimeConverter)} Error : 无法转换为{nameof(DateTime)}");

            #endregion

            switch (_style)
            {
                case DateTimeFormatStyle.TimeStampMillisecond:
                    var result =  Convert.ToInt64((dateTime - _originDateTime).TotalMilliseconds);
                    writer.WriteValue(result);
                    break;
                case DateTimeFormatStyle.Default:
                    new IsoDateTimeConverter().WriteJson(writer, value, serializer);
                    break;
                default:
                    throw new JsonException($"{nameof(DateTimeConverter)} Error : 错误的 {nameof(DateTimeFormatStyle)}");
            }
        }
    }
}