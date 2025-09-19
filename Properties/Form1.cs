using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClinicMangSystem
{
    public partial class Form1 : Form
    {
        SqlConnection con = new SqlConnection(@"Server=DESKTOP-JLQCCR6;Database=ClinicManagement;Integrated Security=True;");
        public object Properties { get; private set; }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadRoles();
            int cornerRadius = 20;
            var path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(new Rectangle(0, 0, cornerRadius, cornerRadius), 180, 90);
            path.AddArc(new Rectangle(this.Width - cornerRadius, 0, cornerRadius, cornerRadius), -90, 90);
            path.AddArc(new Rectangle(this.Width - cornerRadius, this.Height - cornerRadius, cornerRadius, cornerRadius), 0, 90);
            path.AddArc(new Rectangle(0, this.Height - cornerRadius, cornerRadius, cornerRadius), 90, 90);
            path.CloseFigure();
            this.Region = new Region(path);
        }
        public Form1()
        {
            InitializeComponent();
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.PasswordChar = '*';  
        }
//-------------------------------------------------------------------------------------------
        private void btnLogin_Click_1(object sender, EventArgs e)
        {
            string username = txtUserName.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                con.Open();

                string query = @"
            SELECT u.ID, u.UserName, r.RoleName, 'Admin' AS RoleType
            FROM UserLogin u
            JOIN UserRole r ON u.RoleID = r.RoleID
            WHERE u.UserName = @Username AND u.UserPassword = @Password

            UNION

            SELECT dl.DrID, dl.DrUsername, 'Doctor' AS RoleName, 'Doctor' AS RoleType
            FROM tblDoctorLogin dl
            WHERE dl.DrUsername = @Username AND dl.DrPassword = @Password AND dl.DrStatus = 1

            UNION

            SELECT rl.ReceptionistID, rl.ReceptionistUsername, 'Receptionist' AS RoleName, 'Receptionist' AS RoleType
            FROM tblReceptionistLogin rl
            WHERE rl.ReceptionistUsername = @Username AND rl.ReceptionistPassword = @Password AND rl.ReceptionistStatus = 1";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string uname = reader["UserName"].ToString();
                    string role = reader["RoleName"].ToString();
                    string roleType = reader["RoleType"].ToString();

                    MessageBox.Show($"Welcome, {uname}!\nYour Role: {role}", "Login Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    OpenDashboard(roleType);
                }
                else
                {
                    MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        private void OpenDashboard(string roleType)
        {
            this.Hide();

            if (roleType == "Admin")
            {
                HelpCenter admin = new HelpCenter();
                admin.Show();
            }
            else if (roleType == "Doctor")
            {
                AssignPatient doctor = new AssignPatient();
                doctor.Show();
            }
            else if (roleType == "Receptionist")
            {
                Patients receptionist = new Patients();
                receptionist.Show();
            }
            else
            {
                MessageBox.Show("Unauthorized Role.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Show();
            }
        }

        private void LoadRoles()
        {
            try
            {
                con.Open();
                string query = "SELECT RoleName FROM UserRole";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    comboBox1.Items.Add(reader["RoleName"].ToString());
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }
//---------------------------------------------------------------------------------------
        private void btnSignUp_Click(object sender, EventArgs e)
        {
            SignupForm signup = new SignupForm();
            signup.Show();
            this.Hide();
        }
        private void btnForget_Click(object sender, EventArgs e)
        {
            ForgetPassword forgetPassword = new ForgetPassword();
            forgetPassword.Show();
            this.Hide();
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
        private void Form1_Shown(object sender, EventArgs e)
        {
            // Define a dictionary of TextBox and their respective placeholder text
            Dictionary<TextBox, string> textBoxPlaceholders = new Dictionary<TextBox, string>
            {
              { txtUserName, "Enter Username" },
              { txtPassword, "Enter Password" }
            };

            // Apply the placeholder functionality to all TextBoxes in the form
            foreach (var entry in textBoxPlaceholders)
            {
                TextBox txtBox = entry.Key;
                string placeholder = entry.Value;

                // Apply the placeholder functionality
                ValidationHelper.ApplyPlaceholderTextOnClick(txtBox, placeholder);

                // Special handling for Password TextBox
                if (txtBox == txtPassword)
                {
                    txtBox.UseSystemPasswordChar = false; // Disable password dots when placeholder is active

                    txtBox.GotFocus += (s, ev) =>
                    {
                        if (txtBox.Text == placeholder)
                        {
                            txtBox.Text = "";
                            txtBox.UseSystemPasswordChar = true; // Enable password dots
                        }
                    };

                    txtBox.LostFocus += (s, ev) =>
                    {
                        if (string.IsNullOrWhiteSpace(txtBox.Text))
                        {
                            txtBox.Text = placeholder;
                            txtBox.UseSystemPasswordChar = false; // Disable dots when placeholder is visible
                        }
                    };
                }
            }
        }
        private void btnLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click_1(sender, e); // Calls the Login button function
            }
        }
    }
}
