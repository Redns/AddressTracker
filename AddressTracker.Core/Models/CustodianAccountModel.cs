using AddressTracker.Core.Factories;
using AddressTracker.Core.Hosts;
using System.Text.Json.Serialization;

namespace AddressTracker.Core.Models
{
    /// <summary>
    /// 域名托管商账户
    /// </summary>
    public class CustodianAccountModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 域名托管商
        /// </summary>
        public required CustodianCode CustodianCode { get; set; }

        /// <summary>
        /// 认证对象
        /// </summary>
        public required string AuthenticationToken { get; set; }

        private Custodian? _custodian;
        [JsonIgnore] public Custodian Custodian
        {
            get
            {
                return _custodian ??= CustodianFactory.CreateHost(this);
            }
        }
    }
    
    /// <summary>
    /// 域名托管商
    /// </summary>
    public enum CustodianCode
    {
        DnsPod = 0,         // DnsPod
        Aliyun,             // 阿里云
        Qiniuyun            // 七牛云
    }
}
