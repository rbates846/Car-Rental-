using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Car_Rental
{
    public partial class Customers : Form
    {
        public Customers()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            HomePage h = new HomePage();
            h.Show();
            this.Hide();
        }

        private void Customers_Load(object sender, EventArgs e)
        {
            GetCustomersRecord();
        }

        SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-TU2KL502\SQLEXPRESS;Initial Catalog=carRental;Integrated Security=True");

        public int Customer_Id;

        private void GetCustomersRecord()
        {
            SqlCommand cmd = new SqlCommand("Select * from tbl_customers", con);
            DataTable dt = new DataTable();

            con.Open();

            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();

            dataGridView1.DataSource = dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (IsValid()) 
            {
                SqlCommand cmd = new SqlCommand("Insert into tbl_customers values (@Name, @Address, @NIC, @Tel_N, @Gender)", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Name", textBox1.Text);
                cmd.Parameters.AddWithValue("@Address", textBox2.Text);
                cmd.Parameters.AddWithValue("@NIC", textBox3.Text);
                cmd.Parameters.AddWithValue("@Tel_N", textBox4.Text);
                cmd.Parameters.AddWithValue("@Gender", textBox5.Text);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("New Customer Successfully Inserted", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                GetCustomersRecord();

                ResetFormControls();
            }
        }

        private bool IsValid()
        {
            if (textBox1.Text == string.Empty)
            {
                MessageBox.Show("Customer Name is Required", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ResetFormControls();
        }

        private void ResetFormControls()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();

            textBox1.Focus();
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Customer_Id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            textBox4.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            textBox5.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Customer_Id > 0)
            {
                SqlCommand cmd = new SqlCommand("UPDATE tbl_customers SET Name = @Name, Address = @Address, NIC = @NIC, Tel_N = @Tel_N, Gender = @Gender  WHERE Customer_Id = @Customer_Id", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Name", textBox1.Text);
                cmd.Parameters.AddWithValue("@Address", textBox2.Text);
                cmd.Parameters.AddWithValue("@NIC", textBox3.Text);
                cmd.Parameters.AddWithValue("@Tel_N", textBox4.Text);
                cmd.Parameters.AddWithValue("@Gender", textBox5.Text);


                cmd.Parameters.AddWithValue("@Customer_Id", this.Customer_Id);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Customer Updated Successfully", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);

                GetCustomersRecord();

                ResetFormControls();
            }
            else
            {
                MessageBox.Show("Select Customer to Update", "Select", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (Customer_Id > 0)
            {
                if (MessageBox.Show("Are you sure want to delete?", "Delete Record", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand("DELETE From tbl_customers WHERE Customer_Id = @Customer_Id", con);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@Customer_Id", this.Customer_Id);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();


                    GetCustomersRecord();

                    ResetFormControls();
                }
            }
            else
            {
                MessageBox.Show("Select Customer to Delete", "Select", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            string keyword = textBox6.Text;
            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbl_customers WHERE Name LIKE '%" + keyword + "%' OR Address LIKE '%" + keyword + "%' OR NIC LIKE '%" + keyword + "%' OR Tel_N LIKE '%" + keyword + "%' OR Gender LIKE '%" + keyword + "%'", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.DataSource = dt;
        }
    }
}
