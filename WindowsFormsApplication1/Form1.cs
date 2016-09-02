using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void FindFiles(object sender, EventArgs e)
        {
            //show Dialog and get result.
            Stream myStream = null;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = "C:\\cached_files";
            ofd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            ofd.FilterIndex = 2;
            ofd.RestoreDirectory = true;

            if(ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if((myStream = ofd.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            //Reading the stream here, will create a method to display the contents of a 
                            //text file
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from server");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //TODO: Change implementation to use list of filenames!
            //Create webclient that sends http request to http://localhost:8080/GetAvailableFilenames
            //Once the list has been returned, populate the listBox in form2

            string getFilesUrl = "http://localhost:8080/Service1.svc/GetAvailableFilenames" ;
            //opens new window-
            WebRequest addRequest = WebRequest.Create(getFilesUrl);
            addRequest.Method = "GET";
            WebResponse res = addRequest.GetResponse();

     
            Stream resStream = res.GetResponseStream();


            StreamReader readStream = new StreamReader(resStream, Encoding.UTF8);
            
            string response = readStream.ReadToEnd();
            
    
            Form2 form2 = new Form2();
            form2.Show();
            
        }
    }
}
