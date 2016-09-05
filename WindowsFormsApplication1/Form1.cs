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
            //Create webclient that sends http request to http://localhost:8099/GetAvailableFilenames
            //Once the list has been returned, populate the listBox in form2
            //Available files, directly query server.
            string getFilesUrl = "http://localhost:8080/Service1.svc/GetAvailableFilenames" ;
            //opens new window-
            WebRequest addRequest = WebRequest.Create(getFilesUrl);
            addRequest.Method = "GET";
            WebResponse res = addRequest.GetResponse();

     
            Stream resStream = res.GetResponseStream();


            StreamReader readStream = new StreamReader(resStream, Encoding.UTF8);
            
            string response = readStream.ReadToEnd();
            List<string> files = response.Split('\"').ToList();
            
            var files2 = files.Where(x => x.Length > 3).ToList();
            listBox1.DataSource = files2;

            //Form2 form2 = new Form2(files2);
            //form2.Show();
            
        }
        private void listBox_Click(object sender, EventArgs e)
        {
            //This gives us the item name we want to download!
            //Extract the name and then send the request to http://localhost:8099/Service1.svc/DownloadFile/{filename}
            var item = listBox1.SelectedItem;
            //Set up the request
            WebRequest request = WebRequest.Create(
                "http://localhost:8099/Service1.svc/downloadFile/" + item);
            request.Method = "GET";
            //Send request
            WebResponse response = request.GetResponse();
            //Get the stream
            Stream responseStream = response.GetResponseStream();
            StreamReader readStream = new StreamReader(responseStream, Encoding.UTF8);
            var result = readStream.ReadToEnd();
            if(result != null)
            {
                displayFile(result, item);
            }
            

        }
        private void displayFile(string result, object filename)
        {
            //This method displays the content of the file once it has been received,
            //with the header of the form being that of the filename
            Form2 form2 = new Form2(result, filename);
            //before showing the result, write the file to disk
            string path = "C:\\711\\downloaded\\"
                + (string)filename;
            //TODO: Change implementation to use: WriteAllBytes
            System.IO.File.WriteAllText(path, result);
            //To check if write was successfull
            if (File.Exists(path)){
                MessageBox.Show("File was written to " + path );
                form2.Show();
            }
            else
            {
                MessageBox.Show("For some reason the file didn't get written :(");
            }
                
            
             
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        /* private void OnSelected(object sender, RoutedEventArgs e)
{
    ListBoxItem lbi = e.Source as ListBoxItem;

    if (lbi != null)
    {
        label1.Content = lbi.Content.ToString() + " is selected.";
    }
*/
    }
    
}
