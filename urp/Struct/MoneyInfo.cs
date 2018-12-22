using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace urp.Struct
{
    public class Value
    {
        public string ConsumeTime { get; set; }
        public string Area { get; set; }
        public string TradeBranchName { get; set; }
        public string ClientNo { get; set; }
        public string GeneralOperateTypeName { get; set; }
        public string ConsumeAmount { get; set; }
    }

    public class MoneyInfo
    {
        public string total { get; set; }
        public string resultCode { get; set; }
        public List<Value> value { get; set; }
    }
}
