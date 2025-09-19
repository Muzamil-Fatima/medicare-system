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
    public partial class Patients : BaseForm
    {
        private string patientID;
        public BaseForm MidParent { get; internal set; }

        private ViewAllPatient viewAllPatientForm;
        public Patients()
        {
            InitializeComponent();
            ValidationHelper.LoadGenderComboBox(cmbGender);
        }
        public Patients(ViewAllPatient form) : this()
        {
            this.viewAllPatientForm = form ?? throw new ArgumentNullException(nameof(form), "ViewAllPatient form cannot be null.");
        }
        public void SetPatientID(string patientID)
        {
            this.patientID = patientID;  // Store the PatientID in the private variable
            LoadPatientData(patientID);  // Call to load data for the specified PatientID
        }
        private void ClearFields()
        {
            txtPatientName.Clear();
            txtPatientGuardian.Clear();
            txtPatientCNIC.Clear();
            txtPatientPhone.Clear();
            txtPatientAllergies.Clear();
            txtPatientAddress.Clear();
            cmbGender.SelectedIndex = -1;
        }
        private void btnDeletes_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPatientCNIC.Text) || !string.IsNullOrEmpty(txtPatientPhone.Text))
            {
                if (viewAllPatientForm != null)
                {
                    // ViewAllPatient form ka method call karna
                    viewAllPatientForm.RemovePatientDetails(txtPatientCNIC.Text, txtPatientPhone.Text);
                }
                else
                {
                    MessageBox.Show("View All Patients form is not open.");
                }
            }
            else
            {
                MessageBox.Show("Please enter CNIC or Phone Number to delete.");
            }
        }
        // Clear Input Fields
        private void btnClears_Click(object sender, EventArgs e)
        {
            ClearFields();
        }
        private void LoadPatientData(string patientID)
        {
            string query = @"
            SELECT p.PatientID, p.PatientName, p.PatientGuardianName, p.PatientGenderID, 
                   c.PatientPhone, c.PatientAddress, 
                   m.PatientCNIC, m.PatientAge, m.PatientAllergies
            FROM tblPatient p
            JOIN tblPatientContact c ON p.PatientID = c.PatientID
            JOIN tblPatientMedical m ON p.PatientID = m.PatientID
            WHERE p.PatientID = @PatientID";

            using (SqlConnection con = new SqlConnection(ValidationHelper.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@PatientID", patientID);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        // Populate the form fields with the fetched patient data
                        txtPatientName.Text = reader["PatientName"].ToString();
                        txtPatientGuardian.Text = reader["PatientGuardianName"].ToString();
                        cmbGender.SelectedValue = reader["PatientGenderID"];
                        txtPatientPhone.Text = reader["PatientPhone"].ToString();
                        txtPatientAddress.Text = reader["PatientAddress"].ToString();
                        txtPatientCNIC.Text = reader["PatientCNIC"].ToString();
                        txtAge.Text = reader["PatientAge"].ToString();
                        txtPatientAllergies.Text = reader["PatientAllergies"].ToString();
                    }
                    con.Close();
                }
            }
        }
        // Open Patient List Form
        private void btnUpdates_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(patientID))  // Check if the PatientID is set
            {
                string patientName = txtPatientName.Text;
                string guardianName = txtPatientGuardian.Text;
                int genderID = Convert.ToInt32(cmbGender.SelectedValue);
                string phone = txtPatientPhone.Text;
                string address = txtPatientAddress.Text;
                string cnic = txtPatientCNIC.Text;
                string allergies = txtPatientAllergies.Text;

                // Validate Age Input
                int age;
                if (string.IsNullOrWhiteSpace(txtAge.Text))
                {
                    MessageBox.Show("Please enter the patient's age.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;  
                }
                else if (!int.TryParse(txtAge.Text, out age))
                {
                    MessageBox.Show("Please enter a valid age in digits only.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;  
                }
                else if (age <= 0)
                {
                    MessageBox.Show("Age cannot be zero or negative. Please enter a valid age.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;  
                }

                using (SqlConnection con = new SqlConnection(ValidationHelper.ConnectionString))
                {
                    con.Open();

                    // Check if CNIC already exists for another patient
                    string checkQuery = "SELECT COUNT(*) FROM tblPatientMedical WHERE PatientCNIC = @CNIC AND PatientID != @PatientID";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, con))
                    {
                        checkCmd.Parameters.AddWithValue("@CNIC", cnic);
                        checkCmd.Parameters.AddWithValue("@PatientID", patientID);

                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (count > 0)
                        {
                            MessageBox.Show("This CNIC is already registered with another patient. Please enter a unique CNIC.",
                                            "Duplicate CNIC", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;  // Stop execution
                        }
                    }

                    //  Proceed with update only if CNIC is unique
                    string updateQuery = @"
                       UPDATE tblPatient 
                       SET PatientName = @PatientName, PatientGuardianName = @GuardianName, PatientGenderID = @GenderID
                       WHERE PatientID = @PatientID;

                       UPDATE tblPatientContact 
                       SET PatientPhone = @Phone, PatientAddress = @Address
                       WHERE PatientID = @PatientID;

                       UPDATE tblPatientMedical 
                       SET PatientCNIC = @CNIC, PatientAge = @Age, PatientAllergies = @Allergies
                       WHERE PatientID = @PatientID;
                       ";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@PatientName", patientName);
                        cmd.Parameters.AddWithValue("@GuardianName", guardianName);
                        cmd.Parameters.AddWithValue("@GenderID", genderID);
                        cmd.Parameters.AddWithValue("@Phone", phone);
                        cmd.Parameters.AddWithValue("@Address", address);
                        cmd.Parameters.AddWithValue("@CNIC", cnic);
                        cmd.Parameters.AddWithValue("@Age", age);
                        cmd.Parameters.AddWithValue("@Allergies", allergies);
                        cmd.Parameters.AddWithValue("@PatientID", patientID);

                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Patient details updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ViewAllPatient allPatientForm = Application.OpenForms["ViewAllPatient"] as ViewAllPatient;
                if (allPatientForm != null)
                {
                    allPatientForm.dgvPatientss.DataSource = null; // Clear first to ensure refresh
                    allPatientForm.dgvPatientss.DataSource = allPatientForm.GetPatients();  // Reload updated data
                }

                this.Close(); 
            }
            else
            {
                MessageBox.Show("Please select a patient to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dtpPatientDOB_ValueChanged(object sender, EventArgs e)
        {
            DateTime dob = dtpPatientDOB.Value;
            int age = DateTime.Now.Year - dob.Year;
            // Adjust age if the birthday hasn't occurred yet this year.
            if (DateTime.Now.DayOfYear < dob.DayOfYear)
            {
                age--;
            }
            txtAge.Text = age.ToString();
        }
        private void btnAdds_Click(object sender, EventArgs e)
        {
            // Ensure required fields are filled
            if (string.IsNullOrWhiteSpace(txtPatientName.Text) || string.IsNullOrWhiteSpace(txtPatientPhone.Text))
            {
                MessageBox.Show("Patient Name and Phone Number are required!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Get genderID safely
            int genderID = (cmbGender.SelectedValue != null && int.TryParse(cmbGender.SelectedValue.ToString(), out int gID)) ? gID : 0;

            string patientID = "";  // Declare patientID

            using (SqlConnection con = new SqlConnection(ValidationHelper.ConnectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                try
                {
                    // Insert Patient and get PatientID
                    string query = "EXEC InsertPatient @Name, @GuardianName, @GenderID";
                    using (SqlCommand cmd = new SqlCommand(query, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@Name", txtPatientName.Text);
                        cmd.Parameters.AddWithValue("@GuardianName", txtPatientGuardian.Text);
                        cmd.Parameters.AddWithValue("@GenderID", genderID);
                        object results = cmd.ExecuteScalar();
                        if (results != null)
                        {
                            patientID = results.ToString();
                        }
                        else
                        {
                            throw new Exception("Failed to retrieve Patient ID.");
                        }
                    }
                    // Insert Contact Info
                    query = "INSERT INTO tblPatientContact (PatientID, PatientPhone, PatientAddress) VALUES (@PatientID, @Phone, @Address)";
                    using (SqlCommand cmd = new SqlCommand(query, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@PatientID", patientID);
                        cmd.Parameters.AddWithValue("@Phone", txtPatientPhone.Text);
                        cmd.Parameters.AddWithValue("@Address", txtPatientAddress.Text);
                        cmd.ExecuteNonQuery();
                    }
                    // Insert Medical Info
                    query = "INSERT INTO tblPatientMedical (PatientID, PatientCNIC, PatientAge, PatientAllergies) VALUES (@PatientID, @CNIC, @Age, @Allergies)";
                    using (SqlCommand cmd = new SqlCommand(query, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@PatientID", patientID);
                        cmd.Parameters.AddWithValue("@CNIC",txtPatientCNIC.Text);
                        cmd.Parameters.AddWithValue("@Age", txtAge.Text);
                        cmd.Parameters.AddWithValue("@Allergies", txtPatientAllergies.Text);
                        cmd.ExecuteNonQuery();
                    }
                    // Commit transaction
                    transaction.Commit();
                    MessageBox.Show("Patient Added Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void txtPhone_Leave_1(object sender, EventArgs e)
        {
            // Validate the phone number using the centralized validation helper.
            if (!ValidationHelper.IsValidPhone(txtPatientPhone.Text))
            {
                MessageBox.Show("Invalid phone format! Please enter a number with 10-15 digits.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txtCNIC_Leave(object sender, EventArgs e)
        {
            if (!ValidationHelper.IsValidCNIC(txtPatientCNIC.Text))
            {
                MessageBox.Show("Invalid CNIC format. Please enter in 12345-1234567-1 format.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void txtCNIC_TextChanged(object sender, EventArgs e)
        {
            txtPatientCNIC.Text = ValidationHelper.FormatCNIC(txtPatientCNIC.Text);
            txtPatientCNIC.SelectionStart = txtPatientCNIC.Text.Length; // Maintain cursor position
        }
        private void Patients_Shown(object sender, EventArgs e)
        {
            // Define a dictionary of TextBox and their respective placeholder text
            Dictionary<TextBox, string> textBoxPlaceholders = new Dictionary<TextBox, string>
            {
            { txtPatientName, "Enter Patient Name" },
            { txtPatientCNIC, "Enter Patient CNIC" },
            { txtPatientPhone, "Enter Patient Phone" },
            { txtPatientGuardian, "Enter Guardian" },
            { txtPatientAddress, "Enter Patient Address" },
            {txtPatientAllergies,"Enter Patient Allergies" },
            };
            // Apply the placeholder functionality to all TextBoxes in the form
            foreach (var entry in textBoxPlaceholders)
            {
                ValidationHelper.ApplyPlaceholderTextOnClick(entry.Key, entry.Value);
            }
        }
        private void Patients_Click(object sender, EventArgs e)
        {
        }
    }
}