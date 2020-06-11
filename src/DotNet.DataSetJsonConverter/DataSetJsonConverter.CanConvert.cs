﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Newtonsoft.Json;

namespace DotNet.DataSetJsonConverter
{
    public partial class DataSetJsonConverter:JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(DataSet).IsAssignableFrom(objectType);
        }
    }
}
