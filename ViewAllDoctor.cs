using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace ClinicMangSystem
{
    public partial class ViewAllDoctor: BaseForm
    {
        public BaseForm MidParent { get; internal set; }
        public ViewAllDoctor()
        {
            InitializeComponent();
        } 
        private void ViewAllDoctor_Load(object sender, EventArgs e)
        {
            LoadAllDoctors();
        }
        public void LoadAllDoctors(string filter = "", string column = "")
        {
            using (SqlConnection conn = new SqlConnection(ValidationHelper.ConnectionString))
            {
                conn.Open();

                string query = @"SELECT  d.DrID, d.DrName, g.Gender, d.DrDOB, d.DrCNIC, d.DrEmail,
                               c.DrPhone, c.DrAddress, dp.DrQualification, s.SpecializationName AS Specialization, 
                               dp.DrLicenseNo, dp.DrExperience, da.DrAvailableDays, da.DrConsultationStart, 
                               da.DrConsultationEnd, da.DrRoomNo, da.DrAppointmentDuration, da.DrMaxPatientsDay, 
                               dl.DrUsername, dl.DrStatus
                              FROM tblDoctor d
                              JOIN tblGender g ON d.GenderID = g.GenderID
                              LEFT JOIN tblDoctorContact c ON d.DrID = c.DrID
                              LEFT JOIN tblDoctorProfessional dp ON d.DrID = dp.DrID
                              LEFT JOIN tblDoctorSpecialization s ON dp.SpecializationID = s.SpecializationID
                              LEFT JOIN tblDoctorAvailability da ON d.DrID = da.DrID
                              LEFT JOIN tblDoctorLogin dl ON d.DrID = dl.DrID";
                if (!string.IsNullOrWhiteSpace(filter) && !string.IsNullOrWhiteSpace(column))
                {
                    query += $" WHERE {column} LIKE @Filter";
                }

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrWhiteSpace(filter) && !string.IsNullOrWhiteSpace(column))
                    {
                        cmd.Parameters.AddWithValue("@Filter", "%" + filter + "%");
                    }
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvDoctors.DataSource = dt;
                }
            }
        }
        private void LoadData()
        {
            LoadAllDoctors(); // Simply call LoadAllDoctors with no filter.
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadData();
        }
        public void RemoveDoctorFromGrid(string doctorID)
        {
            foreach (DataGridViewRow row in dgvDoctors.Rows)
            {
                if (row.Cells["DrID"].Value != null && row.Cells["DrID"].Value.ToString() == doctorID)
                {
                    dgvDoctors.Rows.Remove(row); // GridView se row remove karo
                    MessageBox.Show("Doctor removed from the grid!", "Success");
                    return;
                }
            }
        }
        private void dgvDoctors_CellDoubleClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure a valid row is selected
            {
                int drID = Convert.ToInt32(dgvDoctors.Rows[e.RowIndex].Cells["DrID"].Value);
                // Open the Doctor's details form
                Doctors detailsForm = new Doctors();
                // Pass doctor data to the Doctors form
                detailsForm.txtDoctorID.Text = drID.ToString();
                detailsForm.txtDrName.Text = dgvDoctors.Rows[e.RowIndex].Cells["DrName"].Value.ToString();
                detailsForm.dtpDrDOB.Text = dgvDoctors.Rows[e.RowIndex].Cells["DrDOB"].Value.ToString();
                detailsForm.txtDrCNIC.Text = dgvDoctors.Rows[e.RowIndex].Cells["DrCNIC"].Value.ToString();
                detailsForm.txtDrPhone.Text = dgvDoctors.Rows[e.RowIndex].Cells["DrPhone"].Value.ToString();
                detailsForm.txtDrEmail.Text = dgvDoctors.Rows[e.RowIndex].Cells["DrEmail"].Value.ToString();
                detailsForm.txtDrAddress.Text = dgvDoctors.Rows[e.RowIndex].Cells["DrAddress"].Value.ToString();
                detailsForm.cmbDrSpecialization.Text = dgvDoctors.Rows[e.RowIndex].Cells["Specialization"].Value.ToString();
                detailsForm.txtDrQualification.Text = dgvDoctors.Rows[e.RowIndex].Cells["DrQualification"].Value.ToString();
                detailsForm.txtDrLicenseNo.Text = dgvDoctors.Rows[e.RowIndex].Cells["DrLicenseNo"].Value.ToString();
                detailsForm.txtDrExperience.Text = dgvDoctors.Rows[e.RowIndex].Cells["DrExperience"].Value.ToString();
                detailsForm.txtDrUserName.Text = dgvDoctors.Rows[e.RowIndex].Cells["DrUsername"].Value.ToString();
                detailsForm.txtDrConsultationStart.Text = dgvDoctors.Rows[e.RowIndex].Cells["DrConsultationStart"].Value.ToString();
                detailsForm.txtDrConsultationEnd.Text = dgvDoctors.Rows[e.RowIndex].Cells["DrConsultationEnd"].Value.ToString();
                detailsForm.txtDrRoomNo.Text = dgvDoctors.Rows[e.RowIndex].Cells["DrRoomNo"].Value.ToString();
                detailsForm.cmbDrAppointmentDuration.Text = dgvDoctors.Rows[e.RowIndex].Cells["DrAppointmentDuration"].Value.ToString();
                detailsForm.txtDrMaxPatientsDay.Text = dgvDoctors.Rows[e.RowIndex].Cells["DrMaxPatientsDay"].Value.ToString();
                detailsForm.rdbActive.Checked = Convert.ToInt32(dgvDoctors.Rows[e.RowIndex].Cells["DrStatus"].Value) == 1;
                detailsForm.rdbInactive.Checked = !detailsForm.rdbActive.Checked;
                // Handle Available Days Selection
                string availableDays = dgvDoctors.Rows[e.RowIndex].Cells["DrAvailableDays"].Value?.ToString();
                if (!string.IsNullOrEmpty(availableDays))
                {
                    string[] selectedDays = availableDays.Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < detailsForm.checkedListBoxDays.Items.Count; i++)
                    {
                        string item = detailsForm.checkedListBoxDays.Items[i].ToString().Trim();
                        if (selectedDays.Contains(item, StringComparer.OrdinalIgnoreCase))
                        {
                            detailsForm.checkedListBoxDays.SetItemChecked(i, true);
                        }
                    }
                }
                detailsForm.ShowDialog(); // Show the Doctors form

                // After closing the Doctors form, refresh the DataGridView
                RefreshDoctorsGrid(); // Refreshing the grid after the form is closed
            }
        }
        private void RefreshDoctorsGrid()
        {
            // Fetch the latest data from the database and bind it to the DataGridView
            dgvDoctors.DataSource = GetDoctorsData(); // Get latest doctor data
            dgvDoctors.Refresh(); // Refresh the DataGridView
        }
        private DataTable GetDoctorsData()
        {
            DataTable dt = new DataTable();
            string query = @"
        SELECT 
            d.DrID, 
            d.DrName, 
            g.Gender, 
            d.DrDOB,
            d.DrCNIC, 
            d.DrEmail,
            c.DrPhone, 
            c.DrAddress, 
            dp.DrQualification, 
            s.SpecializationName AS Specialization, 
            dp.DrLicenseNo, 
            dp.DrExperience, 
            da.DrAvailableDays, 
            da.DrConsultationStart, 
            da.DrConsultationEnd, 
            da.DrRoomNo, 
            da.DrAppointmentDuration, 
            da.DrMaxPatientsDay, 
            dl.DrUsername, 
            dl.DrStatus
        FROM tblDoctor d
        JOIN tblGender g ON d.GenderID = g.GenderID
        LEFT JOIN tblDoctorContact c ON d.DrID = c.DrID
        LEFT JOIN tblDoctorProfessional dp ON d.DrID = dp.DrID
        LEFT JOIN tblDoctorSpecialization s ON dp.SpecializationID = s.SpecializationID
        LEFT JOIN tblDoctorAvailability da ON d.DrID = da.DrID
        LEFT JOIN tblDoctorLogin dl ON d.DrID = dl.DrID";

            using (SqlConnection conn = new SqlConnection(ValidationHelper.ConnectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.Fill(dt);
            }
            return dt;
        }
        public void LoadDoctors()
        {
            DataTable dt = GetDoctorsData();
            dgvDoctors.DataSource = dt;
        }
        private void btnDrSearch_Click(object sender, EventArgs e)
        {
            // Highlight matching cells and move matching rows to top
            ValidationHelper.HighlightMatchingCells(dgvDoctors, txtSearch);
            //ValidationHelper.MoveMatchingRowsToTop(dgvDoctors, txtSearch);
        }
        private void ViewAllDoctor_Shown(object sender, EventArgs e)
        {
            // Define a dictionary of TextBox and their respective placeholder text
            Dictionary<TextBox, string> textBoxPlaceholders = new Dictionary<TextBox, string>
            {
            { textBox1, "Search Here" },
            };

            // Apply the placeholder functionality to all TextBoxes in the form
            foreach (var entry in textBoxPlaceholders)
            {
                ValidationHelper.ApplyPlaceholderTextOnClick(entry.Key, entry.Value);
            }
        }

        private void btnRefreshs_Click(object sender, EventArgs e)
        {
            ValidationHelper.ResetGrid(dgvDoctors);
        }
    }
}
