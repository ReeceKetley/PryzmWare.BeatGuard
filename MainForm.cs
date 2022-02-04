using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeatGuard_Test
{
    public partial class MainForm : Form
    {
        private string file;
        public MainForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = Guid.NewGuid().ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".wav";
            ofd.ShowDialog();
            file = ofd.FileName;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
