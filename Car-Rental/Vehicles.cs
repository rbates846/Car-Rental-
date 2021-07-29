using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Car_Rental
{
    public partial class Vehicles : Form
    {
        public Vehicles()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            HomePage h = new HomePage();
            h.Show();
            this.Hide(); 
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Vehicles_Load(object sender, EventArgs e)
        {
            GetVehiclesRecord();
        }

        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True");

        public int Id;
        private void GetVehiclesRecord()
        {
            SqlCommand cmd = new SqlCommand("Select * from tbl_vehicle", con);
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
                SqlCommand cmd = new SqlCommand("Insert into tbl_vehicle values (@Type, @Model, @Plate_N, @Year, @Seat_C, @Colour)", con);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@Type", comboBox1.Text);
                cmd.Parameters.AddWithValue("@Model", comboBox2.Text);
                cmd.Parameters.AddWithValue("@Plate_N", textBox1.Text);
                cmd.Parameters.AddWithValue("@Year", textBox2.Text);
                cmd.Parameters.AddWithValue("@Seat_C", numericUpDown1.Value);
                cmd.Parameters.AddWithValue("@Colour", textBox3.Text);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("New Vehicle Successfully Inserted", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                GetVehiclesRecord();

                ResetFormControls();
            }
        }

        private bool IsValid()
        {
            if (comboBox1.Text == string.Empty)
            {
                MessageBox.Show("Type is Required", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            comboBox2.Text = string.Empty;
            textBox1.Clear();
            textBox2.Clear();
            numericUpDown1.Value = 0;
            textBox3.Clear();

            comboBox1.Focus();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Id > 0)
            {
                SqlCommand cmd = new SqlCommand("UPDATE tbl_vehicle SET Type = @Type, Model =  @Model, Plate_N =  @Plate_N, Year =  @Year, Seat_C =  @Seat_C, Colour =  @Colour WHERE Id = @Id", con);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@Type", comboBox1.Text);
                cmd.Parameters.AddWithValue("@Model", comboBox2.Text);
                cmd.Parameters.AddWithValue("@Plate_N", textBox1.Text);
                cmd.Parameters.AddWithValue("@Year", textBox2.Text);
                cmd.Parameters.AddWithValue("@Seat_C", numericUpDown1.Value);
                cmd.Parameters.AddWithValue("@Colour", textBox3.Text);
                cmd.Parameters.AddWithValue("@Id", this.Id);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show(" Vehicle Information Updated Successfully ", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);

                GetVehiclesRecord();

                ResetFormControls();
            }
            else
            {
                MessageBox.Show(" Please Select Vehicle Informations to Open ", "Select?", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            comboBox1.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            comboBox2.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            numericUpDown1.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (Id > 0)
            {
                SqlCommand cmd = new SqlCommand("DELETE from tbl_vehicle WHERE Id = @Id", con);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@Id", this.Id);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show(" Vehicle Information Deleted from the system ", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);

                GetVehiclesRecord();

                ResetFormControls();
            }
            else
            {
                MessageBox.Show(" Please Select Vehicle Informations to Delete ", "Select?", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            string keyword = textBox6.Text;
            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbl_vehicle WHERE Type LIKE '%" + keyword + "%' OR Model LIKE '%" + keyword + "%' OR Plate_N LIKE '%" + keyword + "%' OR Year LIKE '%" + keyword + "%' OR Seat_C LIKE '%" + keyword + "%' OR Colour LIKE '%" + keyword + "%'", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.DataSource = dt;
        }
    }
}
