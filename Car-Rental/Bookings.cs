using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Car_Rental
{
    public partial class Bookings : Form
    {
        public Bookings()
        {
            InitializeComponent();
            FillCombo();
            FillCombo1();
            FillCombo2();
        }

        void FillCombo()
        {
            string sql = "Select * from tbl_customers";
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataReader myreader;
            try
            {
                con.Open();
                myreader = cmd.ExecuteReader();
                while (myreader.Read())
                {
                    string Name = myreader.GetString(1);
                    comboBox1.Items.Add(Name);
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

        void FillCombo1()
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
                    string Plate_N = myreader.GetString(3);
                    comboBox2.Items.Add(Plate_N);
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

        void FillCombo2()
        {
            string sql = "Select * from tbl_drivers";
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataReader myreader;
            try
            {
                con.Open();
                myreader = cmd.ExecuteReader();
                while (myreader.Read())
                {
                    string Name = myreader.GetString(2);
                    comboBox3.Items.Add(Name);
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (IsValid())
            {
                SqlCommand cmd = new SqlCommand("Insert into tbl_bookings values (@Customer, @Vehicle_N, @Driver, @No_D)", con);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@Customer", comboBox1.Text);
                cmd.Parameters.AddWithValue("@Vehicle_N", comboBox2.Text);      
                cmd.Parameters.AddWithValue("@Driver", comboBox3.Text);
                cmd.Parameters.AddWithValue("@No_D", numericUpDown1.Value);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("New Booking Successfully Inserted", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                GetBookingsRecord();

                ResetFormControls();
            }
            
        }

        private bool IsValid()
        {
            if (comboBox1.Text == string.Empty)
            {
                MessageBox.Show("Customer Name is Required", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void Bookings_Load(object sender, EventArgs e)
        {
            GetBookingsRecord();
        }
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True");

        public int Booking_Id;

        private void GetBookingsRecord()
        {
            SqlCommand cmd = new SqlCommand("Select * from tbl_bookings", con);
            DataTable dt = new DataTable();

            con.Open();

            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();

            dataGridView1.DataSource = dt;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ResetFormControls();
        }

        private void ResetFormControls()
        {
            comboBox1.Text = string.Empty;
            comboBox2.Text = string.Empty;
            comboBox3.Text = string.Empty;
            numericUpDown1.Value = 0;

            comboBox1.Focus();
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Booking_Id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            comboBox1.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            comboBox2.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            comboBox3.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            numericUpDown1.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Booking_Id > 0)
            {
                SqlCommand cmd = new SqlCommand("UPDATE tbl_bookings SET Customer = @Customer, Vehicle_N = @Vehicle_N, Driver = @Driver WHERE Booking_Id = @Booking_Id", con);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@Customer", comboBox1.Text);
                cmd.Parameters.AddWithValue("@Vehicle_N", comboBox2.Text);
                cmd.Parameters.AddWithValue("@Driver", comboBox3.Text);
                cmd.Parameters.AddWithValue("@No_D", numericUpDown1.Value);


                cmd.Parameters.AddWithValue("@Booking_Id", this.Booking_Id);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Booking Updated Successfully", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);

                GetBookingsRecord();

                ResetFormControls();
            }
            else
            {
                MessageBox.Show("Select Booking to Update", "Select", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (Booking_Id > 0)
            {
                if (MessageBox.Show("Are you sure want to delete?", "Delete Record", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand("DELETE From tbl_bookings WHERE Booking_Id = @Booking_Id", con);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@Booking_Id", this.Booking_Id);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();


                    GetBookingsRecord();

                    ResetFormControls();
                }
            }
            else
            {
                MessageBox.Show("Select Booking to Delete", "Select", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            string keyword = textBox6.Text;
            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbl_bookings WHERE Customer LIKE '%" + keyword + "%' OR Vehicle_N LIKE '%" + keyword + "%' OR Driver LIKE '%" + keyword + "%' OR No_D LIKE '%" + keyword + "%'", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.DataSource = dt;
        }
    }
}
