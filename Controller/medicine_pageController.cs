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
namespace DB2VM.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class medicine_pageController : ControllerBase
    {
        [Route("storage_list")]
        [HttpGet()]
        public string Get_storage_list(string? src_storehouse)
        {
            if (src_storehouse.StringIsEmpty()) return "";
            string str = "";
            if(src_storehouse == "1")
            {
                str = Basic.Net.WEBApiGet("http://10.14.16.50:443/api/medicine_page/storage_list");
            }
            if (src_storehouse == "2")
            {
                str = Basic.Net.WEBApiGet("http://10.14.16.49:443/api/medicine_page/storage_list");
            }
            return str;
        }
    }
}
