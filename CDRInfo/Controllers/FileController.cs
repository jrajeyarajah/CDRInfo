using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
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
                using (FileStream fs = File.Open(path + postedFile.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (BufferedStream bs = new BufferedStream(fs))
                using (StreamReader sr = new StreamReader(bs))
                {
                    string line;
                    int lineCount = 0;
                    HttpResponseMessage reponse = new HttpResponseMessage();

                    //DateTime start = DateTime.Now;
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Trim();
                        lineCount++;
                        string errorMsg = processingCDR.ProcessLine(line);
                        if (errorMsg.StartsWith("Error"))
                        {
                            reponse = Request.CreateResponse(HttpStatusCode.BadRequest, string.Format("At Line:{0} {1} {2}", lineCount.ToString(), errorMsg,line));

                            break;
                        }

                    }
                    if (reponse.StatusCode != HttpStatusCode.BadRequest)
                    {
                        reponse = Request.CreateResponse(HttpStatusCode.OK, postedFile.FileName);
                    }
                    return reponse;
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
