using System;
using System.Data;
using Newtonsoft.Json;

namespace DotNet.JsonConverters
{
    public partial class DataSetConverter : JsonConverter
    {
        private readonly ConvertLevel        _level;
        private readonly DateTimeFormatStyle _dateTimeFormatStyle;

        public DataSetConverter(ConvertLevel level = ConvertLevel.Normal, DateTimeFormatStyle style = DateTimeFormatStyle.Default)
        {
            _level               = level;
            _dateTimeFormatStyle = style;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(DataSet).IsAssignableFrom(objectType);
        }
    }
}