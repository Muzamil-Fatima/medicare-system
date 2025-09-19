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
using Microsoft.VisualBasic;

namespace ClinicMangSystem
{
    public partial class ForgetPassword : Form
    {
        public object Interaction { get; private set; }
        public ForgetPassword()
        {
            InitializeComponent();
        }
        private void btnContinue_Click(object sender, EventArgs e)
        {
            // 1. Get the email from the textbox
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Please enter an email address!");
                return;
            }
            using (SqlConnection conn = new SqlConnection(ValidationHelper.ConnectionString))
            {
                conn.Open();
                string checkUserQuery = "SELECT ID FROM UserLogin WHERE UserEmail = @Email";
                int userId = -1;

                using (SqlCommand checkCmd = new SqlCommand(checkUserQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@Email", email);
                    object result = checkCmd.ExecuteScalar();

                    if (result != null)
                    {
                        userId = Convert.ToInt32(result);
                    }
                }
                if (userId == -1)
                {
                    MessageBox.Show("This email does not exist in our records!");
                    return;
                }
                // 3. Show a custom password input form
                string newPassword = ShowPasswordDialog();
                if (string.IsNullOrEmpty(newPassword))
                {
                    MessageBox.Show("Password reset cancelled or empty password.");
                    return;
                }
                // 4. Update the password in the database
                string updateQuery = "UPDATE UserLogin SET UserPassword = @NewPassword WHERE ID = @UserID";
                using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@NewPassword", newPassword);
                    updateCmd.Parameters.AddWithValue("@UserID", userId);

                    int rowsAffected = updateCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Password updated successfully!");
                        Form1 s = new Form1();
                        this.Hide();
                        s.Show();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update the password. Please try again.");
                    }
                }
            }
        }
        private string ShowPasswordDialog()
        {
            Form passwordForm = new Form()
            {
                Width = 350,
                Height = 200,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen
            };

            Label lbl = new Label() { Left = 20, Top = 20, Text = "Enter new password:", Width = 300 };
            TextBox txtPassword = new TextBox() { Left = 20, Top = 50, Width = 300, PasswordChar = '*' };
            Button btnOK = new Button() { Text = "OK", Left = 120, Width = 100, Top = 90 };
            btnOK.DialogResult = DialogResult.OK;

            passwordForm.Controls.Add(lbl);
            passwordForm.Controls.Add(txtPassword);
            passwordForm.Controls.Add(btnOK);
            passwordForm.AcceptButton = btnOK;

            return passwordForm.ShowDialog() == DialogResult.OK ? txtPassword.Text : "";
        }

        private void ForgetPassword_Shown(object sender, EventArgs e)
        {
            // Define a dictionary of TextBox and their respective placeholder text
            Dictionary<TextBox, string> textBoxPlaceholders = new Dictionary<TextBox, string>
            {
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

        private void btnContinue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnContinue_Click(sender, e); 
            }
        }
    }
}