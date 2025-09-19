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
    public partial class SignupForm : Form
    {
        public SignupForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Form1 Login = new Form1();
            Login.Show();
            this.Hide();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string role = comboBox1.SelectedItem?.ToString()?.Trim();
            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtCreatePassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();
            bool termsAccepted = chkConditions.Checked;

            // Validate input fields
            if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(name) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Please fill all fields!");
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match!");
                return;
            }

            if (!termsAccepted)
            {
                MessageBox.Show("You must accept the terms and conditions.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(ValidationHelper.ConnectionString))
            {
                try
                {
                    conn.Open();

                    // Get RoleID from UserRole table
                    string getRoleIdQuery = "SELECT RoleID FROM UserRole WHERE RoleName = @role";
                    int roleId;

                    using (SqlCommand roleCmd = new SqlCommand(getRoleIdQuery, conn))
                    {
                        roleCmd.Parameters.AddWithValue("@role", role);
                        object result = roleCmd.ExecuteScalar();

                        if (result == null)
                        {
                            MessageBox.Show("Invalid role selected!");
                            return;
                        }

                        roleId = Convert.ToInt32(result);
                    }

                    // Insert user into UserLogin table
                    string insertUserQuery = "INSERT INTO UserLogin (RoleID, UserName, UserEmail, UserPassword) VALUES (@roleId, @name, @email, @pass)";

                    using (SqlCommand userCmd = new SqlCommand(insertUserQuery, conn))
                    {
                        userCmd.Parameters.AddWithValue("@roleId", roleId);
                        userCmd.Parameters.AddWithValue("@name", name);
                        userCmd.Parameters.AddWithValue("@email", email);
                        userCmd.Parameters.AddWithValue("@pass", password);

                        userCmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Account Created Successfully!");

                    Home dashboard = new Home();
                    dashboard.Show();
                    this.Hide();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void SignupForm_Shown(object sender, EventArgs e)
        {
            // Define a dictionary of TextBox and their respective placeholder text
            Dictionary<TextBox, string> textBoxPlaceholders = new Dictionary<TextBox, string>
            {
            { txtName , "Enter Name " },
            { txtConfirmPassword, "Confirm Password" },
             { txtCreatePassword , "Create Password" },
            { txtEmail, "Enter Email" },
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
        private void btnRegister_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnRegister_Click(sender, e); 
            }
        }
    }

}

