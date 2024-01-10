using AddressTracker.Common;
using AddressTracker.Core.Helpers;
using AddressTracker.Core.Hosts;
using AddressTracker.Core.Models;
using log4net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace AddressTracker
{
    class Program
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        private static ILog? _logger;
        public static ILog Logger
        {
            get
            {
                if (_logger is null)
                {
                    _logger = LogManager.GetLogger("*");
                    log4net.Config.XmlConfigurator.Configure(configFile: new FileInfo("log4net.config"));
                }
                return _logger;
            }
        }

        public static DateTime? NetworkInterfaceLastModifyTime { get; set; }

        public static async Task Main()
        {
            // 加载设置
            var appSetting = AppSetting.Load();
            if (appSetting is null)
            {
                Logger.Error("Failed to load appsettings.json"); return;
            }

            // 配置域名映射设置
            appSetting.DomainMapModels = appSetting.DomainMapModels.Where(m => m.IsEnable).ToList();
            if (appSetting.DomainMapModels.Count == 0)
            {
                Logger.Error("No domain mapping configuration enabled, application has exited"); return;
            }

            appSetting.DomainMapModels.ForEach(m =>
            {
                m.Setting ??= appSetting.General.DomainMap;
            });

            await UpdataAddressAsync(appSetting.CustodianAccounts, appSetting.DomainMapModels);
            NetworkChange.NetworkAddressChanged += async (sender, args) =>
            {
                await UpdataAddressAsync(appSetting.CustodianAccounts, appSetting.DomainMapModels);
            };

            while (true)
            {
                new AutoResetEvent(false).WaitOne();
            }
        }

        public static async Task UpdataAddressAsync(List<CustodianAccountModel> custodianAccounts, List<DomainMapModel> domainMaps)
        {
            // 网络发生变化时该函数委托可能多次触发
            // 由于 DDNS 强制刷新时间最小间隔为 1 分钟，为了避免潜在的冲突，此处的冷却时间应小于 60 秒
            if((DateTime.Now - NetworkInterfaceLastModifyTime)?.Seconds < 30)
            {
                return;
            }
            NetworkInterfaceLastModifyTime = DateTime.Now;

            // 获取所有网络接口
            var networkInterfaces = NetworkHelper.GetAllInterfaces();
            foreach (var domainMap in domainMaps)
            {
                // 获取域名映射对应的网络接口
                var networkInterface = networkInterfaces.FirstOrDefault(n => (n.Status is OperationalStatus.Up) && (n.PhysicalAddress ==  domainMap.PhysicalAddress));
                if(networkInterface is null)
                {
                    Logger.Error($"No corresponding network interface found ({domainMap.PhysicalAddress}) or network interface is not enabled"); continue;
                }

                // TODO 增加 IP 协议栈检查

                // 获取要映射的 IP 地址
                // TODO 实现 PreferTempoaryIpv6 功能
                var ipAddress = domainMap.IsIPv4 ? networkInterface.IPAddresses.FirstOrDefault(i => i.AddressFamily is AddressFamily.InterNetwork) :
                                                   networkInterface.IPAddresses.FirstOrDefault(i => (i.AddressFamily is AddressFamily.InterNetworkV6) && !i.IsIPv6LinkLocal);
                if (ipAddress is null)
                {
                    Logger.Error($"Failed to obtain valid IP address of physical address {domainMap.PhysicalAddress} ({(domainMap.IsIPv4 ? "IPv4" : "IPv6")})"); continue;
                }
                
                foreach(var domain in domainMap.Domains)
                {
                    // 获取域名组对于托管商
                    var custodian = custodianAccounts.FirstOrDefault(c => c.Id == domain.CustodianAccountId)?.Custodian;
                    if (custodian is null)
                    {
                        Logger.Error($"Failed to find the custodian ({domain.CustodianAccountId}), please check whether the configuration is correct"); continue;
                    }

                    // 更新域名
                    var domainNameUpdateTasks = new List<Task>(domain.Names.Count);
                    foreach(var name in domain.Names)
                    {
                        domainNameUpdateTasks.Add(Task.Run(async () =>
                        {
                            var domainNameUpdateResult = await custodian.UpdateRecordAsync(new DomainRecord(name, ipAddress.ToString())
                            {
                                IsIPv4Record = domainMap.IsIPv4
                            });

                            if (domainNameUpdateResult is not true)
                            {
                                Logger.Error($"Failed to update address {ipAddress} of domain {name}");
                            }
                            else
                            {
                                Logger.Info($"Success to update address {ipAddress} of domain {name}");
                            }
                        }));
                    }
                    await Task.WhenAll(domainNameUpdateTasks);
                }
            }
        }
    }
}

