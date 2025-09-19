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
    public partial class Prescriptions : BaseForm
    {
        public Prescriptions(string pID, string pName, string dID, string dName)
        {
            InitializeComponent();
            txtDoctorID.Text = dID;
            txtDoctorName.Text = dName;
            txtPatientID.Text = pID;
            txtPatientName.Text = pName;
        }

        private void Prescriptions_Load(object sender, EventArgs e)
        {
           
        }
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Save Successfully");
            AssignPatient n = new AssignPatient();
            this.Hide();
            n.Show();
            //using (SqlConnection conn = new SqlConnection(connectionString))
            //{
            //    string query = "INSERT INTO tblPrescriptions (DoctorID, PatientID, Medicines, Notes, DatePrescribed) " +
            //                   "VALUES (@DoctorID, @PatientID, @Medicines, @Notes, @DatePrescribed); " +
            //                   "SELECT SCOPE_IDENTITY();";  // Last inserted PrescriptionID return karega

            //    SqlCommand cmd = new SqlCommand(query, conn);
            //    cmd.Parameters.AddWithValue("@DoctorID", txtDoctorID.Text);
            //    cmd.Parameters.AddWithValue("@PatientID", txtPatientID.Text);
            //    cmd.Parameters.AddWithValue("@Medicines", txtMedicines.Text);
            //    cmd.Parameters.AddWithValue("@Notes", txtPrescription.Text);
            //    cmd.Parameters.AddWithValue("@DatePrescribed", DateTime.Now);

            //    conn.Open();
            //    int prescriptionID = Convert.ToInt32(cmd.ExecuteScalar());  // Retrieve last inserted ID

            //    MessageBox.Show($"Prescription Saved Successfully! ID: {prescriptionID}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }
        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Delete Successfully");
            //using (SqlConnection conn = new SqlConnection(connectionString))
            //{
            //    string query = "DELETE FROM tblPrescriptions WHERE DoctorID = @DoctorID AND PatientID = @PatientID AND DatePrescribed = @DatePrescribed";

            //    SqlCommand cmd = new SqlCommand(query, conn);
            //    cmd.Parameters.AddWithValue("@DoctorID", txtDoctorID.Text);
            //    cmd.Parameters.AddWithValue("@PatientID", txtPatientID.Text);
            //    cmd.Parameters.AddWithValue("@DatePrescribed", DateTime.Today); // Aaj ki date ke hisaab se delete karega

            //    conn.Open();
            //    int rowsAffected = cmd.ExecuteNonQuery();

            //    if (rowsAffected > 0)
            //    {
            //        MessageBox.Show("Prescription Deleted Successfully!", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    }
            //    else
            //    {
            //        MessageBox.Show("No matching prescription found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //}
        }

        private void Prescriptions_Shown(object sender, EventArgs e)
        {
            // Define a dictionary of TextBox and their respective placeholder text
            Dictionary<TextBox, string> textBoxPlaceholders = new Dictionary<TextBox, string>
            {
            { txtPatientName, "Enter Patient Name" },
            { txtPatientID, "Enter Patient CNIC" },
            { txtDoctorName, "Enter Patient Phone" },
            { txtDoctorID, "Enter Guardian" },
            { txtMedicines, "Enter Medicine" },
            };

            // Apply the placeholder functionality to all TextBoxes in the form
            foreach (var entry in textBoxPlaceholders)
            {
                ValidationHelper.ApplyPlaceholderTextOnClick(entry.Key, entry.Value);
            }
        }
    }
}
