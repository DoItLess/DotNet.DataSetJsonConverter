using System;
using System.Data;
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

        public enum ConvertLevel
        {
            Minimal,
            Normal,
            Maximal,
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(DataTable).IsAssignableFrom(objectType);
        }
    }
}