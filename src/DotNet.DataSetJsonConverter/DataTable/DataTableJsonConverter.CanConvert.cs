using System;
using Newtonsoft.Json;

namespace DotNet.DataSetJsonConverter
{
    public partial class DataTableJsonConverter : JsonConverter
    {
        private readonly ConvertLevel _level;

        public DataTableJsonConverter(ConvertLevel level = ConvertLevel.Normal)
        {
            _level = level;
        }


        public override bool CanConvert(Type objectType)
        {
            return typeof(System.Data.DataTable).IsAssignableFrom(objectType);
        }
    }
}