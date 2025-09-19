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
    public partial class ViewAllPatient : BaseForm
    {
        private Patients registrationForm;
        //private string selectedPatientID;
        public BaseForm MidParent { get; internal set; }
        public ViewAllPatient(Patients form1)
        {
            InitializeComponent();
            dgvPatientss.DataSource = GetPatients();
            this.registrationForm = form1;
            LoadPatients();
        }
        public ViewAllPatient() : this(null)
        {
        }
        private void ViewAllPatient_Load(object sender, EventArgs e)
        {
            try
            {
                LoadPatients();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during form load: " + ex.Message);
            }
        }
        // Load Data into DataGridView
        public void LoadPatients(string searchQuery = "")
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ValidationHelper.ConnectionString))
                {
                    con.Open();
                    string query = @"SELECT p.PatientID, p.PatientName, p.PatientGuardianName, 
                                    g.Gender, c.PatientPhone, c.PatientAddress, 
                                    m.PatientCNIC, m.PatientAge, m.PatientAllergies,
                                    p.Date_Time
                             FROM tblPatient p
                             JOIN tblGender g ON p.PatientGenderID = g.GenderID
                             LEFT JOIN tblPatientContact c ON p.PatientID = c.PatientID
                             LEFT JOIN tblPatientMedical m ON p.PatientID = m.PatientID
                             WHERE p.PatientName LIKE @Search 
                             OR m.PatientCNIC LIKE @Search 
                             OR c.PatientPhone LIKE @Search";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Search", "%" + searchQuery + "%");
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        // Format Date Column for Readability
                        if (dt.Columns.Contains("Date_Time"))
                        {
                            dt.Columns["Date_Time"].ColumnName = "Registration Date";
                        }
                        dgvPatientss.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading patients: " + ex.Message);
            }
        }
        public DataTable GetPatients()
        {
            string query = @"
            SELECT p.PatientID, p.PatientName, p.PatientGuardianName, g.Gender, 
                   c.PatientPhone, c.PatientAddress, 
                   m.PatientCNIC, m.PatientAge, m.PatientAllergies
            FROM tblPatient p
            JOIN tblGender g ON p.PatientGenderID = g.GenderID
            LEFT JOIN tblPatientContact c ON p.PatientID = c.PatientID
            LEFT JOIN tblPatientMedical m ON p.PatientID = m.PatientID";
            using (SqlConnection con = new SqlConnection(ValidationHelper.ConnectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        private void dgvPatientss_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int selectedRowIndex = e.RowIndex;

            if (selectedRowIndex >= 0)
            {
                DataGridViewRow selectedRow = dgvPatientss.Rows[selectedRowIndex];
                string patientID = selectedRow.Cells["PatientID"].Value.ToString(); 
                Patients patientForm = new Patients(this);
                patientForm.SetPatientID(patientID);
                patientForm.Show();

                LoadPatients();
            }
        }
        public void RemovePatientDetails(string cnic, string phone)
        {
            bool recordFound = false;

            foreach (DataGridViewRow row in dgvPatientss.Rows)
            {
                string rowCNIC = row.Cells["PatientCNIC"].Value?.ToString();
                string rowPhone = row.Cells["PatientPhone"].Value?.ToString();

                if ((rowCNIC == cnic && !string.IsNullOrEmpty(rowCNIC)) ||
                    (rowPhone == phone && !string.IsNullOrEmpty(rowPhone)))
                {
                    //  Sirf Patient ID, Name, aur Date ko safe rakhna hai, baaki sab remove karna hai
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        string columnName = dgvPatientss.Columns[cell.ColumnIndex].Name;

                        if (columnName != "PatientID" && columnName != "PatientName" && columnName != "Date_Time")
                        {
                            cell.Value = DBNull.Value;  // Data remove karna
                        }
                    }
                    row.DefaultCellStyle.BackColor = Color.LightSkyBlue;

                    recordFound = true;
                    break;
                }
            }
            if (recordFound)
            {
                MessageBox.Show("Deleted Successfully from DataGridView.");
            }
            else
            {
                MessageBox.Show("No matching patient found in the list.");
            }
        }
        private void btnSearchs_Click(object sender, EventArgs e)
        {
            // Highlight matching cells and move matching rows to top
            ValidationHelper.HighlightMatchingCells(dgvPatientss, txtSearch);
            ValidationHelper.MoveMatchingRowsToTop(dgvPatientss, txtSearch);
            LoadPatients(txtSearch.Text);
        }
        private void btnRefreshs_Click(object sender, EventArgs e)
        {
            try
            {
                LoadPatients();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during refresh: " + ex.Message);
            }
        }
        private void ViewAllPatient_Shown(object sender, EventArgs e)
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
        private void dgvPatientss_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dgvPatientss.DataSource is DataTable dt)
            {
                // "SortID" column check karo, agar already exist nahi karta to add karo
                if (!dt.Columns.Contains("SortID"))
                {
                    dt.Columns.Add("SortID", typeof(int));

                    // Har row ke "PatientID" se sorting number extract karo
                    foreach (DataRow row in dt.Rows)
                    {
                        string patientID = row["PatientID"].ToString();
                        if (!string.IsNullOrEmpty(patientID) && patientID.Contains('-'))
                        {
                            if (int.TryParse(patientID.Split('-')[0], out int sortID))
                            {
                                row["SortID"] = sortID;
                            }
                        }
                    }
                    // Sorting apply karo
                    dt.DefaultView.Sort = "SortID DESC";
                }
            }
        }
    }
}