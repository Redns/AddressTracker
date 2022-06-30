using log4net;

namespace AddressTracker.Common
{
    public class GlobalValues
    {
        public static AppSetting? AppSetting { get; set; }
        public static readonly ILog Logger = LogManager.GetLogger("AddressTracker");

        /// <summary>
        /// 初始化系统配置
        /// </summary>
        /// <returns></returns>
        public static bool Init()
        {
            // 配置 log4net 配置文件路径
            log4net.Config.XmlConfigurator.Configure(configFile:new FileInfo("log4net.config"));
            
            // 加载程序配置文件 appsettings.json
            if ((AppSetting = AppSetting.Load()) is not null) { return true; }
            else { return false; }
        }
    }
}
