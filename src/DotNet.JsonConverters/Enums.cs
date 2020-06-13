namespace DotNet.JsonConverters
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
        Maximal
    }

    /// <summary>
    /// 时间类型转换风格
    /// </summary>
    public enum DateTimeFormatStyle
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default,

        /// <summary>
        /// 时间戳,毫秒,long
        /// </summary>
        TimeStampMillisecond
    }
}