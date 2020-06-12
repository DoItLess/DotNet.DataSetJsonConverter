using System;
using System.Data;
using Newtonsoft.Json;

namespace DotNet.DataSetJsonConverter
{
    public partial class DataTableJsonConverter : JsonConverter
    {
        private readonly ConvertLevel       _level;
        private readonly DateTimeFormatType _dateTimeFormatType;

        public DataTableJsonConverter(ConvertLevel level = ConvertLevel.Normal, DateTimeFormatType type = DateTimeFormatType.Default)
        {
            _level              = level;
            _dateTimeFormatType = type;
        }


        public override bool CanConvert(Type objectType)
        {
            return typeof(DataTable).IsAssignableFrom(objectType);
        }

        /// <summary>
        /// DateTime 转 时间戳，毫秒
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private static long DateTimeToMilliseconds(DateTime dateTime)
        {
            var startTime = new DateTime(1970,1,1);
            return (long) (dateTime - startTime).TotalMilliseconds;
        }

        /// <summary>
        /// 时间戳，毫秒 转 DateTime
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        private static DateTime MillisecondsToDateTime(long milliseconds)
        {
            var startTime = new DateTime(1970,1,1);
            return startTime.AddMilliseconds(milliseconds);
        }
    }
}