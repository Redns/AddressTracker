using System.Net;
using System.Timers;
using AddressTracker.Common;

namespace AddressTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                if (GlobalValues.Init() && (GlobalValues.AppSetting is not null))
                {
                    // 初始化定时器
                    TimerInit(GlobalValues.AppSetting);
                    while (true) { Thread.Sleep(1000 * 60 * 100); }
                }
                GlobalValues.Logger.Error("Failed to load appsettings");
            }
            catch(Exception e)
            {
                GlobalValues.AppSetting?.Save();

                Console.WriteLine("Application abort");
                GlobalValues.Logger.Error("Application abort", e);
            }
        }


        /// <summary>
        /// 初始化定时器
        /// </summary>
        /// <param name="interval">定时间隔（单位：ms）</param>
        private async static void TimerInit(AppSetting settings)
        {
            await RefreshAddress(settings.Domain.Root, 
                                 settings.Domain.SubDomain,
                                 $"{settings.DnsPod.ID},{settings.DnsPod.Token}",
                                 settings.TecentCloud.SecretId,
                                 settings.TecentCloud.SecretKey);

            var timer = new System.Timers.Timer(settings.General.RefreshInterval) 
            { 
                AutoReset = true,
                Enabled = true
            };
            timer.Elapsed += (async (object? source, ElapsedEventArgs e) =>
            {
                await RefreshAddress(settings.Domain.Root,
                                 settings.Domain.SubDomain,
                                 $"{settings.DnsPod.ID},{settings.DnsPod.Token}",
                                 settings.TecentCloud.SecretId,
                                 settings.TecentCloud.SecretKey);
            });
            timer.Start();
        }


        /// <summary>
        /// 更新主机地址
        /// </summary>
        /// <returns></returns>
        private async static Task RefreshAddress(string root, string subDomain, string loginToken, string secretId, string secretKey)
        {
            try
            {
                var address = Dns.GetHostAddresses(Dns.GetHostName(), System.Net.Sockets.AddressFamily.InterNetwork)
                             .Where((ip) => !IsLocalAddress(ip));
                if (address.Any())
                {
                    // 获取 domain_id 和 record_id
                    var domainId = await DnsPodHelper.GetDomainID(root, loginToken);
                    var recordId = await DnsPodHelper.GetRecordId(domainId, subDomain, loginToken);

                    // 更新 DNS 记录
                    DnsPodHelper.ModifyRecord(secretId, secretKey, root, subDomain, address.First().ToString(), ulong.Parse(recordId));

                    Console.WriteLine($"Update address to {address.First()}, current interval is {GlobalValues.AppSetting?.General.RefreshInterval} ms");
                    GlobalValues.Logger.Info($"Update address to {address.First()}");
                }
                else
                {
                    GlobalValues.Logger.Error("Failed to update address, please check your settings or connect to network");
                }
            }
            catch(Exception e)
            {
                GlobalValues.Logger.Error("Failed to update address", e);
            }
        }


        /// <summary>
        /// 判断 IP 是否为本机
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private static bool IsLocalAddress(IPAddress ip)
        {
            return ip.ToString() switch
            {
                "localhost" => true,
                "127.0.0.1" => true,
                "127.0.1.1" => true,
                _ => false
            };
        }
    }
}