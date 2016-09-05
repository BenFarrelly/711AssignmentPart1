using CacheService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            WebServiceHost host = new WebServiceHost(typeof(Service1),
                new Uri("http://localhost:8099/"));
            ServiceEndpoint ep = host.AddServiceEndpoint(typeof(Service1),
                new WebHttpBinding(), "");
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
