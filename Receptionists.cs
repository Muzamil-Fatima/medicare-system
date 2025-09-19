using MySqlX.XDevAPI.Common;
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
    public partial class Receptionists : BaseForm
    {
        SqlConnection con = new SqlConnection(@"Server=DESKTOP-JLQCCR6;Database=ClinicManagement;Integrated Security=True");
        public int receptionistID = 0;
        public Receptionists()
        {
            InitializeComponent();
        }
        public BaseForm MidParent { get; internal set; }
        public void Receptionists_Load(object sender, EventArgs e)
        {
            ValidationHelper.LoadGenderComboBox(cmbGender);
        }
        //public void LoadData(string id, string name, string phone, string gender, string cnic, string password, string address)
        //{
        //    receptionistID = int.Parse(id);
        //    txtName.Text = name;
        //    txtPhone.Text = phone;
        //    cmbGender.Text = gender;
        //    txtCNIC.Text = cnic;
        //    txtUserName.Text = password;
        //    txtAddress.Text = address;
        //}
        private void btnClear_Click_1(object sender, EventArgs e)
        {
            ClearFields();
        }
        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            // Agar receptionist select nahi hui, to ViewAllReceptionists form open karega
            if (receptionistID == 0)
            {
                ViewAllReceptionists viewForm = new ViewAllReceptionists();
                viewForm.ShowDialog(); // Wait karega jab tak user receptionist select nahi karta
                return;
            }
            // Agar receptionist select ho chuki hai, to update karega
            if (ValidateFields())
            {
                UpdateReceptionistData();
                LoadReceptionistData();
            }
        }
        private void UpdateReceptionistData()
        {
            con.Open();
            using (SqlTransaction transaction = con.BeginTransaction())
            {
                try
                {
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        cmd.Transaction = transaction;
                        cmd.CommandText = @"UPDATE tblReceptionist 
                                 SET ReceptionistName = @Name, ReceptionistEmail = @Email, 
                                     ReceptionistCNIC = @CNIC, GenderID = @GenderID
                                 WHERE ReceptionistID = @ReceptionistID";
                        cmd.Parameters.AddWithValue("@ReceptionistID", receptionistID);
                        cmd.Parameters.AddWithValue("@Name", txtName.Text);
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                        cmd.Parameters.AddWithValue("@CNIC", txtCNIC.Text);
                        cmd.Parameters.AddWithValue("@GenderID", cmbGender.SelectedValue);
                        cmd.ExecuteNonQuery();
                    }

                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        cmd.Transaction = transaction;
                        cmd.CommandText = @"UPDATE tblReceptionistContact 
                                 SET ReceptionistPhone = @Phone, ReceptionistAddress = @Address
                                 WHERE ReceptionistID = @ReceptionistID";
                        cmd.Parameters.AddWithValue("@ReceptionistID", receptionistID);
                        cmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                        cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                        cmd.ExecuteNonQuery();
                    }
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        cmd.Transaction = transaction;
                        cmd.CommandText = @"UPDATE tblReceptionistLogin 
                                 SET ReceptionistUsername = @Username, ReceptionistPassword = @Password
                                 WHERE ReceptionistID = @ReceptionistID";
                        cmd.Parameters.AddWithValue("@ReceptionistID", receptionistID);
                        cmd.Parameters.AddWithValue("@Username", txtUserName.Text);
                        cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
                        cmd.ExecuteNonQuery();
                    }
                    transaction.Commit();
                    MessageBox.Show("Receptionist details updated successfully.");
                    LoadReceptionistData();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error updating data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                }
            }
        }
        private void LoadReceptionistData()
        {
        }
        private bool IsUsernameUnique(string username)
        {
            try
            {
                con.Open();
                string query = "SELECT COUNT(*) FROM tblReceptionistLogin WHERE ReceptionistUsername = @Username";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    int count = (int)cmd.ExecuteScalar();
                    return count == 0; // If count is 0, username is unique
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking username: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }
        private bool ValidateFields()
        {
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtCNIC.Text) ||
                string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtPhone.Text) ||
                string.IsNullOrEmpty(txtAddress.Text) || string.IsNullOrEmpty(txtPassword.Text) ||
                string.IsNullOrEmpty(txtUserName.Text) || cmbGender.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill all fields before proceeding!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
        private int GetReceptionistID(string cnic)
        {
            string query = "SELECT ReceptionistID FROM tblReceptionist WHERE ReceptionistCNIC = @CNIC";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@CNIC", cnic);
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {// Validation Check
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtCNIC.Text) ||
                string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtAddress.Text) || string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtUserName.Text) || cmbGender.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill all fields before proceeding!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Check if Username is Unique BEFORE Inserting
            if (!IsUsernameUnique(txtUserName.Text))
            {
                MessageBox.Show("This Username is already taken. Please choose a different one!", "Duplicate Username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SqlTransaction transaction = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                // Transaction Start
                transaction = con.BeginTransaction();
                string insertReceptionist = @"INSERT INTO tblReceptionist 
                     (ReceptionistName, GenderID, ReceptionistEmail, ReceptionistCNIC) 
                     OUTPUT INSERTED.ReceptionistID
                     VALUES (@Name, @Gender, @Email, @CNIC)";
                int newReceptionistID;
                using (SqlCommand cmd = new SqlCommand(insertReceptionist, con, transaction))
                {
                    cmd.Parameters.AddWithValue("@Name", txtName.Text);
                    cmd.Parameters.AddWithValue("@Gender", cmbGender.SelectedValue);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@CNIC", txtCNIC.Text);
                    newReceptionistID = (int)cmd.ExecuteScalar();
                }
                // Insert Contact Information
                string insertContact = @"INSERT INTO tblReceptionistContact 
            (ReceptionistID, ReceptionistPhone, ReceptionistAddress) 
            VALUES (@ReceptionistID, @Phone, @Address)";

                using (SqlCommand cmd = new SqlCommand(insertContact, con, transaction))
                {
                    cmd.Parameters.AddWithValue("@ReceptionistID", newReceptionistID);
                    cmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                    cmd.ExecuteNonQuery();
                }
                // Insert Login Information
                string insertLogin = @"INSERT INTO tblReceptionistLogin 
            (ReceptionistID, ReceptionistUsername, ReceptionistPassword) 
            VALUES (@ReceptionistID, @Username, @Password)";
                using (SqlCommand cmd = new SqlCommand(insertLogin, con, transaction))
                {
                    cmd.Parameters.AddWithValue("@ReceptionistID", newReceptionistID);
                    cmd.Parameters.AddWithValue("@Username", txtUserName.Text);
                    cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
                    cmd.ExecuteNonQuery();
                }
                // Transaction Commit (Final Save)
                transaction.Commit();
                MessageBox.Show("Data Added Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
            }
            catch (SqlException ex)
            {
                if (transaction != null)
                    transaction.Rollback();  // Rollback if any error occurs

                MessageBox.Show("Database error: " + ex.Message, "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                if (transaction != null)
                    transaction.Rollback();
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        private void DeleteReceptionistContact(int receptionistID)
        {
            string query = "DELETE FROM tblReceptionistContact WHERE ReceptionistID = @id";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@id", receptionistID);
                cmd.ExecuteNonQuery();
            }
        }
        private void DeleteReceptionistLogin(int receptionistID)
        {
            string query = "DELETE FROM tblReceptionistLogin WHERE ReceptionistID = @id";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@id", receptionistID);
                cmd.ExecuteNonQuery();
            }
        }
        private bool DeleteReceptionist(int receptionistID)
        {
            string query = "DELETE FROM tblReceptionist WHERE ReceptionistID = @id";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@id", receptionistID);
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        private void RefreshReceptionistGrid()
        {
            //ViewAllReceptionists viewForm = (ViewAllReceptionists)Application.OpenForms["ViewAllReceptionists"];
            //if (viewForm != null)
            //{
            //    viewForm.LoadReceptionistData();
            //}
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCNIC.Text))
            {
                MessageBox.Show("Please enter CNIC to delete the receptionist!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult result = MessageBox.Show("Are you sure you want to delete this receptionist?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    con.Open(); 

                    // Get ReceptionistID
                    int receptionistID = GetReceptionistID(txtCNIC.Text);
                    if (receptionistID == -1)
                    {
                        MessageBox.Show("No record found for this CNIC!", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Delete Related Records
                    DeleteReceptionistContact(receptionistID);
                    DeleteReceptionistLogin(receptionistID);

                    // Delete Receptionist
                    if (DeleteReceptionist(receptionistID))
                    {
                        MessageBox.Show($"Receptionist with CNIC {txtCNIC.Text} deleted successfully!", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearFields();

                        // Refresh grid in ViewAllReceptionists form
                        RefreshReceptionistGrid();
                    }
                    else
                    {
                        MessageBox.Show("Error deleting the receptionist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Database error: " + ex.Message, "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close(); 
                    }
                }
            }
        }
        private void ClearFields()
        {
            receptionistID = 0;
            txtName.Text = "";
            txtPhone.Text = "";
            cmbGender.SelectedIndex = -1;
            txtCNIC.Text = "";
            txtUserName.Text = "";
            txtAddress.Text = "";
            txtEmail.Text = "";
        }
        private void txtPhone_Leave_1(object sender, EventArgs e)
        {
            // Validate the phone number using the centralized validation helper.
            if (!ValidationHelper.IsValidPhone(txtPhone.Text))
            {
                MessageBox.Show("Invalid phone format! Please enter a number with 10-15 digits.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txtCNIC_Leave_1(object sender, EventArgs e)
        {
            if (!ValidationHelper.IsValidCNIC(txtCNIC.Text))
            {
                MessageBox.Show("Invalid CNIC format. Please enter in 12345-1234567-1 format.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void txtCNIC_TextChanged(object sender, EventArgs e)
        {
            txtCNIC.Text = ValidationHelper.FormatCNIC(txtCNIC.Text);
            txtCNIC.SelectionStart = txtCNIC.Text.Length;
        }
        private void Receptionists_Shown(object sender, EventArgs e)
        {
            // Define a dictionary of TextBox and their respective placeholder text
            Dictionary<TextBox, string> textBoxPlaceholders = new Dictionary<TextBox, string>
            {
            { txtName, "Enter Name" },
            { txtPhone, "Enter Phone" },
            { txtCNIC, "xxxxx-xxxxxxx-x" },
            { txtAddress, "Enter Address" },
            { txtUserName, "Enter User Name" },
            { txtPassword, "Enter Password" },
            { txtEmail, "Enter Email" },
            };
            // Apply the placeholder functionality to all TextBoxes in the form
            foreach (var entry in textBoxPlaceholders)
            {
                ValidationHelper.ApplyPlaceholderTextOnClick(entry.Key, entry.Value);
            }
        }
    }
}