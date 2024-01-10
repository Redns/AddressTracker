using TencentCloud.Common;
using TencentCloud.Dnspod.V20210323;
using TencentCloud.Dnspod.V20210323.Models;

namespace AddressTracker.Core.Hosts
{
    public class DnsPodCustodian : Custodian
    {
        /// <summary>
        /// 请求客户端
        /// </summary>
        private readonly DnspodClient _dnsPodClient;
        public DnsPodCustodian(string authorizationToken) : base(authorizationToken)
        {
            var authorizationTokenSlices = authorizationToken.Split('-');
            if (authorizationTokenSlices.Length != 2)
            {
                throw new ArgumentException("Failed to parse DnsPod authorization token, secretId and secretKey must split with &", nameof(authorizationToken));
            }

            _dnsPodClient = new DnspodClient(new Credential
            {
                SecretId = authorizationTokenSlices[0],
                SecretKey = authorizationTokenSlices[1]
            }, string.Empty);
        }

        /// <summary>
        /// 添加域名
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public override async ValueTask<bool> AddDomainAsync(string domain)
        {
            var createDomainResp = await _dnsPodClient.CreateDomain(new CreateDomainRequest
            { 
                Domain = domain 
            });

            if(createDomainResp.DomainInfo.Domain != domain)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public override async ValueTask<bool> AddRecordAsync(DomainRecord record)
        {
            await _dnsPodClient.CreateRecord(new CreateRecordRequest
            {
                Domain = record.PriDomain,
                SubDomain = record.SubDomain,
                RecordType = record.IsIPv4Record ? "AAAA" : "A",
                RecordLine = string.IsNullOrEmpty(record.Line) ? "默认" : record.Line,
                Value = record.Value,
                MX = record.MX,
                TTL = record.TTL,
                Weight = record.Weight,
                Status = record.Status == DomainRecordStatus.Enable ? "ENABLE" : "DISABLE",
                Remark = record.Remark
            });
            return true;
        }

        /// <summary>
        /// 获取托管的所有域名
        /// </summary>
        /// <returns></returns>
        public override async ValueTask<IEnumerable<string>> GetAllDomainsAsync()
        {
            var resp = await _dnsPodClient.DescribeDomainList(new DescribeDomainListRequest());
            if (resp.DomainList.Length == 0)
            {
                return Enumerable.Empty<string>();
            }
            return Array.ConvertAll(resp.DomainList, domain => domain.Name);
        }

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public override async ValueTask<bool> UpdateRecordAsync(DomainRecord record)
        {
            await _dnsPodClient.ModifyRecord(new ModifyRecordRequest
            {
                Domain = record.PriDomain,
                SubDomain = record.SubDomain,
                RecordId = await GetRecordId(record.PriDomain, record.SubDomain),
                RecordType = record.IsIPv4Record ? "A" : "AAAA",
                RecordLine = string.IsNullOrEmpty(record.Line) ? "默认" : record.Line,
                Value = record.Value,
                MX = record.MX,
                TTL = record.TTL,
                Weight = record.Weight,
                Status = record.Status == DomainRecordStatus.Enable ? "ENABLE" : "DISABLE",
                Remark = record.Remark
            });
            return true;
        }

        /// <summary>
        /// 获取域名解析记录 ID
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ArgumentException"></exception>
        private async ValueTask<ulong> GetRecordId(string priDomain, string subDomain)
        {
            var resp = await _dnsPodClient.DescribeRecordList(new DescribeRecordListRequest
            {
                Domain = priDomain,
                Subdomain = subDomain
            });

            if (resp.RecordList.Length == 0)
            {
                throw new Exception($"Resolution of domain {priDomain}.{subDomain} does not exist");
            }

            return resp.RecordList.First().RecordId ?? throw new ArgumentException($"Failed to obtain the DNS record ID of domain {priDomain}.{subDomain}", nameof(resp.RecordList));
        }
    }
}
