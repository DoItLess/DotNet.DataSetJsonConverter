using Newtonsoft.Json;
using System;
using System.Data;

namespace DotNet.DataSetJsonConverter
{
    public partial class DataSetJsonConverter:JsonConverter
    {
        private readonly ConvertLevel _level;

        public DataSetJsonConverter(ConvertLevel level)
        {
            _level = level;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(DataSet).IsAssignableFrom(objectType);
        }
    }
}
