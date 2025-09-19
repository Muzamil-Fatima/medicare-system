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
using System.Xml.Linq;

namespace ClinicMangSystem
{
    public partial class ViewAllAppointment: BaseForm
    {
        public ViewAllAppointment()
        {
            InitializeComponent();
            LoadAppointments();
        }
        public string CustomAppointmentID { get; set; }

        public void LoadAppointments()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ValidationHelper.ConnectionString))
                {
                    con.Open();

                    string query = @"SELECT 
                           a.CustomAppointmentID,
                           p.PatientID,
                           p.PatientName,
                           pc.PatientPhone,
                           d.DrID,
                           d.DrName,
                           ds.SpecializationName AS DrSpecialization,
                           a.AppointmentDate,
                           a.AppointmentTime,
                           a.AppointmentStatus,
                           a.AppointmentReason  -- Directly retrieving the reason from tblAppointments
                           FROM tblAppointments a
                           JOIN tblPatient p ON a.PatientID = p.PatientID
                           LEFT JOIN tblPatientContact pc ON p.PatientID = pc.PatientID
                           JOIN tblDoctor d ON a.DrID = d.DrID
                           LEFT JOIN tblDoctorProfessional dp ON d.DrID = dp.DrID
                           LEFT JOIN tblDoctorSpecialization ds ON dp.SpecializationID = ds.SpecializationID";

                           SqlDataAdapter da = new SqlDataAdapter(query, con);
                           DataTable dt = new DataTable();
                           da.Fill(dt);

                              // Format the AppointmentDate to 'yyyy-MM-dd' before binding
                           foreach (DataRow row in dt.Rows)
                           {
                               DateTime appointmentDate = Convert.ToDateTime(row["AppointmentDate"]);
                               row["AppointmentDate"] = appointmentDate.ToString("yyyy-MM-dd"); // Format the date
                           }

                           dgvAppointments.DataSource = dt;
                      }
                    }
                   catch (Exception ex)
                  {
                    MessageBox.Show("Error loading appointments: " + ex.Message);
                  }
        }

        private void ViewAllAppointment_Load(object sender, EventArgs e)
        {
        }
        private void dgvAppointments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    // Ensure the appointment form is initialized (Form 1)
                    Appointment appointmentForm = new Appointment();

                    DataGridViewRow row = dgvAppointments.Rows[e.RowIndex];

                    // Pass the CustomAppointmentID from Form 2 to Form 1
                    appointmentForm.CustomAppointmentID = row.Cells["CustomAppointmentID"].Value.ToString();

                    // Set the other form fields as necessary (e.g., PatientID, DoctorID, etc.)
                    appointmentForm.txtPatientName.Text = row.Cells["PatientName"].Value.ToString();
                    appointmentForm.txtDrName.Text = row.Cells["DrName"].Value.ToString();
                    appointmentForm.txtPatientID.Text = row.Cells["PatientID"].Value.ToString();
                    appointmentForm.dtpAppointmentDate.Text = row.Cells["AppointmentDate"].Value.ToString();
                    appointmentForm.dtpTime.Text = row.Cells["AppointmentTime"].Value.ToString();
                    appointmentForm.cmbStatus.Text = row.Cells["AppointmentStatus"].Value.ToString();
                    appointmentForm.txtPatientPhone.Text = row.Cells["PatientPhone"].Value.ToString();
                    appointmentForm.txtReasonForVisit.Text = row.Cells["AppointmentReason"].Value.ToString();
                    appointmentForm.txtDoctorID.Text = row.Cells["DrID"].Value.ToString();
                    appointmentForm.txtDrSpecialization.Text = row.Cells["DrSpecialization"]?.Value?.ToString() ?? "";
                    // Show the form
                    appointmentForm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening appointment form: " + ex.Message);
            }
        }


        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSearchs_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                ValidationHelper.HighlightMatchingCells(dgvAppointments, txtSearch);
                ValidationHelper.ScrollToFirstMatch(dgvAppointments, txtSearch);
            }
        }

        private void ViewAllAppointment_Shown(object sender, EventArgs e)
        {
            // Define a dictionary of TextBox and their respective placeholder text
            Dictionary<TextBox, string> textBoxPlaceholders = new Dictionary<TextBox, string>
            {
            { txtSearchs, "Search Here" },
            };

            // Apply the placeholder functionality to all TextBoxes in the form
            foreach (var entry in textBoxPlaceholders)
            {
                ValidationHelper.ApplyPlaceholderTextOnClick(entry.Key, entry.Value);
            }
        }

        private void btnRefreshs_Click(object sender, EventArgs e)
        {
           
        }

        private void btnSearchs_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
                ValidationHelper.ResetGrid(dgvAppointments);
            else
                ValidationHelper.HighlightMatchingCells(dgvAppointments, txtSearch);
        }
    }
}