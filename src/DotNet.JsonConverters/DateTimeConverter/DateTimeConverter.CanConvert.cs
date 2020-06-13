using System;
using Newtonsoft.Json;

namespace DotNet.JsonConverters
{
    public partial class DateTimeConverter : JsonConverter
    {
        private DateTimeFormatStyle _style;
        private DateTime _originDateTime = new DateTime(1970, 1, 1);

        public DateTimeConverter(DateTimeFormatStyle style = DateTimeFormatStyle.Default)
        {
            _style = style;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(DateTime).IsAssignableFrom(objectType);
        }
    }
}