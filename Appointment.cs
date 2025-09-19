using Mysqlx.Crud;
using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClinicMangSystem
{
    public partial class Appointment : BaseForm
    {
        public Appointment()
        {
            InitializeComponent();
            this.Resize += new EventHandler(Appointment_Resize);
            LoadDoctorIDs(); // Load Doctor IDs for AutoComplete
            LoadPatientIDs();
            // Add status options
            cmbStatus.Items.Add("Scheduled");
            cmbStatus.Items.Add("Completed");
            cmbStatus.Items.Add("Cancelled");
        }
        public int AppointmentID { get; set; }
        public string CustomAppointmentID { get; set; }
        private void Appointment_Load(object sender, EventArgs e)
        {
            //btnBook.Enabled = true;
            LoadDoctorIDs();
        }
        public BaseForm MidParent { get; internal set; }
        private void btnUpdates_Click(object sender, EventArgs e)
        {
            // Fetch the values from the form controls
            string customAppointmentID = CustomAppointmentID; // The CustomAppointmentID passed from Form 2
            string patientID = txtPatientID.Text.Trim();
            string doctorID = txtDoctorID.Text.Trim();
            string status = cmbStatus.SelectedItem?.ToString() ?? "Pending";
            string reason = txtReasonForVisit.Text.Trim();
            // Validate the input for Doctor ID and Appointment ID
            if (string.IsNullOrEmpty(doctorID) || !int.TryParse(doctorID, out int doctorIdInt))
            {
                MessageBox.Show("Doctor ID must be a valid number.");
                return;
            }
            try
            {
                using (SqlConnection con = new SqlConnection(ValidationHelper.ConnectionString))
                {
                    con.Open();
                    string query = @" UPDATE tblAppointments
                SET 
                    DrID = @DrID,
                    AppointmentStatus = @Status,
                    AppointmentReason = @Reason
                WHERE CustomAppointmentID = @CustomAppointmentID"; // Use CustomAppointmentID here
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        // Add parameters to the SQL query
                        cmd.Parameters.AddWithValue("@DrID", doctorIdInt);  // Doctor ID
                        cmd.Parameters.AddWithValue("@Status", status);     // Appointment Status
                        cmd.Parameters.AddWithValue("@Reason", reason);     // Appointment Reason (may be NULL)
                        cmd.Parameters.AddWithValue("@CustomAppointmentID", customAppointmentID); // Pass the CustomAppointmentID
                        // Execute the update query
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Appointment successfully updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (Application.OpenForms["ViewAllAppointments"] is ViewAllAppointment viewForm)
                    {
                        viewForm.LoadAppointments(); // Call refresh method for ViewAllAppointments form
                    }
                    ClearFields();
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Database error: " + sqlEx.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }
        private void ClearFields()
        {
            txtPatientName.Clear();
            txtPatientPhone.Clear();
            txtReasonForVisit.Clear();
            txtDoctorID.Clear();
            txtDrSpecialization.Clear();
            txtDrName.Clear();
            cmbDoctorAvailability.SelectedIndex = -1;
            dtpTime.Value = DateTime.Now;
            dtpAppointmentDate.Value = DateTime.Today;
            cmbStatus.SelectedIndex = -1;
        }
        private void btnViewAlls_Click(object sender, EventArgs e)
        {
            ViewAllAppointment form2 = new ViewAllAppointment();
            form2.Show();
        }
        private void Appointment_Resize(object sender, EventArgs e)
        {

        }
//---------------- btn Book   
        private void btnBooks_Click(object sender, EventArgs e)
        {
            // Get values from form controls
            string patientID = txtPatientID.Text.Trim();
            string doctorID = txtDoctorID.Text.Trim();
            // Validate Doctor ID to ensure it's a valid number
            if (string.IsNullOrEmpty(doctorID) || !int.TryParse(doctorID, out int doctorIdInt))
            {
                MessageBox.Show("Doctor ID must be a valid number.");
                return;
            }
            // Use the DateTime object directly for AppointmentDate
            DateTime selectedDate = dtpAppointmentDate.Value.Date;
            DateTime selectedTime = dtpTime.Value;
            string status = cmbStatus.SelectedItem?.ToString() ?? "Pending";
            // Get the reason for the appointment
            string reason = txtReasonForVisit.Text.Trim(); 
            // Use DBNull.Value for empty or null reason
            object reasonValue;
            if (string.IsNullOrEmpty(reason))
            {
                reasonValue = DBNull.Value;
            }
            else
            {
                reasonValue = reason;
            }
            try
            {
                using (SqlConnection con = new SqlConnection(ValidationHelper.ConnectionString))
                {
                    con.Open();
                    string formattedTime = selectedTime.ToString("hh:mm tt"); // AM/PM format
                    DateTime formattedDate = selectedDate; // Use the DateTime object
                    string query = @"
                          INSERT INTO tblAppointments 
                          (PatientID, DrID, AppointmentTime, AppointmentDate, AppointmentStatus, AppointmentReason)
                          VALUES (@PatientID, @DrID, @Time, @Date, @Status, @Reason)";
                      using (SqlCommand cmd = new SqlCommand(query, con))
                      {
                        // Add parameters to the SQL query
                        cmd.Parameters.AddWithValue("@PatientID", patientID);
                        cmd.Parameters.AddWithValue("@DrID", doctorIdInt); 
                        cmd.Parameters.AddWithValue("@Time", formattedTime); 
                        cmd.Parameters.AddWithValue("@Date", formattedDate); 
                        cmd.Parameters.AddWithValue("@Status", status); 
                        cmd.Parameters.AddWithValue("@Reason", reasonValue);
                        // Execute the insert query
                        cmd.ExecuteNonQuery();
                      }
                    MessageBox.Show("Appointment successfully added!\nTime: " + formattedTime,
                                     "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (Application.OpenForms["ViewAllAppointments"] is ViewAllAppointment viewForm)
                    {
                        viewForm.LoadAppointments(); // Call the refresh method
                    }
                    ClearFields();
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Database error: " + sqlEx.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
//----------------- Patient Detail
        private void FetchPatientDetails(string searchBy, string value)
        {
            using (SqlConnection con = new SqlConnection(ValidationHelper.ConnectionString))
            {
                try
                {
                    con.Open();
                    string query = "";
                    if (searchBy == "ID")
                    {
                        query = @"
                            SELECT p.PatientID, p.PatientName, c.PatientPhone 
                            FROM tblPatient p
                            LEFT JOIN tblPatientContact c ON p.PatientID = c.PatientID
                            WHERE p.PatientID = @Value";
                    }
                    else if (searchBy == "Phone")
                    {
                        query = @"
                            SELECT p.PatientID, p.PatientName, c.PatientPhone 
                            FROM tblPatient p
                            LEFT JOIN tblPatientContact c ON p.PatientID = c.PatientID
                            WHERE c.PatientPhone = @Value";
                    }
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Value", value);
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            txtPatientID.Text = reader["PatientID"].ToString();
                            txtPatientName.Text = reader["PatientName"].ToString();
                            txtPatientPhone.Text = reader["PatientPhone"]?.ToString();
                        }
                        else
                        {
                            MessageBox.Show($"{searchBy} not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        private void txtPatientID_Leave_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtPatientID.Text))
            {
                FetchPatientDetails("ID", txtPatientID.Text);
            }
        }
        private void txtPatientPhone_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtPatientPhone.Text))
            {
                if (!ValidationHelper.IsValidPhone(txtPatientPhone.Text))
                {
                    MessageBox.Show("Invalid phone format! Please enter a number with 10-15 digits.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPatientPhone.Focus();
                    return;
                }
                FetchPatientDetails("Phone", txtPatientPhone.Text);
            }
        }
        private void LoadPatientIDs()
        {
            using (SqlConnection con = new SqlConnection(ValidationHelper.ConnectionString))
            {
                string query = "SELECT PatientID FROM tblPatient";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                AutoCompleteStringCollection patientIDs = new AutoCompleteStringCollection();
                while (reader.Read())
                {
                    patientIDs.Add(reader["PatientID"].ToString());
                }
                reader.Close();

                txtPatientID.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txtPatientID.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtPatientID.AutoCompleteCustomSource = patientIDs;
            }
        }
//----------------- Doctor  Detail
        private void txtDoctorID_Leave(object sender, EventArgs e)
        {
            // Validate that DoctorID is numeric and greater than 0 before calling FetchDoctorDetails
            if (string.IsNullOrWhiteSpace(txtDoctorID.Text) || !int.TryParse(txtDoctorID.Text, out int doctorID) || doctorID <= 0)
            {
                MessageBox.Show("Please enter a valid Doctor ID (numeric and greater than 0).", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Call FetchDoctorDetails with the numeric doctorID
            FetchDoctorDetails(doctorID);
        }
        // Doctor IDs Load Karega
        private void LoadDoctorIDs()
        {
            using (SqlConnection con = new SqlConnection(ValidationHelper.ConnectionString))
            {
                string query = "SELECT DrID FROM tblDoctor";  
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                AutoCompleteStringCollection doctorIDs = new AutoCompleteStringCollection();
                while (reader.Read())
                {
                    doctorIDs.Add(reader["DrID"].ToString());
                }
                reader.Close();

                txtDoctorID.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txtDoctorID.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtDoctorID.AutoCompleteCustomSource = doctorIDs;
            }
        }
        // Fetch Doctor details based on the provided Doctor ID
        private void FetchDoctorDetails(int doctorID)
        {
            if (doctorID <= 0) 
            {
                MessageBox.Show("Please enter a valid Doctor ID!", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (SqlConnection con = new SqlConnection(ValidationHelper.ConnectionString))
            {
                try
                {
                    con.Open();
                    string query = @"
                       SELECT d.DrID, d.DrName, s.SpecializationName 
                       FROM tblDoctor d
                       LEFT JOIN tblDoctorProfessional dp ON d.DrID = dp.DrID
                       LEFT JOIN tblDoctorSpecialization s ON dp.SpecializationID = s.SpecializationID
                       WHERE d.DrID = @DoctorID";
                      using (SqlCommand cmd = new SqlCommand(query, con))
                       {
                        cmd.Parameters.AddWithValue("@DoctorID", doctorID);
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            txtDoctorID.Text = reader["DrID"].ToString();
                            txtDrName.Text = reader["DrName"].ToString();
                            txtDrSpecialization.Text = reader["SpecializationName"]?.ToString();
                        }
                        else
                        {
                            MessageBox.Show("Doctor ID not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database Connection Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void cmbDoctorAvailability_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbDoctorAvailability.SelectedItem != null)
            {
                dtpTime.Text = cmbDoctorAvailability.SelectedItem.ToString();
            }
        }
        private void dtpAppointmentDate_ValueChanged_1(object sender, EventArgs e)
        {
            LoadDoctorAvailability();
        }
        // Load Doctor availability (time slots)
        private void LoadDoctorAvailability()
        {
            cmbDoctorAvailability.Items.Clear();
            string doctorID = txtDoctorID.Text.Trim();
            if (string.IsNullOrEmpty(doctorID) || !int.TryParse(doctorID, out _))
            {
                // Don't show message box — just exit silently
                return;
            }
            DateTime selectedDate = dtpAppointmentDate.Value.Date;
            string selectedDay = dtpAppointmentDate.Value.DayOfWeek.ToString();
            using (SqlConnection conn = new SqlConnection(ValidationHelper.ConnectionString))
            {
                conn.Open();
                string query = @"
                   SELECT DrConsultationStart, DrConsultationEnd, DrAppointmentDuration
                    FROM tblDoctorAvailability 
                    WHERE DrID = @drID 
                    AND DrAvailableDays LIKE '%' + @selectedDay + '%'";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@drID", Convert.ToInt32(doctorID));
                    cmd.Parameters.AddWithValue("@selectedDay", selectedDay);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            TimeSpan startTime = reader.GetTimeSpan(0);
                            TimeSpan endTime = reader.GetTimeSpan(1);
                            string slotDurationStr = reader.GetString(2);

                            int slotDuration = ExtractNumbers(slotDurationStr);
                            if (slotDuration > 0)
                            {
                                List<string> availableSlots = GenerateTimeSlots(
                                    startTime, endTime, slotDuration,
                                    selectedDate.ToString("yyyy-MM-dd"), doctorID);

                                if (availableSlots.Count > 0)
                                {
                                    cmbDoctorAvailability.Items.AddRange(availableSlots.ToArray());
                                }
                                else
                                {
                                    MessageBox.Show("All slots are booked for this doctor on this day.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid slot duration format in the database.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Doctor is not available on this day.");
                        }
                    }
                }
            }
        }
        //  Updated ExtractNumbers method
        private int ExtractNumbers(string input)
        {
            var match = System.Text.RegularExpressions.Regex.Match(input, @"\d+");
            return match.Success ? int.Parse(match.Value) : 0;
        }
        private List<string> GenerateTimeSlots(TimeSpan startTime, TimeSpan endTime, int slotDuration, string selectedDate, string doctorID)
        {
            List<string> availableSlots = new List<string>();
            // Safely parse appointment date
            if (!DateTime.TryParse(selectedDate, out DateTime appointmentDate))
            {
                MessageBox.Show("Invalid date format. Please select a valid date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return availableSlots;
            }
            // Safely parse doctorID
            if (!int.TryParse(doctorID, out int drID))
            {
                MessageBox.Show("Invalid Doctor ID. Please check and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return availableSlots;
            }
            string formattedDate = appointmentDate.ToString("yyyy-MM-dd");
            HashSet<string> bookedSlots = new HashSet<string>();
            using (SqlConnection conn = new SqlConnection(ValidationHelper.ConnectionString))
            {
                conn.Open();
                string bookedSlotsQuery = @"SELECT AppointmentTime FROM tblAppointments 
                                    WHERE DrID = @drID AND AppointmentDate = @appointmentDate";

                using (SqlCommand cmd = new SqlCommand(bookedSlotsQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@drID", drID);
                    cmd.Parameters.AddWithValue("@appointmentDate", formattedDate);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                // Safely parse time string
                                string dbTime = reader.GetString(0);
                                if (DateTime.TryParse(dbTime, out DateTime bookedTime))
                                {
                                    string bookedTimeString = bookedTime.ToString("hh:mm tt");
                                    bookedSlots.Add(bookedTimeString);
                                }
                            }
                        }
                    }
                }
            }

            //  Generate available slots
            TimeSpan currentSlot = startTime;
            while (currentSlot < endTime)
            {
                string formattedSlot = DateTime.Today.Add(currentSlot).ToString("hh:mm tt");

                if (!bookedSlots.Contains(formattedSlot))
                {
                    availableSlots.Add(formattedSlot);
                }

                currentSlot = currentSlot.Add(TimeSpan.FromMinutes(slotDuration));
            }

            if (availableSlots.Count == 0)
            {
                MessageBox.Show("No available slots today.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return availableSlots;
        }

//----------------------- Without DataBase  --
        private void Appointment_Shown(object sender, EventArgs e)
        {
            // Define a dictionary of TextBox and their respective placeholder text
            Dictionary<TextBox, string> textBoxPlaceholders = new Dictionary<TextBox, string>
            {
            { txtPatientID, "Enter Patient ID" },
            { txtPatientName, "Enter Patient Name" },
            { txtPatientPhone, "Enter Patient Phone" },
            { txtReasonForVisit, "Enter Reason"},
            { txtDoctorID, "Enter Doctor ID" },
            { txtDrSpecialization, "Specialization" },
            { txtDrName, "Enter Doctor Name" },
            };
            // Apply the placeholder functionality to all TextBoxes in the form
            foreach (var entry in textBoxPlaceholders)
            {
                ValidationHelper.ApplyPlaceholderTextOnClick(entry.Key, entry.Value);
            }
        }
    }
}