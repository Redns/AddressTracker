namespace AddressTracker.Core.Settings
{
    /// <summary>
    /// 通用设置
    /// </summary>
    public class GeneralSetting
    {
        /// <summary>
        /// 是否显示回环地址
        /// </summary>
        public bool DisplayLoopbackAddress { get; set; } = true;

        /// <summary>
        /// 域名映射
        /// </summary>
        public required DomainMapSetting DomainMap { get; set; }
    }
}
