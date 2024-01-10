using AddressTracker.Core.Helpers;

namespace AddressTracker.Core.Hosts
{
    public abstract class Custodian
    {
        /// <summary>
        /// 授权令牌
        /// </summary>
        protected string _authorizationToken;
        protected Custodian(string authorizationToken)
        {
            _authorizationToken = authorizationToken;
        }

        /// <summary>
        /// 添加域名
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public abstract ValueTask<bool> AddDomainAsync(string domain);

        /// <summary>
        /// 获取托管的所有域名
        /// </summary>
        /// <returns></returns>
        public abstract ValueTask<IEnumerable<string>> GetAllDomainsAsync();

        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public abstract ValueTask<bool> AddRecordAsync(DomainRecord record);

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public abstract ValueTask<bool> UpdateRecordAsync(DomainRecord record);
    }

    /// <summary>
    /// 域名记录
    /// </summary>
    public class DomainRecord
    {
        /// <summary>
        /// 域名
        /// </summary>
        public string PriDomain { get; set; }

        /// <summary>
        /// 子域名
        /// </summary>
        public string SubDomain { get; set; }

        /// <summary>
        /// 是否为 IPv6 解析记录
        /// </summary>
        public bool IsIPv4Record { get; set; } = true;

        /// <summary>
        /// 记录线路
        /// </summary>
        public string Line { get; set; } = string.Empty;

        /// <summary>
        /// 记录值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public ulong MX { get; set; } = 10L;

        /// <summary>
        /// TTL
        /// </summary>
        public ulong TTL { get; set; } = 600L;

        /// <summary>
        /// 权重
        /// </summary>
        public ulong Weight { get; set; } = 0L;

        /// <summary>
        /// 状态
        /// </summary>
        public DomainRecordStatus Status { get; set; } = DomainRecordStatus.Enable;

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = string.Empty;

        public DomainRecord(string domain, bool isIPv6Record, string line, string value, ulong mx, ulong ttl, ulong weight, DomainRecordStatus status, string remark)
        {
            (PriDomain, SubDomain) = domain.ParseDomain();
            IsIPv4Record = isIPv6Record;
            Line = line;
            Value = value;
            MX = mx;
            TTL = ttl;
            Weight = weight;
            Status = status;
            Remark = remark;
        }

        public DomainRecord(string domain, string value)
        {
            (PriDomain, SubDomain) = domain.ParseDomain();
            Value = value;
        }
    }

    /// <summary>
    /// 域名记录状态
    /// </summary>
    public enum DomainRecordStatus
    {
        Enable = 0,
        Disable
    }
}
