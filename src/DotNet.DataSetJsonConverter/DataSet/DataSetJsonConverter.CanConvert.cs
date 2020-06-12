using Newtonsoft.Json;
using System;
using System.Data;

namespace DotNet.DataSetJsonConverter
{
    public partial class DataSetJsonConverter:JsonConverter
    {
        private readonly ConvertLevel _level;
        private readonly DateTimeFormatType _dateTimeFormatType;

        public DataSetJsonConverter(ConvertLevel level = ConvertLevel.Normal, DateTimeFormatType type = DateTimeFormatType.Default)
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
