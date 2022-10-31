using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CDRInfo.Controllers
{
    public class MostExpensiveController : ApiController
    {
        [HttpGet]
        public IEnumerable<CDRData> Get([FromUri]string callerId, int number=5 )
        {
            CDRDataLayer cdrdataLayer = new CDRDataLayer();
            //cdrdataLayer.sql = "select * from CDRData where currency ='GBP' Order by Cost desc";
            //List<CDRData> cdrDatas = cdrdataLayer.CdrDatas.Where(d => d.caller_id.Equals(callerId) && d.currency.ToUpper().Equals("GBP")).OrderByDescending(x => x.cost).Take(5).ToList();
            List<CDRData> cdrDatas = cdrdataLayer.CdrDatas.Where(d => d.caller_id.Equals(callerId) && d.currency.ToUpper().Equals("GBP")).OrderByDescending(x => x.cost).Take(number).ToList();
            return cdrDatas;
        }
    }
}
