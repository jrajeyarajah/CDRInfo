using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ProcessCDRFile;

namespace CDRInfo.Controllers
{
    public class FileController : ApiController
    {
        [Route("api/File/Upload")]
        [HttpPost]
        public HttpResponseMessage Upload()
        {
            //Create the Directory.
            string path = HttpContext.Current.Server.MapPath("~/Upload/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (HttpContext.Current.Request.Files.Count == 1)
            {
                //Fetch the File.
                HttpPostedFile postedFile = HttpContext.Current.Request.Files[0];

                //Save the File.
                postedFile.SaveAs(path + postedFile.FileName);

                CDRDataProcessing processingCDR = new CDRDataProcessing();

                processingCDR.readFile(path + postedFile.FileName);

                //Send OK Response to Client.
                return Request.CreateResponse(HttpStatusCode.OK, postedFile.FileName);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
