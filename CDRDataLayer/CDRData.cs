using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer
{
    public class CDRData
    {
        public string caller_id { get; set; }
        public string recipient { get; set; }
        public DateTime call_date { get; set; }
        public DateTime end_time { get; set; }
        public int? duration { get; set; }
        public Single cost { get; set; }
        public string reference { get; set; }
        public string currency { get; set; }   
        public char type { get; set; }
    }
    
    public class InCDRData
    {
        public string caller_id { get; set; }
        public string recipient { get; set; }
        public string call_date { get; set; }
        public string end_time { get; set; }
        public string duration { get; set; }
        public string cost { get; set; }
        public string reference { get; set; }
        public string currency { get; set; }
        public string type { get; set; }
    }
}