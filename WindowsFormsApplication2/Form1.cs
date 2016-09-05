using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void refresh_Click(object sender, EventArgs e)
        {
            string path = "C:\\711\\cache_log.txt";
            using(StreamReader s = new StreamReader(path))
            {
                string log = s.ReadToEnd();
                richTextBox1.Text = log;
                listBox1.DataSource = HelperMethods.GetAvailableFiles();
            }
        }

        private void clear_Click(object sender, EventArgs e)
        {
            string dir = "C:\\711\\cache\\";
            string[] fileList = Directory.GetFiles(dir);
            foreach(string f in fileList)
            {
                File.Delete(f);
            }
            listBox1.DataSource = null;
        }
      
    }
}
