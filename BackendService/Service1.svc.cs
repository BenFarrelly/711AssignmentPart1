using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using System.IO;

namespace BackendService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public string EchoWithGet(string s)
        {
            return "You said " + s;
        }

        public string EchoWithPost(string s)
        {
            return "You said " + s;
        }

        public List<string> GetAvailableFilenames()
        {
            string path = "C:\\711\\files";
            if (Directory.Exists(path))
            {
                string[] availableFiles = Directory.GetFiles(path);
                List<string> filesList = new List<string>();
                foreach(string s in availableFiles)
                {
                    filesList.Add(Path.GetFileName(s));
                }
                availableFiles.ToList();
                return filesList;
            }
            else
            {
                Console.WriteLine("Could not find " + path);
                return null;
            }
           


        }

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
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

        public Stream downloadFile(string fileName)
        {
            //Sends text file encoded into JSON to the client
            //file: Filename in string format.
            string downloadFilePath = "C:\\711\\files\\" + fileName;
            if (File.Exists(downloadFilePath))
            {
                //This line might not be needed -- assumed file is already there
               // File.Create(downloadFilePath);

                string headerInfo = "attachment; filename=" + fileName;
                WebOperationContext.Current.OutgoingResponse.Headers["Content-Disposition"]
                    = headerInfo;

                WebOperationContext.Current.OutgoingResponse.ContentType
                    = "application/octet-stream";

                return File.OpenRead(downloadFilePath);
            }
            return null;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            WebServiceHost host = new WebServiceHost(typeof(Service1), new Uri("http://localhost:8080/"));
            ServiceEndpoint ep = host.AddServiceEndpoint(typeof(Service1), new WebHttpBinding(), "");
            try
            {
                host.Open();
                Console.WriteLine("Service is running:");
                Console.WriteLine("Press enter to quit...");
                Console.ReadLine();
                host.Close();
            }
            catch(CommunicationException e)
            {
                Console.WriteLine("An exception occured: {0}", e.Message);
                host.Abort();
            }
        }
    }
}
