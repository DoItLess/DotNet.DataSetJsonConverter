using System;
using System.Data;

namespace DotNet.JsonConverter
{
    public partial class DataSetConverter:Newtonsoft.Json.JsonConverter
    {
        private readonly ConvertLevel _level;
        private readonly DateTimeFormatType _dateTimeFormatType;

        public DataSetConverter(ConvertLevel level = ConvertLevel.Normal, DateTimeFormatType type = DateTimeFormatType.Default)
        {
            _level = level;
            _dateTimeFormatType = type;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(DataSet).IsAssignableFrom(objectType);
        }
    }
}
