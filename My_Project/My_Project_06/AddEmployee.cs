using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace My_Project_06
{
    public partial class AddEmployee : Form
    {
        string img = "";
        public AddEmployee()
        {
            InitializeComponent();
        }
        public Form SyncForm { get;set; }

        private void AddEmployee_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.img = this.openFileDialog1.FileName;
                this.pictureBox1.Image = Image.FromFile(this.openFileDialog1.FileName);
            }
        }
        private void AddProductForm_Load(object sender, EventArgs e)
        {
            GetId();
        }

        private void GetId()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString))
            {
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("select ISNULL(MAX(id), 0)+1 from products", con))
                {
                    con.Open();
                    int id = (int)cmd.ExecuteScalar();
                    con.Close();
                    textBox1.Text = id.ToString();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (img == "") return;
            string ext = Path.GetExtension(img);
            string filename = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
            string savename = Path.Combine(Path.GetFullPath(@"..\..\Pictures"), filename);
            File.Copy(img, savename, true);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString))
            {
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand(@"INSERT INTO products(id, name, unitprice, mfgdate, picture,available) 
                                        VALUES (@i, @n,@u,@m,@p, @a)", con))
                {
                    cmd.Parameters.AddWithValue("@i", textBox1.Text);
                    cmd.Parameters.AddWithValue("@n", textBox2.Text);
                    cmd.Parameters.AddWithValue("@u", textBox3.Text);
                    cmd.Parameters.AddWithValue("@m", dateTimePicker1.Value);
                    cmd.Parameters.AddWithValue("@a", checkBox1.Checked);
                    cmd.Parameters.AddWithValue("@p", filename);
                    con.Open();
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Data inserted", "Success");
                    }
                    con.Close();
                    GetId();
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                    dateTimePicker1.Value = DateTime.Now;
                    checkBox1.Checked = false;
                    pictureBox1.Image = null;
                    img = "";
                    this.SyncForm.BindData();
                }
            }
        }
    }
}
