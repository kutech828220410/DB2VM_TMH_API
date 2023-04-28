using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
namespace DB2VM
{
    public class class_order_data
    {

        private List<sub_order_data> _order_data = new List<sub_order_data>();

        public List<sub_order_data> order_data { get => _order_data; set => _order_data = value; }

        public class sub_order_data
        {
            private string _藥品碼 = "";
            private string _藥品名稱 = "";
            private string _包裝單位 = "";
            private string _異動量 = "";
            private string _病歷號 = "";
            private string _開方日期 = "";
            private string pRI_KEY = "";     
            private string _病人名稱 = "";

            public string 藥品碼 { get => _藥品碼; set => _藥品碼 = value; }
            public string 藥品名稱 { get => _藥品名稱; set => _藥品名稱 = value; }
            public string 包裝單位 { get => _包裝單位; set => _包裝單位 = value; }
            public string 異動量 { get => _異動量; set => _異動量 = value; }
            public string 病歷號 { get => _病歷號; set => _病歷號 = value; }
            public string 開方日期 { get => _開方日期; set => _開方日期 = value; }
            public string PRI_KEY { get => pRI_KEY; set => pRI_KEY = value; }     
            public string 病人名稱 { get => _病人名稱; set => _病人名稱 = value; }
        }
    }

    [Serializable]
    public class OrderClass
    {
        private string _藥局代碼 = "";
        private string _藥品碼 = "";
        private string _藥品名稱 = "";
        private string _包裝單位 = "";
        private string _交易量 = "";
        private string _病歷號 = "";
        private string _開方時間 = "";
        private string pRI_KEY = "";
        private string _藥袋條碼 = "";
        private string _劑量 = "";
        private string _頻次 = "";
        private string _途徑 = "";
        private string _天數 = "";
        private string _處方序號 = "";
        private string _病人姓名 = "";

        public string 藥局代碼 { get => _藥局代碼; set => _藥局代碼 = value; }
        public string 藥品碼 { get => _藥品碼; set => _藥品碼 = value; }
        public string 藥品名稱 { get => _藥品名稱; set => _藥品名稱 = value; }
        public string 包裝單位 { get => _包裝單位; set => _包裝單位 = value; }
        public string 交易量 { get => _交易量; set => _交易量 = value; }
        public string 病歷號 { get => _病歷號; set => _病歷號 = value; }
        public string 開方時間 { get => _開方時間; set => _開方時間 = value; }
        public string PRI_KEY { get => pRI_KEY; set => pRI_KEY = value; }
        public string 藥袋條碼 { get => _藥袋條碼; set => _藥袋條碼 = value; }
        public string 劑量 { get => _劑量; set => _劑量 = value; }
        public string 頻次 { get => _頻次; set => _頻次 = value; }
        public string 途徑 { get => _途徑; set => _途徑 = value; }
        public string 天數 { get => _天數; set => _天數 = value; }
        public string 處方序號 { get => _處方序號; set => _處方序號 = value; }
        public string 病人姓名 { get => _病人姓名; set => _病人姓名 = value; }
    }
}
