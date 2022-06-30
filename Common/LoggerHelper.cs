using log4net;

namespace AddressTracker.Common
{
    public class LoggerHelper
    {
        private readonly ILog logger = LogManager.GetLogger("AddressTracker");

        public static void Info(string message)
        {
            
        }
    }
}
