using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;

namespace CacheService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public Stream downloadFile(string fileName)
        {
            //first check if file is in the cache
            string path = "C:\\711\\cache\\" + fileName;
            using(StreamWriter s = File.AppendText("C:\\711\\cache_log.txt"))
            {
                s.WriteLine("User request, File:" + fileName + " at " + DateTime.Now.ToString());
            }
            
            if (File.Exists(path)) //The path for when the file is in the cache
            {//return the file here
                string headerInfo = "attachment; filename=" + fileName;
                WebOperationContext.Current.OutgoingResponse.Headers["Content-Disposition"]
                    = headerInfo;

                WebOperationContext.Current.OutgoingResponse.ContentType
                    = "application/octet-stream";
                using (StreamWriter s = File.AppendText("C:\\711\\cache_log.txt"))
                {
                    s.WriteLine("Response: sent cached file:" + fileName);
                }
                return File.OpenRead(path);
            } 
            else if (!File.Exists(path))
            {//pass the get request on to the service, download this file to keep on the cache
                //Create request to server
                WebRequest request = WebRequest.Create(
                    "http://localhost:8080/Service1.svc/downloadFile/" + fileName);
                request.Method = "GET";
                //Send response to server
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(responseStream, Encoding.UTF8);
                var result = readStream.ReadToEnd();
                if(result != null)
                {
                    //TODO: Change implementation to use WriteAllBytes
                    System.IO.File.WriteAllText(path, result);
                    if (File.Exists(path))
                    {
                        using (StreamWriter s = File.AppendText("C:\\711\\cache_log.txt"))
                        {
                            s.WriteLine("Response: downloaded file:" + fileName);
                        }
                    }
                    return File.OpenRead(path);
                }
            }
            return null;
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            WebServiceHost host = new WebServiceHost(typeof(Service1), new Uri("http://localhost:8099/"));
            ServiceEndpoint ep = host.AddServiceEndpoint(typeof(Service1), new WebHttpBinding(), "");
            //Create log file
            File.Create("C:\\cache_log.txt");
            try
            {
                host.Open();
                Console.WriteLine("Service is running:");
                Console.WriteLine("Press enter to quit...");
                Console.ReadLine();
                host.Close();
            }
            catch (CommunicationException e)
            {
                Console.WriteLine("An exception occured: {0}", e.Message);
                host.Abort();
            }
        }
    }
}
