using Newtonsoft.Json;

namespace AddressTracker.Common
{
    public class AppSetting
    {
        public DnsPod DnsPod { get; set; }
        public TecentCloud TecentCloud { get; set; }
        public Domain Domain { get; set; }
        public General General { get; set; }

        public AppSetting(DnsPod dnsPod, TecentCloud tecentCloud, Domain domain, General general)
        {
            DnsPod = dnsPod;
            TecentCloud = tecentCloud;
            Domain = domain;
            General = general;
        }


        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="path">配置文件路径</param>
        /// <returns></returns>
        public static AppSetting? Load(string path = "appsettings.json")
        {
            return JsonConvert.DeserializeObject<AppSetting>(File.ReadAllText(path));
        }


        /// <summary>
        /// 保存设置
        /// </summary>
        /// <param name="path">设置文件路径</param>
        /// <returns></returns>
        public void Save(string path = "appsettings.json")
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }


    /// <summary>
    /// DnsPod 配置
    /// </summary>
    public class DnsPod
    {
        public string ID { get; set; }
        public string Token { get; set; }

        public DnsPod(string iD, string token)
        {
            ID = iD;
            Token = token;
        }
    }


    /// <summary>
    /// 腾讯云 API 配置
    /// </summary>
    public class TecentCloud
    {
        public string APPID { get; set; }           
        public string SecretId { get; set; }
        public string SecretKey { get; set; }
    }


    /// <summary>
    /// 域名配置
    /// </summary>
    public class Domain
    {
        public string Root { get; set; }
        public string SubDomain { get; set; }

        public Domain(string root, string subDomain)
        {
            Root = root;                // 根域名（二级域名）
            SubDomain = subDomain;      // 子域名
        }
    }


    /// <summary>
    /// 通用配置
    /// </summary>
    public class General
    {
        public long RefreshInterval { get; set; }    // 域名更新间隔
    }
}
