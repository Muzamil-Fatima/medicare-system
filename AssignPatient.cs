using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ClinicMangSystem
{
    public partial class AssignPatient : BaseForm
    {
       

        public AssignPatient()
        {
            InitializeComponent();
        }
        public BaseForm MidParent { get; internal set; }
        //-----------------------
        private void dataGridViewAppointments_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure a valid row is clicked
            {
                // Get selected row data
                string patientID = dgvAppointments.Rows[e.RowIndex].Cells["PatientID"].Value.ToString();
                string patientName = dgvAppointments.Rows[e.RowIndex].Cells["PatientName"].Value.ToString();
                string doctorID = txtDoctorID.Text.Trim(); // Get Doctor ID from the text field

                // Fetch doctor name from database
                string doctorName = GetDoctorNameFromDB(doctorID);

                // Open Prescription Form and pass data
                Prescriptions prescriptionForm = new Prescriptions(patientID, patientName, doctorID, doctorName);
                prescriptionForm.Show();
            }
        }
        public void LoadDoctorAppointments(string doctorID)
        {
            using (SqlConnection conn = new SqlConnection(ValidationHelper.ConnectionString))
            {
                string query = @"SELECT a.AppointmentID, p.PatientName, p.PatientID, pc.Phone, ar.ReasonDescription AS ReasonForVisit, 
                                a.AppointmentDate, a.AppointmentTime
                         FROM tblAppointments a
                         INNER JOIN tblPatient p ON a.PatientID = p.PatientID
                         INNER JOIN tblPatientContact pc ON p.PatientID = pc.PatientID
                         INNER JOIN tblAppointmentDetails ad ON a.AppointmentID = ad.AppointmentID
                         INNER JOIN tblAppointmentReason ar ON ad.ReasonID = ar.ReasonID
                         WHERE a.DrID = @DoctorID";

                         SqlCommand cmd = new SqlCommand(query, conn);
                         cmd.Parameters.AddWithValue("@DoctorID", doctorID);
                         SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                         DataTable dt = new DataTable();
                         adapter.Fill(dt);

                         dgvAppointments.DataSource = dt;
            }
        }
        private void LoadPatientData(string doctorID)
        {
        
            using (SqlConnection conn = new SqlConnection(ValidationHelper.ConnectionString))
            {
                string query = @"
                  SELECT 
                  a.AppointmentID,
                  a.CustomAppointmentID,
                  a.PatientID,
                  p.PatientName,
                  c.PatientPhone,
                  a.AppointmentDate,
                  a.AppointmentTime,
                  a.AppointmentStatus,
                  a.AppointmentReason,
                  d.DrName AS DoctorName
                  FROM tblAppointments a
                  INNER JOIN tblPatient p ON a.PatientID = p.PatientID
                  LEFT JOIN tblPatientContact c ON p.PatientID = c.PatientID
                  LEFT JOIN tblDoctor d ON a.DrID = d.DrID
                  WHERE a.DrID = @DoctorID";
                   SqlCommand cmd = new SqlCommand(query, conn);
                   cmd.Parameters.AddWithValue("@DoctorID", doctorID);
                   SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                   DataTable dt = new DataTable();
                   adapter.Fill(dt);
                dgvAppointments.DataSource = dt;
            }
        }
        private string GetDoctorNameFromDB(string doctorID)
        {
            string doctorName = "Unknown";
            using (SqlConnection conn = new SqlConnection(ValidationHelper.ConnectionString))
            {
                string query = "SELECT DrName FROM tblDoctor WHERE DrID = @DrID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DrID", doctorID);
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    doctorName = result.ToString();
                }
            }
            return doctorName;
        }
        private void btnPatients_Click(object sender, EventArgs e)
        {
       
            string doctorID = txtDoctorID.Text.Trim(); // DrID input
            string password = txtDoctorPassword.Text.Trim(); // DrPassword input
            using (SqlConnection conn = new SqlConnection(ValidationHelper.ConnectionString))
            {
                // Validate using DrID and plaintext DrPassword from tblDoctorLogin
                string query = "SELECT COUNT(*) FROM tblDoctorLogin WHERE DrID = @DrID AND DrPassword = @Password AND DrStatus = 1";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DrID", doctorID);
                cmd.Parameters.AddWithValue("@Password", password); // Direct comparison without hashing
                try
                {
                    conn.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0)
                    {
                        // If login is successful
                        MessageBox.Show("Login Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadPatientData(doctorID); // Load patients after successful login
                    }
                    else
                    {
                        // If login fails (invalid credentials or inactive account)
                        MessageBox.Show("Invalid credentials or inactive account!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    // Log any exception that occurs
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        //// Method to hash the password using SHA256
        //private string HashPassword(string password)
        //{
        //    using (SHA256 sha256 = SHA256.Create())
        //    {
        //        byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        //        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower(); // Convert byte[] to string
        //    }
        //}
        private void btnSearchs_Click(object sender, EventArgs e)
        {
            string doctorID = txtDoctorID.Text.Trim(); // DrID input
            string password = txtDoctorPassword.Text.Trim(); // DrPassword input
            // Print the values for debugging
            Console.WriteLine("Doctor ID: " + doctorID);
            Console.WriteLine("Password: " + password);
            using (SqlConnection conn = new SqlConnection(ValidationHelper.ConnectionString))
            {
                // Validate using DrID and plain text DrPassword from tblDoctorLogin
                string query = "SELECT COUNT(*) FROM tblDoctorLogin WHERE DrID = @DoctorID AND DrPassword = @Password AND DrStatus = 1";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DoctorID", doctorID);
                cmd.Parameters.AddWithValue("@Password", password); // Direct comparison without hashing
                try
                {
                    conn.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0)
                    {
                        // If login is successful
                        MessageBox.Show("Login Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // If login fails (invalid credentials or inactive account)
                        MessageBox.Show("Invalid credentials or inactive account!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    // Log any exception that occurs
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
//---------------------- Without DataBase -------------------------
        private void btnSearch_Click(object sender, EventArgs e)
        {
            (dgvAppointments.DataSource as DataTable).DefaultView.RowFilter =
            string.Format("PatientName LIKE '%{0}%' OR ReasonForVisit LIKE '%{0}%'", txtSearch.Text);
        }
        private void AssignPatient_Shown(object sender, EventArgs e)
        {
            // Define a dictionary of TextBox and their respective placeholder text
            Dictionary<TextBox, string> textBoxPlaceholders = new Dictionary<TextBox, string>
            {
            { txtDoctorID, "Enter Doctor ID" },
            { txtDoctorPassword, "Enter Doctor Password" },
            { txtSearch, "Searc Here" },
            };
            // Apply the placeholder functionality to all TextBoxes in the form
            foreach (var entry in textBoxPlaceholders)
            {
                ValidationHelper.ApplyPlaceholderTextOnClick(entry.Key, entry.Value);
            }
        }
    }
}