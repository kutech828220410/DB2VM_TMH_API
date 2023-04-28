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
using Oracle.ManagedDataAccess.Client;
using System.Xml;
namespace DB2VM
{
    [Route("dbvm/[controller]")]
    [ApiController]
    public class BBARController : ControllerBase
    {
        public enum enum_醫囑資料
        {
            GUID,
            PRI_KEY,
            藥局代碼,
            藥袋條碼,
            藥品碼,
            藥品名稱,
            病人姓名,
            病歷號,
            交易量,
            開方日期,
            產出時間,
            過帳時間,
            狀態,
        }
        public enum enum_急診藥袋
        {
            本次領藥號,
            看診日期,
            病歷號,
            序號,
            頻率,
            途徑,
            總量,
            前次領藥號,
            本次醫令序號,
        }





        static string MySQL_server = $"{ConfigurationManager.AppSettings["MySQL_server"]}";
        static string MySQL_database = $"{ConfigurationManager.AppSettings["MySQL_database"]}";
        static string MySQL_userid = $"{ConfigurationManager.AppSettings["MySQL_user"]}";
        static string MySQL_password = $"{ConfigurationManager.AppSettings["MySQL_password"]}";
        static string MySQL_port = $"{ConfigurationManager.AppSettings["MySQL_port"]}";

        private SQLControl sQLControl_UDSDBBCM = new SQLControl(MySQL_server, MySQL_database, "UDSDBBCM", MySQL_userid, MySQL_password, (uint)MySQL_port.StringToInt32(), MySql.Data.MySqlClient.MySqlSslMode.None);
        private SQLControl sQLControl_醫囑資料 = new SQLControl(MySQL_server, MySQL_database, "order_list", MySQL_userid, MySQL_password, (uint)MySQL_port.StringToInt32(), MySql.Data.MySqlClient.MySqlSslMode.None);

        [HttpGet]
        public string Get(string? BarCode)
        {

            if (BarCode.StringIsEmpty()) return "[]";
            System.Text.StringBuilder soap = new System.Text.StringBuilder();

            soap.Append("<?xml version=\"1.0\" encoding=\"utf - 8\"?>");
            soap.Append("<soap:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'>");
            soap.Append("<soap:Body>");
            soap.Append("<get_patient_order xmlns='http://tempuri.org/'>");
            soap.Append($"<arg_fee_no>{BarCode}</arg_fee_no>");
            soap.Append($"</get_patient_order>");
            soap.Append("</soap:Body>");
            soap.Append("</soap:Envelope>");



            string Xml = Basic.Net.WebServicePost("http://203.71.89.114:9999/T_MED_WS/WebService1.asmx?op=get_patient_order", soap);
            string[] Node_array = new string[] { "soap:Body", "get_patient_orderResponse" };


            XmlElement xmlElement = Xml.Xml_GetElement(Node_array);
            string Result = xmlElement.Xml_GetInnerXml("get_patient_orderResult");
            class_order_data class_Order_Data = Result.JsonDeserializet<class_order_data>();

            string jsonstring = "";
            string commandText = "";
            List<OrderClass> orderClasses = new List<OrderClass>();
            List<object[]> list_value_Add = new List<object[]>();
            for (int i = 0; i < class_Order_Data.order_data.Count; i++)
            {
                OrderClass orderClass = new OrderClass();
                orderClass.PRI_KEY = class_Order_Data.order_data[i].PRI_KEY.ToString().Trim();
                orderClass.藥局代碼 = "癌症藥局";
                orderClass.處方序號 = "";
                orderClass.藥袋條碼 = $"{BarCode}";
                orderClass.藥品碼 = class_Order_Data.order_data[i].藥品碼;
                orderClass.藥品名稱 = class_Order_Data.order_data[i].藥品名稱;
                orderClass.病人姓名 = class_Order_Data.order_data[i].病人名稱;
                orderClass.病歷號 = class_Order_Data.order_data[i].病歷號;
                orderClass.包裝單位 = class_Order_Data.order_data[i].包裝單位;

                orderClass.交易量 = class_Order_Data.order_data[i].異動量.StringToInt32().ToString();
                string Time = class_Order_Data.order_data[i].開方日期.Trim();
                if (Time.Length == 7)
                {
                    string Year = (Time.Substring(0, 3).StringToInt32() + 1911).ToString();
                    string Month = Time.Substring(3, 2);
                    string Day = Time.Substring(5, 2);
                    string Hour = "00";
                    string Min = "00";
                    string Sec = "00";
                    orderClass.開方時間 = $"{Year}/{Month}/{Day} {Hour}:{Min}:{Sec}";
                }

                orderClasses.Add(orderClass);

            }


            for (int i = 0; i < orderClasses.Count; i++)
            {
                List<object[]> list_value = this.sQLControl_醫囑資料.GetRowsByDefult(null, enum_醫囑資料.PRI_KEY.GetEnumName(), orderClasses[i].PRI_KEY);
                if (list_value.Count == 0)
                {
                    object[] value = new object[new enum_醫囑資料().GetLength()];
                    value[(int)enum_醫囑資料.GUID] = Guid.NewGuid().ToString();
                    value[(int)enum_醫囑資料.PRI_KEY] = orderClasses[i].PRI_KEY;
                    value[(int)enum_醫囑資料.藥局代碼] = orderClasses[i].藥局代碼;
                    value[(int)enum_醫囑資料.藥品碼] = orderClasses[i].藥品碼;
                    value[(int)enum_醫囑資料.藥品名稱] = orderClasses[i].藥品名稱;
                    value[(int)enum_醫囑資料.病歷號] = orderClasses[i].病歷號;
                    value[(int)enum_醫囑資料.藥袋條碼] = orderClasses[i].藥袋條碼;
                    value[(int)enum_醫囑資料.病人姓名] = orderClasses[i].病人姓名;
                    value[(int)enum_醫囑資料.交易量] = orderClasses[i].交易量;
                    value[(int)enum_醫囑資料.開方日期] = orderClasses[i].開方時間;
                    value[(int)enum_醫囑資料.產出時間] = DateTime.Now.ToDateTimeString_6();
                    value[(int)enum_醫囑資料.過帳時間] = DateTime.MinValue.ToDateTimeString_6();

                    value[(int)enum_醫囑資料.狀態] = "未過帳";
                    list_value_Add.Add(value);
                }
            }

            if (list_value_Add.Count > 0)
            {
                this.sQLControl_醫囑資料.AddRows(null, list_value_Add);
            }
            return orderClasses.JsonSerializationt();
        }


    }
}

