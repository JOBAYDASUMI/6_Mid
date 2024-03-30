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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.dataGridView1.AutoGenerateColumns = false;
            BindData();
        }

        private void BindData()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString))
            {
                DataSet ds = new DataSet();
                using (SqlDataAdapter da = new SqlDataAdapter("select * from employees", con))
                {
                    da.Fill(ds);
                    ds.Tables[0].Columns.Add(new DataColumn("image", typeof(byte[])));

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dr["image"] =
                            File.ReadAllBytes(
                                Path.Combine(Path.GetFullPath(@"..\.."),
                                "Pictures",
                                dr["picture"].ToString()));
                    }
                    dataGridView1.DataSource = ds.Tables[0];
                }
            }
        }
    }
}
