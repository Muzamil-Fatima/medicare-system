using Org.BouncyCastle.Asn1.X500;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClinicMangSystem
{
    public partial class BillingSystem: BaseForm
    {
        SqlConnection conn = new SqlConnection("Data Source =DESKTOP-JLQCCR6; Initial Catalog =ClinicManagement; Integrated Security = True");
        SqlCommand cmd;
        SqlDataReader rdr;

        // Store Service Name & ID
        Dictionary<string, string> serviceData = new Dictionary<string, string>();

        public BillingSystem()
        {
            InitializeComponent();
            LoadServices();
            LoadBillingRecords();
        }
        public BaseForm MidParent { get; internal set; }
        private void BillingSystem_Load(object sender, EventArgs e)
        {

        }
        // Load Services into ComboBox and Store IDs
        private void LoadServices()
        {

            try
            {
                if (conn.State == ConnectionState.Open) conn.Close(); // Ensure connection is closed before opening
                conn.Open();

                string sql = "Select serviceId, serviceName from tblServices";
                cmd = new SqlCommand(sql, conn);
                rdr = cmd.ExecuteReader();

                comboBox1.Items.Clear();
                serviceData.Clear();

                while (rdr.Read())
                {
                    string serviceId = rdr["serviceId"].ToString();
                    string serviceName = rdr["serviceName"].ToString();
                    comboBox1.Items.Add(serviceName);
                    serviceData[serviceName] = serviceId;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading services: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close(); // Close connection at the end
            }
        }
        // Load Billing Records into DataGridView
        private void LoadBillingRecords()
        {
            try
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Open();

                string sql = "Select * from tblBillInfo";
                cmd = new SqlCommand(sql, conn);
                rdr = cmd.ExecuteReader();
                dataGridView1.Rows.Clear();

                while (rdr.Read())
                {
                    dataGridView1.Rows.Add(rdr["patientId"], rdr["patientName"], rdr["contactNumber"], rdr["serviceName"],
                                           rdr["serviceId"], rdr["doctorName"], rdr["doctorFee"],
                                           rdr["serviceCost"], rdr["totalAmount"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading billing records: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }
        private void CalculateTotalAmount()
        {
            decimal doctorFee, serviceCost;

            if (decimal.TryParse(textBox6.Text, out doctorFee) && decimal.TryParse(textBox8.Text, out serviceCost))
            {
                textBox9.Text = (doctorFee + serviceCost).ToString();
            }
            else
            {
                textBox9.Text = "0"; // Default to 0 if parsing fails
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Open();

                string sql = "insert into tblBillInfo (patientId, patientName, contactNumber, serviceName, serviceId, doctorName, doctorFee, serviceCost, totalAmount) " +
                             "Values (@pid, @pname, @contact, @sname, @sid, @dname, @dfee, @scost, @total)";
                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@pid", textBox1.Text);
                cmd.Parameters.AddWithValue("@pname", textBox2.Text);
                cmd.Parameters.AddWithValue("@contact", textBox3.Text);
                cmd.Parameters.AddWithValue("@sname", comboBox1.Text);
                cmd.Parameters.AddWithValue("@sid", textBox4.Text);
                cmd.Parameters.AddWithValue("@dname", textBox5.Text);
                cmd.Parameters.AddWithValue("@dfee", textBox6.Text);
                cmd.Parameters.AddWithValue("@scost", textBox8.Text);
                cmd.Parameters.AddWithValue("@total", textBox9.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Record Added Successfully!");

                LoadBillingRecords();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding record: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close(); // Ensure connection is closed
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Open();

                string sql = "Update tblBillInfo set patientName=@pname, contactNumber=@contact, serviceName=@sname, serviceId=@sid, doctorName=@dname, doctorFee=@dfee, serviceCost=@scost, totalAmount=@total " +
                             "where patientId=@pid";
                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@pid", textBox1.Text);
                cmd.Parameters.AddWithValue("@pname", textBox2.Text);
                cmd.Parameters.AddWithValue("@contact", textBox3.Text);
                cmd.Parameters.AddWithValue("@sname", comboBox1.Text);
                cmd.Parameters.AddWithValue("@sid", textBox4.Text);
                cmd.Parameters.AddWithValue("@dname", textBox5.Text);
                cmd.Parameters.AddWithValue("@dfee", textBox6.Text);
                cmd.Parameters.AddWithValue("@scost", textBox8.Text);
                cmd.Parameters.AddWithValue("@total", textBox9.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Record Updated Successfully!");

                LoadBillingRecords();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating record: " + ex.Message);
            }

            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }

        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Open();

                string sql = "Delete from tblBillInfo where patientId=@pid";
                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@pid", textBox1.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Record Deleted Successfully!");

                LoadBillingRecords();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting record: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox8.Clear();
            textBox9.Clear();
            comboBox1.Text = "";
            textBox1.Focus();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                string sql = "Select serviceId from tblServices where serviceName = @serviceName";
                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@serviceName", comboBox1.Text);

                object result = cmd.ExecuteScalar(); // Get single value (serviceId)

                if (result != null)
                {
                    textBox4.Text = result.ToString(); // Set Service ID in TextBox4
                }
                else
                {
                    textBox4.Text = ""; // Clear if no match
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching service ID: " + ex.Message);
            }
        }
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalAmount();
        }
        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalAmount();
        }
    }
}
