//#define DEBUG

using AddressTracker.Core.Models;
using AddressTracker.Core.Settings;
using System.Text.Json;

namespace AddressTracker.Common
{
    public class AppSetting
    {
        /// <summary>
        /// 通用设置
        /// </summary>
        public GeneralSetting General { get; set; }

        /// <summary>
        /// 托管商账户
        /// </summary>
        private List<CustodianAccountModel>? _custodianAccountModels;
        public List<CustodianAccountModel> CustodianAccounts
        {
            get
            {
                return _custodianAccountModels ??= [];
            }

            set
            {
                _custodianAccountModels = value.DistinctBy(a => a.Id).ToList();
            }
        }

        /// <summary>
        /// 域名映射模型集合
        /// </summary>
        public List<DomainMapModel> DomainMapModels { get; set; }

        public AppSetting(GeneralSetting general, List<CustodianAccountModel> custodianAccounts, List<DomainMapModel> domainMapModels)
        {
            General = general;
            CustodianAccounts = custodianAccounts;
            DomainMapModels = domainMapModels;
        }

        /// <summary>
        /// 加载设置
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static AppSetting? Load(string? path = null)
        {
            if (string.IsNullOrEmpty(path))
            {
#if DEBUG
                path = "appsettings-development.json";
#else
                    path = "appsettings.json";
#endif
            }
            return JsonSerializer.Deserialize<AppSetting>(File.ReadAllText(path));
        }

        /// <summary>
        /// 保存设置
        /// </summary>
        /// <param name="path"></param>
        public void Save(string? path = null)
        {
            if (string.IsNullOrEmpty(path))
            {
#if DEBUG
                path = "appsettings-development.json";
#else
                    path = "appsettings.json";
#endif
            }
            File.WriteAllText(path, JsonSerializer.Serialize(this));
        }
    }
}
