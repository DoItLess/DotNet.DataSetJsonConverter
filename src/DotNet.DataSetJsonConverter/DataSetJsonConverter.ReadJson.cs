using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace DotNet.DataSetJsonConverter
{
    public partial class DataSetJsonConverter
    {
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
