using System;
using System.Data;
using Newtonsoft.Json;

namespace DotNet.JsonConverters
{
    public partial class DataTableConverter : JsonConverter
    {
        private readonly ConvertLevel        _level;
        private readonly DateTimeFormatStyle _style;

        public DataTableConverter(ConvertLevel level = ConvertLevel.Normal, DateTimeFormatStyle style = DateTimeFormatStyle.Default)
        {
            _level = level;
            _style = style;
        }


        public override bool CanConvert(Type objectType)
        {
            return typeof(DataTable).IsAssignableFrom(objectType);
        }
    }
}