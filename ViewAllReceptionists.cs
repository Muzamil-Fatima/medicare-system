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
    public partial class ViewAllReceptionists : BaseForm
    {
        SqlConnection con = new SqlConnection(@"Server=DESKTOP-JLQCCR6;Database=ClinicManagement;Integrated Security=True");
        public ViewAllReceptionists()
        {
            InitializeComponent();
            LoadReceptionistData();
        }
        public BaseForm MidParent { get; internal set; }
        private void LoadReceptionistData()
        {
            try
            {
                con.Open();
                string query = @"SELECT 
                     r.ReceptionistID, 
                     r.ReceptionistName, 
                     r.ReceptionistEmail, 
                     r.ReceptionistCNIC, 
                     g.Gender AS Gender,   -- Corrected column name
                     c.ReceptionistPhone, 
                     c.ReceptionistAddress, 
                     l.ReceptionistUsername, 
                     l.ReceptionistStatus
                 FROM tblReceptionist r
                 LEFT JOIN tblReceptionistContact c ON r.ReceptionistID = c.ReceptionistID
                 LEFT JOIN tblReceptionistLogin l ON r.ReceptionistID = l.ReceptionistID
                 LEFT JOIN tblGender g ON r.GenderID = g.GenderID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dvtReceptionists.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading receptionist data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }
        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            // Highlight matching cells and move matching rows to top
            ValidationHelper.HighlightMatchingCells(dvtReceptionists, txtSearch);
            //ValidationHelper.MoveMatchingRowsToTop(dvtReceptionists, txtSearch);
        }
        private void dvtReceptionists_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dvtReceptionists.Rows[e.RowIndex];
                Receptionists mainForm = (Receptionists)Application.OpenForms["Receptionists"];

                if (mainForm != null)
                {
                    mainForm.txtName.Text = row.Cells["ReceptionistName"].Value.ToString();
                    mainForm.txtPhone.Text = row.Cells["ReceptionistPhone"].Value.ToString();
                    mainForm.cmbGender.Text = row.Cells["Gender"].Value.ToString();
                    mainForm.txtCNIC.Text = row.Cells["ReceptionistCNIC"].Value.ToString();
                    mainForm.txtUserName.Text = row.Cells["ReceptionistUsername"].Value.ToString();
                    mainForm.txtAddress.Text = row.Cells["ReceptionistAddress"].Value.ToString();
                    mainForm.txtEmail.Text = row.Cells["ReceptionistEmail"].Value.ToString();

                    // Receptionist ID ko backend ke liye store kar rahe hain (UI me nahi dikhega)
                    mainForm.receptionistID = Convert.ToInt32(row.Cells["ReceptionistID"].Value);
                }
                this.Close();
            }
        }
    }
}