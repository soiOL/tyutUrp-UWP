using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace urp.Struct
{
    public class Info
    {
        public string stuCode { get; set; }
        public string name { get; set; }
        public string sex { get; set; }
        public string birthDay { get; set; }
        public string country { get; set; }
        public string nation { get; set; }
        public string userDepartment { get; set; }
        public string status { get; set; }
        public string balance { get; set; }
        public string accountId { get; set; }
        public string bankCode { get; set; }
        public string idCard { get; set; }
        public string role { get; set; }
        public string crDate { get; set; }
        public string avDate { get; set; }
        public string memo1 { get; set; }
        public string memo2 { get; set; }
        public string memo3 { get; set; }
        public string memo4 { get; set; }
        public string memo5 { get; set; }
        public string memo6 { get; set; }
        public string memo7 { get; set; }
        public string memo8 { get; set; }
        public string memo9 { get; set; }
        public string memo10 { get; set; }
        public string pic { get; set; }
    }

    public class EcardInfo
    {
        public string total { get; set; }
        public string resultCode { get; set; }
        public List<Info> value { get; set; }
    }
}
