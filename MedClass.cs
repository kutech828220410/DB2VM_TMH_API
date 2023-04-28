using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB2VM
{
    public class MedClass
    {
        private string _藥品碼 = "";
        private string _藥品名稱 = "";
        private string _藥品學名 = "";
        private string _中文名稱 = "";
        private string _包裝單位 = "";
        private string _警訊藥品 = "";
        private string _管制級別 = "";
        private string _料號 = "";
        private string _ATC主碼 = "";
        private string _藥品條碼1 = "";
        private string _藥品條碼2 = "";
        private string _類別 = "";

        public string 藥品碼 { get => _藥品碼; set => _藥品碼 = value; }
        public string 藥品名稱 { get => _藥品名稱; set => _藥品名稱 = value; }
        public string 料號 { get => _料號; set => _料號 = value; }
        public string ATC主碼 { get => _ATC主碼; set => _ATC主碼 = value; }
        public string 藥品條碼1 { get => _藥品條碼1; set => _藥品條碼1 = value; }
        public string 藥品條碼2 { get => _藥品條碼2; set => _藥品條碼2 = value; }
        public string 警訊藥品 { get => _警訊藥品; set => _警訊藥品 = value; }
        public string 管制級別 { get => _管制級別; set => _管制級別 = value; }
        public string 藥品學名 { get => _藥品學名; set => _藥品學名 = value; }
        public string 包裝單位 { get => _包裝單位; set => _包裝單位 = value; }
        public string 中文名稱 { get => _中文名稱; set => _中文名稱 = value; }
        public string 類別 { get => _類別; set => _類別 = value; }
    }
}
