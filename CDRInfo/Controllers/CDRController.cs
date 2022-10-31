using DataLayer;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace CDRInfo.Controllers
{
    public class CDRController : ApiController
    {
        public IEnumerable<CDRData> Get()
        {
            CDRDataLayer cdrdataLayer = new CDRDataLayer();
            List<CDRData> cdrDatas = cdrdataLayer.CdrDatas.ToList();
            return cdrDatas;
        }

        [HttpGet]
        public IEnumerable<CDRData> Get([FromUri]string callerId)
        {
            CDRDataLayer cdrdataLayer = new CDRDataLayer();
            List<CDRData> cdrDatas = cdrdataLayer.CdrDatas.Where(d => d.caller_id.Equals(callerId)).ToList();
            return cdrDatas;
        }

        [HttpGet]
        public IEnumerable<CDRData> GetReference([FromUri]string reference)
        {
            CDRDataLayer cdrdataLayer = new CDRDataLayer();
            List<CDRData> cdrDatas = cdrdataLayer.CdrDatas.Where(d => d.reference.Equals(reference)).ToList();
            return cdrDatas;
        }

    }
}
