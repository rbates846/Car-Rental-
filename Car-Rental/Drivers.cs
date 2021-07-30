using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Car_Rental
{
    public partial class Drivers : Form
    {
        public Drivers()
        {
            InitializeComponent();

            FillCombo();
        }

        void FillCombo()
        {
            string sql = "Select * from tbl_vehicle";
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataReader myreader;
            try
            {
                con.Open();
                myreader = cmd.ExecuteReader();
                while (myreader.Read())
                {
                    string Type = myreader.GetString(1);
                    comboBox1.Items.Add(Type);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            HomePage h = new HomePage();
            h.Show();
            this.Hide();
        }

        private void Drivers_Load(object sender, EventArgs e)
        {
            GetDriversRecord();
        }

        SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-TU2KL502\SQLEXPRESS;Initial Catalog=carRental;Integrated Security=True");

        public int Driver_Id;

        private void GetDriversRecord()
        {
            SqlCommand cmd = new SqlCommand("Select * from tbl_drivers", con);
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
                SqlCommand cmd = new SqlCommand("Insert into tbl_drivers values (@Vehicle_T, @Name, @Age, @License_N, @Tel_No)", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Vehicle_T", comboBox1.Text);
                cmd.Parameters.AddWithValue("@Name", textBox1.Text);
                cmd.Parameters.AddWithValue("@Age", numericUpDown1.Value);
                cmd.Parameters.AddWithValue("@License_N", textBox2.Text);
                cmd.Parameters.AddWithValue("@Tel_No", textBox3.Text);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("New Customer Successfully Inserted", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
               
                GetDriversRecord();

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
            comboBox1.Text = string.Empty;
            textBox1.Clear();
            numericUpDown1.Value = 0;
            textBox2.Clear();
            textBox3.Clear();

            textBox1.Focus();

        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Driver_Id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            comboBox1.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            numericUpDown1.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Driver_Id > 0)
            {
                SqlCommand cmd = new SqlCommand("UPDATE tbl_drivers SET Vehicle_T = @Vehicle_T, Name = @Name, License_N = @License_N, Age = @Age, Tel_No = @Tel_No  WHERE Driver_Id = @Driver_Id", con);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@Vehicle_T", comboBox1.Text);
                cmd.Parameters.AddWithValue("@Name", textBox1.Text);
                cmd.Parameters.AddWithValue("@Age", numericUpDown1.Value);
                cmd.Parameters.AddWithValue("@License_N", textBox2.Text);
                cmd.Parameters.AddWithValue("@Tel_No", textBox3.Text);


                cmd.Parameters.AddWithValue("@Driver_Id", this.Driver_Id);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Driver Updated Successfully", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);

                GetDriversRecord();

                ResetFormControls();
            }
            else
            {
                MessageBox.Show("Select Driver to Update", "Select", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (Driver_Id > 0)
            {
                if (MessageBox.Show("Are you sure want to delete?", "Delete Record", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand("DELETE From tbl_drivers WHERE Driver_Id = @Driver_Id", con);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@Driver_Id", this.Driver_Id);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();


                    GetDriversRecord();

                    ResetFormControls();
                }
            }
            else
            {
                MessageBox.Show("Select Driver to Delete", "Select", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            string keyword = textBox6.Text;
            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbl_drivers WHERE Vehicle_T LIKE '%" + keyword + "%' OR Name LIKE '%" + keyword + "%' OR Age LIKE '%" + keyword + "%' OR License_N LIKE '%" + keyword + "%' OR Tel_N LIKE '%" + keyword + "%'", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.DataSource = dt;
        }
    }
}
