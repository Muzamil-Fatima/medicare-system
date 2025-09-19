using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ClinicMangSystem
{
    public partial class Doctors : BaseForm
    {
        //private int doctorID; // Store the doctor ID to update
        // Keep track if we’re editing an existing record
        private bool isEditMode = false;
        //private string availableDays;
        public BaseForm MidParent { get; internal set; }
        public Doctors()
        {
            InitializeComponent();
            pictureBoxDoctor.SizeMode = PictureBoxSizeMode.Zoom; // Set display mode for pictureBox
            LoadSpecializations();
            ValidationHelper.LoadGenderComboBox(cmbDrGender);
            cmbDrAppointmentDuration.Items.Add("30 minutes");
            cmbDrAppointmentDuration.Items.Add("45 minutes");
            cmbDrAppointmentDuration.Items.Add("60 minutes");
            cmbDrAppointmentDuration.Items.Add("90 minutes");
        }
        private void Doctors_Load(object sender, EventArgs e)
        {
            rdbActive.Checked = true;
            isEditMode = false;

            // By default, DoctorID is blank or read-only
            txtDoctorID.ReadOnly = true;
        }
        //-------- btn Update------
        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            var requiredFields = new List<Control> {
                   txtDrName, txtDrCNIC, txtDrEmail, txtDrPhone, txtDrAddress,
                  txtDrQualification, txtDrLicenseNo, txtDrExperience, txtDrRoomNo, cmbDrAppointmentDuration,
                  txtDrUserName, txtDrPassword };
            if (requiredFields.Any(tb => string.IsNullOrEmpty(tb.Text)))
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Ensure doctorID is valid
            if (!int.TryParse(txtDoctorID.Text, out int doctorID))
            {
                MessageBox.Show("Invalid Doctor ID!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                using (SqlConnection con = new SqlConnection(ValidationHelper.ConnectionString))
                {
                    con.Open();
                    using (SqlTransaction transaction = con.BeginTransaction()) // Use Transaction
                    {
                        try
                        {
                            // Update tblDoctor
                            string updateDoctorQuery = @"UPDATE tblDoctor  SET DrName = @DrName, DrDOB = @DrDOB, GenderID = @GenderID, DrCNIC = @DrCNIC, DrEmail = @DrEmail WHERE DrID = @DrID";
                            using (SqlCommand cmd = new SqlCommand(updateDoctorQuery, con, transaction))
                            {
                                cmd.Parameters.AddWithValue("@DrName", txtDrName.Text);
                                cmd.Parameters.AddWithValue("@DrDOB", dtpDrDOB.Value);
                                cmd.Parameters.AddWithValue("@GenderID", cmbDrGender.SelectedValue ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@DrCNIC", txtDrCNIC.Text);
                                cmd.Parameters.AddWithValue("@DrEmail", txtDrEmail.Text);
                                cmd.Parameters.AddWithValue("@DrID", doctorID);
                                cmd.ExecuteNonQuery();
                            }
                            // Update tblDoctorContact
                            string updateDoctorContactQuery = @" UPDATE tblDoctorContact  SET DrPhone = @DrPhone, DrAddress = @DrAddress  WHERE DrID = @DrID";
                            using (SqlCommand cmd = new SqlCommand(updateDoctorContactQuery, con, transaction))
                            {
                                cmd.Parameters.AddWithValue("@DrPhone", txtDrPhone.Text);
                                cmd.Parameters.AddWithValue("@DrAddress", txtDrAddress.Text);
                                cmd.Parameters.AddWithValue("@DrID", doctorID);
                                cmd.ExecuteNonQuery();
                            }
                            //  Update tblDoctorProfessional
                            string updateDoctorProfessionalQuery = @" UPDATE tblDoctorProfessional  SET SpecializationID = @SpecializationID, DrQualification = @DrQualification, DrLicenseNo = @DrLicenseNo, DrExperience = @DrExperience  WHERE DrID = @DrID";
                            using (SqlCommand cmd = new SqlCommand(updateDoctorProfessionalQuery, con, transaction))
                            {
                                cmd.Parameters.AddWithValue("@SpecializationID", cmbDrSpecialization.SelectedValue ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@DrQualification", txtDrQualification.Text);
                                cmd.Parameters.AddWithValue("@DrLicenseNo", txtDrLicenseNo.Text);
                                cmd.Parameters.AddWithValue("@DrExperience", txtDrExperience.Text);
                                cmd.Parameters.AddWithValue("@DrID", doctorID);
                                cmd.ExecuteNonQuery();
                            }
                            //  Update tblDoctorAvailability
                            string updateDoctorAvailabilityQuery = @" UPDATE tblDoctorAvailability  SET DrAvailableDays = @DrAvailableDays, DrConsultationStart = @DrConsultationStart,  DrConsultationEnd = @DrConsultationEnd, DrRoomNo = @DrRoomNo,    DrAppointmentDuration = @DrAppointmentDuration, DrMaxPatientsDay = @DrMaxPatientsDay  WHERE DrID = @DrID";
                            using (SqlCommand cmd = new SqlCommand(updateDoctorAvailabilityQuery, con, transaction))
                            {
                                string selectedDays = string.Join(", ", checkedListBoxDays.CheckedItems.Cast<string>());
                                cmd.Parameters.AddWithValue("@DrAvailableDays", selectedDays);
                                cmd.Parameters.AddWithValue("@DrConsultationStart", txtDrConsultationStart.Text);
                                cmd.Parameters.AddWithValue("@DrConsultationEnd", txtDrConsultationEnd.Text);
                                cmd.Parameters.AddWithValue("@DrRoomNo", txtDrRoomNo.Text);
                                cmd.Parameters.AddWithValue("@DrAppointmentDuration", cmbDrAppointmentDuration.Text);
                                cmd.Parameters.AddWithValue("@DrMaxPatientsDay", txtDrMaxPatientsDay.Text);
                                cmd.Parameters.AddWithValue("@DrID", doctorID);
                                cmd.ExecuteNonQuery();
                            }
                            // Update tblDoctorLogin (Hashing Password)
                            string updateDoctorLoginQuery = @" UPDATE tblDoctorLogin  SET DrUsername = @DrUsername, DrPassword = @DrPassword, DrStatus = @DrStatus  WHERE DrID = @DrID";
                            using (SqlCommand cmd = new SqlCommand(updateDoctorLoginQuery, con, transaction))
                            {
                                cmd.Parameters.AddWithValue("@DrUsername", txtDrUserName.Text);
                                cmd.Parameters.AddWithValue("@DrPassword", txtDrPassword.Text); // Hash Password
                                cmd.Parameters.AddWithValue("@DrStatus", rdbActive.Checked ? 1 : 0);
                                cmd.Parameters.AddWithValue("@DrID", doctorID);
                                cmd.ExecuteNonQuery();
                            }
                            // Commit Transaction
                            transaction.Commit();
                            MessageBox.Show("Doctor details updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            // Reload DataGridView on Parent Form
                            if (this.Owner is ViewAllDoctor ownerForm)
                            {
                                ownerForm.LoadAllDoctors();
                            }
                            this.Close(); // Close Form After Update
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback(); // ❌ Rollback if any error occurs
                            MessageBox.Show("Error updating doctor details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database connection error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Hash Password Function (Recommended for Security)
        //private string HashPassword(string password)
        //{
        //    using (SHA256 sha256 = SHA256.Create())
        //    {
        //        byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        //        StringBuilder builder = new StringBuilder();
        //        foreach (byte b in bytes)
        //        {
        //            builder.Append(b.ToString("x2"));
        //        }
        //        return builder.ToString();
        //    }
        //}
        //------------------ btn Add------
        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Use the ConnectionString directly from ValidationHelper
                using (SqlConnection con = new SqlConnection(ValidationHelper.ConnectionString))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();
                    int doctorId;

                    // Insert into tblDoctor and Get DrID
                    string insertDoctorQuery = @"INSERT INTO tblDoctor (DrName, GenderID, DrDOB, DrCNIC, DrEmail) 
                                         VALUES (@DrName, @GenderID, @DrDOB, @DrCNIC, @DrEmail);
                                         SELECT CAST(SCOPE_IDENTITY() AS INT);"; // Ensure the ID is returned as an integer

                    using (SqlCommand cmd = new SqlCommand(insertDoctorQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@DrName", txtDrName.Text);
                        cmd.Parameters.AddWithValue("@GenderID", cmbDrGender.SelectedIndex + 1);
                        cmd.Parameters.AddWithValue("@DrDOB", dtpDrDOB.Value);
                        cmd.Parameters.AddWithValue("@DrCNIC", txtDrCNIC.Text);
                        cmd.Parameters.AddWithValue("@DrEmail", txtDrEmail.Text);

                        // Ensure the returned ID is cast as an integer
                        doctorId = Convert.ToInt32(cmd.ExecuteScalar());

                        // Check if doctorId is valid (greater than 0)
                        if (doctorId <= 0)
                        {
                            throw new Exception("Failed to retrieve DrID.");
                        }
                    }

                    // Insert into tblDoctorContact
                    string insertContactQuery = "INSERT INTO tblDoctorContact (DrID, DrPhone, DrAddress) VALUES (@DrID, @DrPhone, @DrAddress);";
                    using (SqlCommand cmd = new SqlCommand(insertContactQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@DrID", doctorId);  // Use the fetched DrID
                        cmd.Parameters.AddWithValue("@DrPhone", txtDrPhone.Text);
                        cmd.Parameters.AddWithValue("@DrAddress", txtDrAddress.Text);
                        cmd.ExecuteNonQuery();
                    }

                    // Insert into tblDoctorProfessional
                    string insertProfessionalQuery = "INSERT INTO tblDoctorProfessional (DrID, SpecializationID, DrQualification, DrLicenseNo, DrExperience) " +
                                                     "VALUES (@DrID, @SpecializationID, @DrQualification, @DrLicenseNo, @DrExperience);";
                    using (SqlCommand cmd = new SqlCommand(insertProfessionalQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@DrID", doctorId);  // Use the fetched DrID
                        cmd.Parameters.AddWithValue("@SpecializationID", cmbDrSpecialization.SelectedIndex + 1);
                        cmd.Parameters.AddWithValue("@DrQualification", txtDrQualification.Text);
                        cmd.Parameters.AddWithValue("@DrLicenseNo", txtDrLicenseNo.Text);
                        cmd.Parameters.AddWithValue("@DrExperience", txtDrExperience.Text);
                        cmd.ExecuteNonQuery();
                    }

                    // Insert into tblDoctorAvailability
                    string availableDays = string.Join(",", checkedListBoxDays.CheckedItems.Cast<string>());
                    string insertAvailabilityQuery = "INSERT INTO tblDoctorAvailability (DrID, DrAvailableDays, DrConsultationStart, DrConsultationEnd, DrRoomNo, DrAppointmentDuration, DrMaxPatientsDay) " +
                                                    "VALUES (@DrID, @DrAvailableDays, @DrConsultationStart, @DrConsultationEnd, @DrRoomNo, @DrAppointmentDuration, @DrMaxPatientsDay);";
                    using (SqlCommand cmd = new SqlCommand(insertAvailabilityQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@DrID", doctorId);  // Use the fetched DrID
                        cmd.Parameters.AddWithValue("@DrAvailableDays", availableDays);
                        cmd.Parameters.AddWithValue("@DrConsultationStart", txtDrConsultationStart.Value.TimeOfDay);
                        cmd.Parameters.AddWithValue("@DrConsultationEnd", txtDrConsultationEnd.Value.TimeOfDay);
                        cmd.Parameters.AddWithValue("@DrRoomNo", txtDrRoomNo.Text);
                        cmd.Parameters.AddWithValue("@DrAppointmentDuration", cmbDrAppointmentDuration.Text);
                        cmd.Parameters.AddWithValue("@DrMaxPatientsDay", txtDrMaxPatientsDay.Value);
                        cmd.ExecuteNonQuery();
                    }

                    // Insert into tblDoctorLogin
                    string insertLoginQuery = "INSERT INTO tblDoctorLogin (DrID, DrUsername, DrPassword, DrStatus) VALUES (@DrID, @DrUsername, @DrPassword, @DrStatus);";
                    using (SqlCommand cmd = new SqlCommand(insertLoginQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@DrID", doctorId);  // Use the fetched DrID
                        cmd.Parameters.AddWithValue("@DrUsername", txtDrUserName.Text);
                        cmd.Parameters.AddWithValue("@DrPassword", txtDrPassword.Text);
                        cmd.Parameters.AddWithValue("@DrStatus", rdbActive.Checked ? 1 : 0);
                        cmd.ExecuteNonQuery();
                    }

                    // Commit the transaction
                    transaction.Commit();

                    // Show success message
                    MessageBox.Show("Doctor added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //------------btn Delete  
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDoctorID.Text))
            {
                MessageBox.Show("Please enter a valid Doctor ID.");
                return;
            }
            string doctorID = txtDoctorID.Text.Trim();
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to remove this doctor from the grid?", "Confirm Deletion", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                //  `FormDoctors` ka instance get karo
                ViewAllDoctor parentForm = Application.OpenForms["ViewAllDoctor"] as ViewAllDoctor;
                if (parentForm != null)
                {
                    parentForm.RemoveDoctorFromGrid(doctorID); // Function call karo
                }
                else
                {
                    MessageBox.Show("Error: Parent form not found!");
                }
            }
        }
        private void ClearFields()
        {
                txtDoctorID.Text = "";
            txtDrName.Text = "";
            cmbDrGender.SelectedIndex = -1;
            txtDrCNIC.Text = "";
            txtDrPhone.Text = "";
            txtDrEmail.Text = "";
            txtDrAddress.Text = "";
            cmbDrSpecialization.SelectedIndex = -1;
            txtDrQualification.Text = "";
            txtDrLicenseNo.Text = "";
            txtDrExperience.Text = "";
            txtDrUserName.Text = "";
            txtDrPassword.Text = "";
            txtDrConsultationStart.Value = DateTime.Now;
            txtDrConsultationEnd.Value = DateTime.Now;
            txtDrRoomNo.Text = "";
            cmbDrAppointmentDuration.SelectedIndex = -1;
            txtDrMaxPatientsDay.Value = 0;
            rdbActive.Checked = true;

            isEditMode = false;
        }
        private void btnClear_Click_1(object sender, EventArgs e)
        {
            ClearFields();
        }
        //-------------------------- Specialization ---------------------------------------
        private void cmbDrSpecialization_Leave(object sender, EventArgs e)
        {
            string specialization = cmbDrSpecialization.Text;
            if (!cmbDrSpecialization.Items.Contains(specialization))
            {
                int specializationID = GetSpecializationID(specialization);
                if (specializationID == 0)
                {
                    DialogResult dialogResult = MessageBox.Show("This specialization is not in the list. Do you want to add it?", "Add New Specialization", MessageBoxButtons.YesNo);

                    if (dialogResult == DialogResult.Yes)
                    {
                        // Save the new specialization in the database
                        SaveNewSpecialization(specialization);

                        // Update the ComboBox with the new specialization
                        UpdateSpecializationComboBox();
                    }
                }
            }
        }
        private void SaveNewSpecialization(string specialization)
        {
            string query = "INSERT INTO tblDoctorSpecialization (SpecializationName) VALUES (@specialization)";

            using (SqlConnection connection = new SqlConnection(ValidationHelper.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@specialization", specialization);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            // **New specialization add hone ke baad ComboBox update karein**
            LoadSpecializations();
        }
        public int GetSpecializationID(string specialization)
        {
            if (string.IsNullOrWhiteSpace(specialization))
            {
                MessageBox.Show("Please select a valid specialization.");
                return 0;
            }
            int specializationID = 0;
            string query = "SELECT SpecializationID FROM tblDoctorSpecialization WHERE SpecializationName COLLATE SQL_Latin1_General_CP1_CI_AI = @specialization";
            using (SqlConnection connection = new SqlConnection(ValidationHelper.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@specialization", specialization);
                    connection.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        specializationID = Convert.ToInt32(result);
                    }
                }
            }
            return specializationID;
        }
        private void LoadSpecializations()
        {
            using (SqlConnection conn = new SqlConnection(ValidationHelper.ConnectionString))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT SpecializationID, SpecializationName FROM tblDoctorSpecialization", conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                cmbDrSpecialization.DataSource = dt;
                cmbDrSpecialization.DisplayMember = "SpecializationName";
                cmbDrSpecialization.ValueMember = "SpecializationID";
                cmbDrSpecialization.SelectedIndex = -1; // Default selection empty rakhna
            }
        }
        private void UpdateSpecializationComboBox()
        {
            string query = "SELECT SpecializationName FROM tblDoctorSpecialization";
            using (SqlConnection connection = new SqlConnection(ValidationHelper.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<string> specializations = new List<string>();

                    while (reader.Read())
                    {
                        specializations.Add(reader["SpecializationName"].ToString());
                    }

                    cmbDrSpecialization.DataSource = null; // Clear previous binding
                    cmbDrSpecialization.DataSource = specializations;
                }
            }
        }
        //--------------------------------------------- No DataBase Extra Function-------------------------------
        // The ResizeImage method
        private Image ResizeImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);
            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);
            var newImage = new Bitmap(newWidth, newHeight);
            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }
        private void txtDrPhone_Leave_1(object sender, EventArgs e)
        {
            // Validate the phone number using the centralized validation helper.
            if (!ValidationHelper.IsValidPhone(txtDrPhone.Text))
            {
                MessageBox.Show("Invalid phone format! Please enter a number with 10-15 digits.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnBrowse_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            openFileDialog1.Title = "Select Doctor Image";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Load the image from the file
                Image originalImage = Image.FromFile(openFileDialog1.FileName);
                // Resize the image to a maximum width/height (for example, 300x300)
                Image resizedImage = ResizeImage(originalImage, 300, 300);
                // Display the resized image in the PictureBox
                pictureBoxDoctor.Image = resizedImage;
                // Optionally, store the image path or the resized image for saving to the database
                pictureBoxDoctor.Tag = openFileDialog1.FileName;
            }
        }
        private void txtDrCNIC_Leave_1(object sender, EventArgs e)
        {
            if (!ValidationHelper.IsValidCNIC(txtDrCNIC.Text))
            {
                MessageBox.Show("Invalid CNIC format. Please enter in 12345-1234567-1 format.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void txtDrCNIC_TextChanged(object sender, EventArgs e)
        {
            txtDrCNIC.Text = ValidationHelper.FormatCNIC(txtDrCNIC.Text);
            txtDrCNIC.SelectionStart = txtDrCNIC.Text.Length; // Maintain cursor position
        }
        private void Doctors_Shown(object sender, EventArgs e)
        {
            // Define a dictionary of TextBox and their respective placeholder text
            Dictionary<TextBox, string> textBoxPlaceholders = new Dictionary<TextBox, string>
            {
            { txtDrName, "Enter Doctor Name" },
            { txtDrCNIC, "Enter Doctor CNIC" },
            { txtDrPhone, "Enter Phone" },
            { txtDrEmail, "Enter Email" },
            { txtDrAddress, "Enter Doctor Address" },
            //{ txtDrID, "No,Need to enter anything" },
            { txtDrQualification, "Enter  Qualification" },
            { txtDrLicenseNo, "Enter License No" },
            { txtDrExperience, "Enter Experience" },
            { txtDrUserName, "Enter UserName" },
            { txtDrPassword, "Enter Dr Password" },
            { txtDrRoomNo, "Room No" },
            };
            // Apply the placeholder functionality to all TextBoxes in the form
            foreach (var entry in textBoxPlaceholders)
            {
                ValidationHelper.ApplyPlaceholderTextOnClick(entry.Key, entry.Value);
            }
        }
        private void cmbDrAppointmentDuration_Leave(object sender, EventArgs e)
        {
            string inputText = cmbDrAppointmentDuration.Text.Trim();

            // Agar pehle se "minutes" likha hai, to sirf number extract karna
            if (inputText.EndsWith(" minutes"))
            {
                inputText = inputText.Replace(" minutes", "").Trim();
            }

            // Check if the remaining input is a valid integer
            if (int.TryParse(inputText, out int minutes))
            {
                cmbDrAppointmentDuration.Text = $"{minutes} minutes"; // Auto-add "minutes"
            }
            else
            {
                MessageBox.Show("Invalid input! Please enter a valid number in minutes.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbDrAppointmentDuration.Text = ""; // Reset field if invalid
            }
        }
        private void cmbDrAppointmentDuration_SelectedValueChanged(object sender, EventArgs e)
        {

        }
        private void cmbDrAppointmentDuration_TextChanged(object sender, EventArgs e)
        {

        }
        private void cmbDrAppointmentDuration_KeyPress(object sender, KeyPressEventArgs e)
        {
        }
    }
}