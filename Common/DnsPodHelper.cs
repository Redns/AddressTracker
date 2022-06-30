using AddressTracker.Common.DnsPodResponse;
using Newtonsoft.Json;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Dnspod.V20210323;
using TencentCloud.Dnspod.V20210323.Models;

namespace AddressTracker.Common
{
    public class DnsPodHelper
    {
        /// <summary>
        /// 获取 domain_id
        /// </summary>
        /// <param name="domain">二级域名</param>
        /// <param name="loginToken">登录令牌(格式为:ID,Token)</param>
        /// <param name="format"></param>
        /// <returns></returns>
        public async static Task<int> GetDomainID(string domain, string loginToken, string format = "json")
        {
            // 构造请求参数
            var url = "https://dnsapi.cn/Domain.List";
            var paramDict = new Dictionary<string, string>()
            {
                { "login_token", loginToken },
                { "format", format }
            };

            try
            {
                // 创建并发送 HTTP 请求
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new FormUrlEncodedContent(paramDict)
                };
                var response = await client.SendAsync(request);

                // 解析请求结果
                var responseObject = JsonConvert.DeserializeObject<DomainIdResponse>(await response.Content.ReadAsStringAsync());
                if((responseObject is not null) && (responseObject.domains.Any()))
                {
                    foreach(var d in responseObject.domains)
                    {
                        if (d.punycode == domain) { return d.id; }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"[Get domain_id failed]{ex.Message}");
            }
            return 0;
        }
    
        
        /// <summary>
        /// 获取 record_id
        /// </summary>
        /// <param name="domainId">domain_id</param>
        /// <param name="subDomain">子域名</param>
        /// <param name="loginToken">登录令牌</param>
        /// <param name="format"></param>
        /// <returns></returns>
        public async static Task<string> GetRecordId(int domainId, string subDomain, string loginToken, string format = "json")
        {
            // 构造请求参数
            var url = "https://dnsapi.cn/Record.List";
            var paramDict = new Dictionary<string, string>()
            {
                { "login_token", loginToken },
                { "format", format },
                { "domain_id", $"{domainId}" }
            };

            try
            {
                // 创建并发送 HTTP 请求
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new FormUrlEncodedContent(paramDict)
                };
                var response = await client.SendAsync(request);

                // 解析请求结果
                var responseObject = JsonConvert.DeserializeObject<RecordIdResponse>(await response.Content.ReadAsStringAsync());
                if((responseObject is not null) && responseObject.records.Any())
                {
                    foreach (var record in responseObject.records)
                    {
                        if(record.name == subDomain)
                        {
                            return record.id;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Get record_id failed]{ex.Message}");
            }
            return string.Empty;
        }


        /// <summary>
        /// 修改 DNS 记录
        /// </summary>
        /// <param name="secretId"></param>
        /// <param name="secretKey"></param>
        /// <param name="domain">二级域名</param>
        /// <param name="subDomain">子域名</param>
        /// <param name="recordLine">记录线路，通过 API 记录线路获得</param>
        /// <param name="value">记录值</param>
        /// <param name="recordId">record_id</param>
        /// <param name="recordType">记录类型</param>
        /// <returns></returns>
        public static void ModifyRecord(string secretId, string secretKey, string domain, string subDomain, string value, ulong recordId, string recordLine = "默认", string recordType = "A")
        {
            Credential cred = new()
            {
                SecretId = secretId,
                SecretKey = secretKey
            };

            ClientProfile clientProfile = new();
            HttpProfile httpProfile = new()
            {
                Endpoint = ("dnspod.tencentcloudapi.com")
            };
            clientProfile.HttpProfile = httpProfile;

            DnspodClient client = new(cred, "", clientProfile);
            ModifyRecordRequest req = new()
            {
                Domain = domain,
                SubDomain = subDomain,
                RecordType = recordType,
                RecordLine = recordLine,
                Value = value,
                RecordId = recordId
            };
            ModifyRecordResponse resp = client.ModifyRecordSync(req);
        }
    }
}
