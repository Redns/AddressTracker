namespace AddressTracker.Common.DnsPodResponse
{
    public class RecordIdResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public RecordIdResponseStatus status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public RecordIdResponseDomain domain { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public RecordIdResponseInfo info { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<RecordsItem> records { get; set; }
    }

    public class RecordIdResponseStatus
    {
        /// <summary>
        /// 
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 操作已经成功完成
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string created_at { get; set; }
    }

    public class RecordIdResponseDomain
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string punycode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string grade { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string owner { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ext_status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ttl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int min_ttl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> dnspod_ns { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string can_handle_at_ns { get; set; }
    }

    public class RecordIdResponseInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string sub_domains { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string record_total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string records_num { get; set; }
    }

    public class RecordsItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ttl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string enabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string updated_on { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 默认
        /// </summary>
        public string line { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string line_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string weight { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string monitor_status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string use_aqb { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string mx { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string hold { get; set; }
    }
}
