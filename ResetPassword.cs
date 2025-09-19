using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ClinicMangSystem
{
    public partial class ResetPassword : Form
    {
        private string userEmail; // We'll store the email here
        public ResetPassword(string email)
        {
            InitializeComponent();
            userEmail = email;
        }
        private void btnChange_Click(object sender, EventArgs e)
        {
            // 1. Gather input
            string newPassword = txtNewPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();
            // 2. Basic validation
            if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Please enter both password fields!");
                return;
            }
            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Passwords do not match!");
                return;
            }
            // 3. Hash the password
            string hashedPassword = HashPassword(newPassword);
            // 4. Update the password in the database
            using (SqlConnection conn = new SqlConnection(ValidationHelper.ConnectionString))
            {
                conn.Open();
                string updateQuery = "UPDATE UserLogin SET UserPassword = @NewPass WHERE UserEmail = @Email";

                using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@NewPass", hashedPassword);
                    cmd.Parameters.AddWithValue("@Email", userEmail);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Password changed successfully!");
                        // back to login form
                        Form1 Login = new Form1();
                        Login.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Error updating password. Please try again.");
                    }
                }
            }
        }
        // Function to hash the password using SHA256
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
        private void ResetPassword_Shown(object sender, EventArgs e)
        {
            // Define a dictionary of TextBox and their respective placeholder text
            Dictionary<TextBox, string> textBoxPlaceholders = new Dictionary<TextBox, string>
            {
            { txtNewPassword, "New Password" },
            { txtConfirmPassword, "Confirm Password" },
            };
            // Apply the placeholder functionality to all TextBoxes in the form
            foreach (var entry in textBoxPlaceholders)
            {
                ValidationHelper.ApplyPlaceholderTextOnClick(entry.Key, entry.Value);
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Exit Confirmation",
                                               MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        private void btnChange_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnChange_Click(sender, e); // Calls the Login button function
            }
        }
    }
}