using System;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace ClinicMangSystem
{
    class ValidationHelper
    {
        // Static property for Connection String
        public static string ConnectionString
        {
            get
            {
                return "Server=DESKTOP-JLQCCR6;Database=ClinicManagement;Integrated Security=True;";
            }
        }

        public static class DoctorSession
        {
            public static int DoctorID { get; set; }
        }

        // ✅ Search Function for All Forms
        public static void SearchData(DataGridView dgv, string tableName, string searchText)
        {
            using (SqlConnection con = new SqlConnection(ValidationHelper.ConnectionString))
            {
                string query = $"SELECT * FROM {tableName} WHERE CONCAT(ID, DrName, CNIC, Phone, Email, Specialization) LIKE @searchText";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@searchText", "%" + searchText + "%");

                DataTable dt = new DataTable();
                da.Fill(dt);
                dgv.DataSource = dt; //GridView ko Update
            }
        }
        public static void LoadGenderComboBox(ComboBox cmbGender)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ValidationHelper.ConnectionString))
                {
                    string query = "SELECT GenderID, Gender FROM tblGender";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbGender.DisplayMember = "Gender";   // User ko Gender dikhaye
                    cmbGender.ValueMember = "GenderID";   // Backend me GenderID store ho
                    cmbGender.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // ✅ CNIC Validation & Auto-Formatting
        public static string FormatCNIC(string cnic)
        {
            if (string.IsNullOrWhiteSpace(cnic))
                return "";
            string rawCNIC = cnic.Replace("-", "").Trim();

            // Ensure only digits are entered
            if (!Regex.IsMatch(rawCNIC, @"^\d*$"))
                return ""; // Return empty if non-numeric

            // Auto format: 12345-1234567-1
            if (rawCNIC.Length > 5)
                rawCNIC = rawCNIC.Insert(5, "-");

            if (rawCNIC.Length > 13)
                rawCNIC = rawCNIC.Insert(13, "-");

            return rawCNIC;
        }

        // ✅ Validate CNIC 
        public static bool IsValidCNIC(string cnic)
        {
            string pattern = @"^\d{5}-\d{7}-\d{1}$";
            return Regex.IsMatch(cnic, pattern);
        }
        public static bool IsValidPhone(string phone)
        {
            string pattern = @"^\d{10,15}$";
            return Regex.IsMatch(phone, pattern);
        }
            // Method to apply placeholder text on TextBox click and leave events
            public static void ApplyPlaceholderTextOnClick(TextBox textBox, string placeholder)
            {
                // Set initial placeholder text with gray color
                if (textBox.Text == "")
                {
                    textBox.Text = placeholder;
                    textBox.ForeColor = Color.Gray;
                }

                // Remove placeholder text on click (if it's the placeholder text)
                textBox.Click += (sender, e) =>
                {
                    if (textBox.Text == placeholder)
                    {
                        textBox.Text = ""; // Remove placeholder text
                        textBox.ForeColor = Color.Black; // Change text color to black
                    }
                };

                // Change text color to black when typing starts (TextChanged event)
                textBox.TextChanged += (sender, e) =>
                {
                    if (textBox.Text != placeholder && textBox.ForeColor != Color.Black)
                    {
                        textBox.ForeColor = Color.Black; // Change text color to black
                    }
                };

                // Restore placeholder text if the TextBox is left empty
                textBox.Leave += (sender, e) =>
                {
                    if (string.IsNullOrEmpty(textBox.Text))
                    {
                        textBox.Text = placeholder; // Restore placeholder text
                        textBox.ForeColor = Color.Gray; // Set text color to gray
                    }
                };

                // Optionally handle GotFocus to remove placeholder text
                textBox.GotFocus += (sender, e) =>
                {
                    if (textBox.Text == placeholder)
                    {
                        textBox.Text = ""; // Remove placeholder text
                        textBox.ForeColor = Color.Black; // Change text color to black
                    }
                };

                // Optionally handle LostFocus event (if needed for specific cases)
                textBox.LostFocus += (sender, e) =>
                {
                    if (string.IsNullOrEmpty(textBox.Text))
                    {
                        textBox.Text = placeholder; // Restore placeholder text
                        textBox.ForeColor = Color.Gray; // Set text color to gray
                    }
                };
            }
        public static void HighlightMatchingCells(DataGridView dgv, TextBox txtSearch)
        {
            string searchText = txtSearch.Text.ToLower();

            foreach (DataGridViewRow row in dgv.Rows)
            {
                bool matchFound = false;

                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null && cell.Value.ToString().ToLower().Contains(searchText) && !string.IsNullOrEmpty(searchText))
                    {
                        cell.Style.BackColor = Color.Yellow;
                        matchFound = true;
                    }
                    else
                    {
                        cell.Style.BackColor = Color.White;
                    }
                }

                row.DefaultCellStyle.BackColor = matchFound ? Color.LightYellow : Color.White;
            }
        }
        public static void ScrollToFirstMatch(DataGridView dgv, TextBox txtSearch)
        {
            string searchText = txtSearch.Text.ToLower();

            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                DataGridViewRow row = dgv.Rows[i];

                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null && cell.Value.ToString().ToLower().Contains(searchText) && !string.IsNullOrEmpty(searchText))
                    {
                        dgv.FirstDisplayedScrollingRowIndex = i;
                        return;
                    }
                }
            }
        }
        public static void ResetGrid(DataGridView dgv)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.BackColor = Color.White;
                }
                row.DefaultCellStyle.BackColor = Color.White;
            }
        }

    }
}