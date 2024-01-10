using System.Net.NetworkInformation;
using System.Net;

namespace AddressTracker.Core.Helpers
{
    public static class NetworkHelper
    {
        /// <summary>
        /// 判断物理地址是否合法
        /// </summary>
        /// <param name="physicalAddress"></param>
        /// <returns></returns>
        public static bool IsPhysicalAddressLegal(this string physicalAddress)
        {
            if (string.IsNullOrEmpty(physicalAddress))
            {
                return false;
            }

            // 检查是否含有非法字符
            physicalAddress = physicalAddress.ToUpper();
            if (physicalAddress.Any(a => (a != '-') && (a != ':') && (a < 'A' || a > 'Z') && (a < '0' || a > '9')))
            {
                return false;
            }

            // 判断格式是否正确
            var physicalAddressSlices = physicalAddress.ToUpper().Split(physicalAddress.Contains('-') ? '-' : ':');
            if ((physicalAddressSlices.Length > 1) && physicalAddressSlices.Any(a => a.Length != 2))
            {
                return false;
            }

            // 判断地址长度是否正确
            physicalAddress = physicalAddress.Replace("-", string.Empty).Replace(":", string.Empty);
            if(physicalAddress.Length != 12)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 格式化物理地址
        /// </summary>
        /// <param name="physicalAddress">待格式化的物理地址</param>
        /// <returns></returns>
        public static string PhysicalAddressFormatted(this string physicalAddress)
        {
            if(!physicalAddress.IsPhysicalAddressLegal())
            {
                throw new ArgumentException($"Physical address resolution fails, {physicalAddress} does not meet the standard format", nameof(physicalAddress));
            }

            return physicalAddress.Replace("-", string.Empty).Replace(":", string.Empty).ToUpper();
        }

        /// <summary>
        /// 判断域名是否合法
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public static bool IsDomainLegel(this string domain)
        {
            if (string.IsNullOrEmpty(domain))
            {
                return false;
            }

            // 检查域名是否含有非法字符
            domain = domain.ToLower();
            for (int i = 0; i < domain.Length; i++)
            {
                if ((domain[i] >= 'a' && domain[i] <= 'z') || (domain[i] >= '0' && domain[i] <= '9') || (domain[i] == '-') || (domain[i] == '.'))
                {
                    continue;
                }
                return false;
            }

            //  检查域名格式是否正确
            var domainSlices = domain.Split('.');
            if (domainSlices.Length < 2)
            {
                return false;
            }

            foreach (var slice in domainSlices)
            {
                if (string.IsNullOrEmpty(slice))
                {
                    return false;
                }
            }
            
            return true;
        }

        /// <summary>
        /// 解析域名
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public static (string priDomain, string subDomain) ParseDomain(this string domain)
        {
            if (!IsDomainLegel(domain))
            {
                throw new ArgumentException($"Domain resolution fails, {domain} does not meet the standard format", nameof(domain));
            }

            var domainSlices = domain.ToLower().Split('.');
            if (domainSlices.Length == 2)
            {
                return (domain, "@");
            }
            return (domain[(domainSlices[0].Length + 1)..], domainSlices[0]);
        }

        /// <summary>
        /// 获取所有网络接口模型
        /// </summary>
        /// <returns></returns>
        public static NetworkInterfaceModel[] GetAllInterfaces()
        {
            return Array.ConvertAll(NetworkInterface.GetAllNetworkInterfaces(), networkInterface => new NetworkInterfaceModel(networkInterface));
        }
    }

    /// <summary>
    /// 网络接口
    /// </summary>
    public class NetworkInterfaceModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 网络接口类型 (WLAN、LAN、PPPoE...)
        /// </summary>
        public NetworkInterfaceType Type { get; set; }

        /// <summary>
        /// 物理地址
        /// </summary>
        public string PhysicalAddress { get; set; } = string.Empty;

        /// <summary>
        /// 网络接口状态（启用/关闭）
        /// </summary>
        public OperationalStatus Status { get; set; }

        /// <summary>
        /// 链接速度 (bps)
        /// </summary>
        private long _linkSpeed = 0L;
        public long LinkSpeed
        {
            get { return _linkSpeed; }
            set
            {
                // 网络接口关闭时 Speed 值不可用
                _linkSpeed = Status is OperationalStatus.Down ? 0L : value;

                if (_linkSpeed == 0)
                {
                    _linkSpeedFormatted = "*";
                }
                else if (_linkSpeed < 1024)
                {
                    _linkSpeedFormatted = $"{_linkSpeed}bps";
                }
                else if (_linkSpeed < 1024 * 1024)
                {
                    _linkSpeedFormatted = $"{_linkSpeed / 1024}Kbps";
                }
                else if (_linkSpeed < 1024 * 1024 * 1024)
                {
                    _linkSpeedFormatted = $"{_linkSpeed / (1024 * 1024)}Mbps";
                }
                else
                {
                    _linkSpeedFormatted = $"{_linkSpeed / (1024 * 1024 * 1024)}Gbps";
                }
            }
        }

        /// <summary>
        /// 链接速度（格式化后的）
        /// </summary>
        private string _linkSpeedFormatted = string.Empty;
        public string LinkSpeedFormatted
        {
            get => _linkSpeedFormatted;
        }

        /// <summary>
        /// 协议栈支持 (None/IPv4/IPv6/Both)
        /// </summary>
        public NetworkProtocolStack NetworkProtocolStack { get; set; } = NetworkProtocolStack.None;

        /// <summary>
        /// IP 地址列表
        /// </summary>
        public List<IPAddress> IPAddresses { get; set; } = [];

        public NetworkInterfaceModel(NetworkInterface networkInterface)
        {
            Id = networkInterface.Id;
            Name = networkInterface.Name;
            Description = networkInterface.Description;
            Type = networkInterface.NetworkInterfaceType;
            PhysicalAddress = networkInterface.GetPhysicalAddress().ToString();
            Status = networkInterface.OperationalStatus;
            LinkSpeed = networkInterface.Speed;
            NetworkProtocolStack = (networkInterface.Supports(NetworkInterfaceComponent.IPv4), networkInterface.Supports(NetworkInterfaceComponent.IPv6)) switch
            {
                (true, false) => NetworkProtocolStack.IPv4,
                (false, true) => NetworkProtocolStack.IPv6,
                (true, true) => NetworkProtocolStack.Both,
                _ => NetworkProtocolStack.None
            };

            // 获取网络接口地址
            if (networkInterface.GetIPProperties().UnicastAddresses.Count > 0)
            {
                foreach (var unicastAddress in networkInterface.GetIPProperties().UnicastAddresses)
                {
                    IPAddresses.Add(unicastAddress.Address);
                }
            }
        }
    }

    /// <summary>
    /// 网络协议栈
    /// </summary>
    public enum NetworkProtocolStack
    {
        None = 0,       // 均不支持
        IPv4,           // 仅支持 IPv4
        IPv6,           // 仅支持 IPv6
        Both            // 同时支持 IPv4 和 IPv6
    }
}
