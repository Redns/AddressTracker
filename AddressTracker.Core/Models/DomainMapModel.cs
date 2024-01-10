using AddressTracker.Core.Helpers;
using AddressTracker.Core.Settings;

namespace AddressTracker.Core.Models
{
    /// <summary>
    /// 域名映射模型
    /// </summary>
    public class DomainMapModel
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable {  get; set; }

        /// <summary>
        /// 物理地址
        /// </summary>
        private string? _physicalAddress;
        public required string PhysicalAddress
        {
            get
            {
                return _physicalAddress ??= string.Empty;
            }

            set
            {
                _physicalAddress = value.PhysicalAddressFormatted();
            }
        }

        /// <summary>
        /// 是否为 IPv4
        /// </summary>
        public bool IsIPv4 { get; set; } = true;

        /// <summary>
        /// 绑定的域名
        /// </summary>
        public required List<Domain> Domains { get; set; }

        /// <summary>
        /// 域名映射设置（为 null 则使用全局设置）
        /// </summary>
        public DomainMapSetting? Setting { get; set; }
    }

    public class Domain
    {
        /// <summary>
        /// 域名
        /// </summary>
        public required List<string> Names { get; set; }

        /// <summary>
        /// 托管商
        /// </summary>
        public required Guid CustodianAccountId { get; set; }
    }
}
