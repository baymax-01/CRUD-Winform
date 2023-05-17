using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CRUD
{
    public partial class EmpDetailsForm : Form
    {
        public EmpDetailsForm()
        {
            InitializeComponent();
            BindGridView();
            pictureBox1.Image = Properties.Resources.OIP;
            cmbgender.SelectedIndex = 0;
        }
        string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
        void BindGridView()
        {

            SqlConnection con = new SqlConnection(cs);
            string query = "select * from EmpDetails ";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataTable data = new DataTable();
            sda.Fill(data);
            dataGridView1.DataSource = data;
            DataGridViewImageColumn dgv = new DataGridViewImageColumn();
            dgv = (DataGridViewImageColumn)dataGridView1.Columns[5];
            dgv.ImageLayout = DataGridViewImageCellLayout.Stretch;
            dataGridView1.RowTemplate.Height = 50;
        }
        public void FillGrid(string searchtext)
        {
            int n;
            bool isNumeric = int.TryParse(txtsearch.Text, out n);
            string query = "";
            SqlConnection con = new SqlConnection(cs);

            try
            {
                query = string.Empty;

                if (isNumeric == false)
                {

                    if (string.IsNullOrEmpty(searchtext.Trim()))
                    {
                        query = "select * from empdetails";
                    }
                    else
                    {

                        query = "select  id, name,age,gender,contact,picture from Empdetails where (name+ ' ' +gender) like'%" + searchtext.Trim() + "%'";
                    }
                }
                if (isNumeric)
                {
                    if (string.IsNullOrEmpty(searchtext.Trim()))
                    {
                        query = "select * from empdetails";
                    }
                    else
                    {
                        query = "select  id, name,age,gender,contact,picture from Empdetails where (id+ ' ' +age+' '+contact) like'%" + searchtext.Trim() + "%'";

                    }
                }

                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                DataTable data = new DataTable();
                sda.Fill(data);
                dataGridView1.DataSource = data;


            }
            catch
            {

                MessageBox.Show("Some Issue Occur ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btninsert_Click(object sender, EventArgs e)
        {
            if (cmbgender.SelectedIndex == 0)
            {
                errorProvider1.SetError(this.cmbgender, "Please Select Gender !");
                cmbgender.Focus();
                return;
            }
            SqlConnection con = new SqlConnection(cs);

            string query = "insert into EmpDetails values (@name,@age,@gender,@contact,@pic)";
            //string query = string.Format("insert into empdetails (name,age,gender,contact,picture) values('{0}','{1}','{2}','{3}','{4}')",
            //    txtname.Text,txtage.Text,cmbgender.SelectedItem,txtcontact.Text,SavePhoto());
            SqlCommand cmd = new SqlCommand(query, con);
            //cmd.Parameters.AddWithValue("@id", txtID.Text);
            cmd.Parameters.AddWithValue("@name", txtname.Text);
            cmd.Parameters.AddWithValue("@age", txtage.Text);
            cmd.Parameters.AddWithValue("@gender", cmbgender.Text);
            cmd.Parameters.AddWithValue("@contact", txtcontact.Text);
            cmd.Parameters.AddWithValue("@pic", SavePhoto());
            con.Open();
            int a = cmd.ExecuteNonQuery();
            if (a > 0)
            {
                MessageBox.Show("Inserted Data Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                BindGridView();
                clearControls();
            }
            else
            {
                MessageBox.Show("Insertion Failed", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            con.Close();

        }
        void clearControls()
        {
            txtID.Text = string.Empty;
            txtname.Text = string.Empty;
            txtcontact.Text = string.Empty;
            txtage.Text = string.Empty;
            cmbgender.SelectedIndex = 0;
            pictureBox1.Image = Properties.Resources.OIP;
        }
        private byte[] SavePhoto()
        {
            MemoryStream ms = new MemoryStream();
            pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
            return ms.GetBuffer();
        }
        private Image GetPhoto(byte[] img)
        {
            MemoryStream ms = new MemoryStream(img);
            return Image.FromStream(ms);
        }

        private void UploadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Title = "Upload Image";
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp|All files (*.*)|*.*";
            if (open.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(open.FileName);
            }
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(cs);
            string query = "delete from EmpDetails where ID = @ID";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@ID", txtID.Text);

            con.Open();
            int a = cmd.ExecuteNonQuery();
            if (a > 0)
            {
                MessageBox.Show("Deleted  Data Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                BindGridView();
                clearControls();
            }
            else
            {
                MessageBox.Show("Deletion Failed Failed", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            con.Close();
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            if (cmbgender.SelectedIndex == 0)
            {
                errorProvider1.SetError(this.cmbgender, "Please Select Gender !");
                cmbgender.Focus();
                return;
            }
            SqlConnection con = new SqlConnection(cs);
            string query = "update Empdetails set name=@name,age=@age,gender=@gender,contact=@contact,picture=@pic where id=@id  ";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", txtID.Text);
            cmd.Parameters.AddWithValue("@name", txtname.Text);
            cmd.Parameters.AddWithValue("@age", txtage.Text);
            cmd.Parameters.AddWithValue("@gender", cmbgender.Text);
            cmd.Parameters.AddWithValue("@contact", txtcontact.Text);
            cmd.Parameters.AddWithValue("@pic", SavePhoto());
            con.Open();
            int a = cmd.ExecuteNonQuery();
            if (a > 0)
            {
                MessageBox.Show("Update Data Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                BindGridView();
                clearControls();
            }
            else
            {
                MessageBox.Show("Updation Failed", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            con.Close();
        }

        private void dataGridView1_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                txtID.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                txtname.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                txtage.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                cmbgender.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                txtcontact.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                pictureBox1.Image = GetPhoto((byte[])dataGridView1.SelectedRows[0].Cells[5].Value);
            }
        }

        private void searchtext_TextChanged(object sender, EventArgs e)
        {
            FillGrid(txtsearch.Text.Trim());
        }
    }
}
