namespace AddressTracker.Core.Settings
{
    /// <summary>
    /// 域名映射
    /// </summary>
    public class DomainMapSetting
    {
        /// <summary>
        /// 强制更新时间（分钟）
        /// </summary>
        private long _forceUpdateInterval = 10L;
        public long ForceUpdateInterval
        {
            get
            {
                return _forceUpdateInterval;
            }

            set
            {
                if (_forceUpdateInterval > 0)
                {
                    _forceUpdateInterval = value;
                }
            }
        }

        /// <summary>
        /// 自动匹配托管商
        /// </summary>
        public bool AutoMatchCustodian { get; set; } = true;

        /// <summary>
        /// 优先选择临时 IPv6 地址（仅在启用 IPv6 时有效，为安全起见默认开启）
        /// </summary>
        public bool PreferTemporaryIPv6 { get; set; } = true;
    }
}
