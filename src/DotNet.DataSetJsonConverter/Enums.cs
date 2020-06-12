namespace DotNet.DataSetJsonConverter
{
    /// <summary>
    /// 转换级别
    /// </summary>
    public enum ConvertLevel
    {
        /// <summary>
        /// 最小
        /// </summary>
        Minimal,

        /// <summary>
        /// 正常
        /// </summary>
        Normal,

        /// <summary>
        /// 最大
        /// </summary>
        Maximal,
    }

    public enum DateTimeFormatType
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default,
        /// <summary>
        /// 时间戳,毫秒,long
        /// </summary>
        TimeStampMillisecond,
    }
}