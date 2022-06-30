namespace AddressTracker.Common.DnsPodResponse
{
    public class DomainIdResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public Status status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Info info { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<DomainsItem> domains { get; set; }
    }


    public class Status
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

    public class Info
    {
        /// <summary>
        /// 
        /// </summary>
        public int domain_total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int all_total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int mine_total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int share_total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int vip_total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ismark_total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int pause_total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int error_total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int lock_total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int spam_total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int vip_expire { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int share_out_total { get; set; }
    }

    public class DomainsItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string grade { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string group_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string searchengine_push { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string is_mark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ttl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string cname_speedup { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string created_on { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string updated_on { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string punycode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ext_status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string src_flag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> grade_ns { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int grade_level { get; set; }
        /// <summary>
        /// 免费版
        /// </summary>
        public string grade_title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string is_vip { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string owner { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string records { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string is_grace_period { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string vip_start_at { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string vip_end_at { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string vip_auto_renew { get; set; }
    }
}
