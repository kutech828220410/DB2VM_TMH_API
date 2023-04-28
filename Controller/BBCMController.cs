using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IBM.Data.DB2.Core;
using System.Data;
using System.Configuration;
using Basic;
using SQLUI;
using System.Xml;
using System.Net;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;

namespace DB2VM.Controller
{
    [Route("dbvm/[controller]")]
    [ApiController]
    public class BBCMController : ControllerBase
    {
        public class class_BBCM_data
        {
            private List<sub_class_BBCM_data> _med_data = new List<sub_class_BBCM_data>();

            public List<sub_class_BBCM_data> med_data { get => _med_data; set => _med_data = value; }

            public class sub_class_BBCM_data
            {
                [JsonPropertyName("藥品碼")]
                public string 藥品碼 { get; set; }
                [JsonPropertyName("藥品名稱")]
                public string 藥品名稱 { get; set; }
                [JsonPropertyName("藥品學名")]
                public string 藥品學名 { get; set; }
                [JsonPropertyName("中文名稱")]
                public string 中文名稱 { get; set; }
                [JsonPropertyName("健保碼")]
                public string 健保碼 { get; set; }
                [JsonPropertyName("藥品條碼")]
                public string 藥品條碼 { get; set; }
                [JsonPropertyName("類別")]
                public string 類別 { get; set; }
                [JsonPropertyName("包裝單位")]
                public string 包裝單位 { get; set; }
                [JsonPropertyName("警訊藥品")]
                public string 警訊藥品 { get; set; }
                [JsonPropertyName("管制級別")]
                public string 管制級別 { get; set; }
            }

           
        }

        public enum enum_雲端藥檔
        {
            GUID,
            藥品碼,
            中文名稱,
            藥品名稱,
            藥品學名,
            健保碼,
            包裝單位,
            包裝數量,
            最小包裝單位,
            最小包裝數量,
            藥品條碼1,
            藥品條碼2,
            警訊藥品,
            管制級別,
            類別,
        }

        static string MySQL_server = $"{ConfigurationManager.AppSettings["MySQL_server"]}";
        static string MySQL_database = $"{ConfigurationManager.AppSettings["MySQL_database"]}";
        static string MySQL_userid = $"{ConfigurationManager.AppSettings["MySQL_user"]}";
        static string MySQL_password = $"{ConfigurationManager.AppSettings["MySQL_password"]}";
        static string MySQL_port = $"{ConfigurationManager.AppSettings["MySQL_port"]}";

        private SQLControl sQLControl_藥檔資料 = new SQLControl(MySQL_server, MySQL_database, "medicine_page_cloud", MySQL_userid, MySQL_password, (uint)MySQL_port.StringToInt32(), MySql.Data.MySqlClient.MySqlSslMode.None);


        [HttpGet]
        public string Get(string Code)
        {
            if (Code.StringIsEmpty()) return "[]";
            System.Text.StringBuilder soap = new System.Text.StringBuilder();
 
            soap.Append("<?xml version=\"1.0\" encoding=\"utf - 8\"?>");
            soap.Append("<soap:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'>");
            soap.Append("<soap:Body>");
            soap.Append("<get_met_data_single xmlns='http://tempuri.org/'>");
            soap.Append($"<arg_med_code>{Code}</arg_med_code>");
            soap.Append($"</get_met_data_single>");
            soap.Append("</soap:Body>");
            soap.Append("</soap:Envelope>");


          
            string Xml = Basic.Net.WebServicePost("http://203.71.89.114:9999/T_MED_WS/WebService1.asmx?op=get_met_data_single", soap);
            string[] Node_array = new string[] { "soap:Body", "get_met_data_singleResponse"};

            XmlElement xmlElement = Xml.Xml_GetElement(Node_array);
            string Result = xmlElement.Xml_GetInnerXml("get_met_data_singleResult");

            class_BBCM_data class_BBCM_Data = Result.JsonDeserializet<class_BBCM_data>();

            if (class_BBCM_Data.med_data.Count == 0) return "[]";
            List<MedClass> medClasses = new List<MedClass>();
            MedClass medClass = new MedClass();
            medClass.藥品碼 = class_BBCM_Data.med_data[0].藥品碼;
            medClass.藥品名稱 = class_BBCM_Data.med_data[0].藥品名稱;
            medClass.藥品學名 = class_BBCM_Data.med_data[0].藥品學名;
            medClass.中文名稱 = class_BBCM_Data.med_data[0].中文名稱;
            medClass.包裝單位 = class_BBCM_Data.med_data[0].包裝單位;
            medClass.藥品條碼1 = class_BBCM_Data.med_data[0].藥品條碼;
            medClass.警訊藥品 = (class_BBCM_Data.med_data[0].警訊藥品 == "Y") ? "True" : "False";
            medClass.管制級別 = class_BBCM_Data.med_data[0].管制級別;
            medClass.類別 = class_BBCM_Data.med_data[0].類別;

            medClasses.Add(medClass);

            List<object[]> list_藥檔資料 = sQLControl_藥檔資料.GetRowsByDefult(null, (int)enum_雲端藥檔.藥品碼, medClass.藥品碼);
            if (list_藥檔資料.Count == 0)
            {
                object[] value = new object[new enum_雲端藥檔().GetLength()];
                value[(int)enum_雲端藥檔.GUID] = Guid.NewGuid().ToString();
                value[(int)enum_雲端藥檔.藥品碼] = medClass.藥品碼;
                value[(int)enum_雲端藥檔.藥品名稱] = medClass.藥品名稱;
                value[(int)enum_雲端藥檔.藥品學名] = medClass.藥品學名;
                value[(int)enum_雲端藥檔.警訊藥品] = medClass.警訊藥品;
                value[(int)enum_雲端藥檔.中文名稱] = medClass.中文名稱;
                value[(int)enum_雲端藥檔.包裝單位] = medClass.包裝單位;
                value[(int)enum_雲端藥檔.藥品條碼1] = medClass.藥品條碼1;
                value[(int)enum_雲端藥檔.類別] = medClass.類別;
                value[(int)enum_雲端藥檔.管制級別] = medClass.管制級別;
                sQLControl_藥檔資料.AddRow(null, value);
            }
            else
            {
                object[] value = list_藥檔資料[0];
                value[(int)enum_雲端藥檔.藥品碼] = medClass.藥品碼;
                value[(int)enum_雲端藥檔.藥品名稱] = medClass.藥品名稱;
                value[(int)enum_雲端藥檔.藥品學名] = medClass.藥品學名;
                value[(int)enum_雲端藥檔.警訊藥品] = medClass.警訊藥品;
                value[(int)enum_雲端藥檔.中文名稱] = medClass.中文名稱;
                value[(int)enum_雲端藥檔.包裝單位] = medClass.包裝單位;
                value[(int)enum_雲端藥檔.藥品條碼1] = medClass.藥品條碼1;
                value[(int)enum_雲端藥檔.類別] = medClass.類別;
                value[(int)enum_雲端藥檔.管制級別] = medClass.管制級別;
                List<object[]> list = new List<object[]>();
                list.Add(value);
                sQLControl_藥檔資料.UpdateByDefulteExtra(null, list);
            }


            if (medClasses.Count == 0) return "[]";
            string jsonString = medClasses.JsonSerializationt();
            return jsonString;
        }
    }
}
