using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO; 

namespace EasyChat
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((textBox1.Text != "") && (textBox1.Text != " ")&& (textBox2.Text != "") && (textBox2.Text != " "))
            {
                try
                {
                    DirectoryInfo data = new DirectoryInfo("ClientInfo");
                    data.Create();

                    var sw = new StreamWriter(@"ClientInfo/dataInfo.txt");
                    sw.WriteLine(textBox1.Text + ":" + textBox2.Text);
                    sw.Close();
                    this.Close();
                    Application.Restart();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Ошибка" + ex.Message);
                }
                
            }
        }
    }
}
