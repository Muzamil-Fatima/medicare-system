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
    public partial class Lab : BaseForm
    {
        public Lab()
        {
            InitializeComponent();
            DisplayLab();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=DESKTOP-JLQCCR6;Initial Catalog=ClinicManagement; Integrated Security=true");
        private void DisplayLab()
        {
            Con.Open();
            string Query = "select*from  tblTest";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            TestGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        int Key = 0;

        public BaseForm MidParent { get; internal set; }

        private void clear()
        {
            TestNameTb.Text = "";
            TestCostTb.Text = "";
            TestTypeTb.Text = "";
            PatNameTb.Text = "";
            DocTb.Text = "";
            DateTime selectedDate = TestConCb.Value;
            Key = 0;
        }
        private void Lab_Load(object sender, EventArgs e)
        {

        }

        private void DelBtn_Click(object sender, EventArgs e)
        {

            if (Key == 0)
            {
                MessageBox.Show("Please select a Test to delete.");
                return;
            }
            try
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM tblTest WHERE testNum=@TKey", Con);
                cmd.Parameters.AddWithValue("@TKey", Key);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Test record Deleted");
                    DisplayLab();
                }
                else
                {
                    MessageBox.Show("Deletion Failed. Make sure the selected Test exists.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                Con.Close();
                DisplayLab();
                clear();
            }
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            if
(TestNameTb.Text == "" || TestCostTb.Text == "" || TestTypeTb.Text == "" || PatNameTb.Text == "" || DocTb.Text == "" || TestConCb.Value == DateTime.MinValue)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into tblTest(testName,testCost,testType,patientName,refDoc,testConduct) values(@TN,@TC,@TT,@PN,@RD,@TCO)", Con);
                    cmd.Parameters.AddWithValue("@TN", TestNameTb.Text);
                    cmd.Parameters.AddWithValue("@TC", TestCostTb.Text);
                    cmd.Parameters.AddWithValue("@TT", TestTypeTb.Text);
                    cmd.Parameters.AddWithValue("@PN", PatNameTb.Text);
                    cmd.Parameters.AddWithValue("@RD", DocTb.Text);
                    cmd.Parameters.AddWithValue("@TCO", TestConCb.Value);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Test added");
                    Con.Close();
                    DisplayLab();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                    Con.Close();
                    clear();
                }
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (Key == 0)
            {
                MessageBox.Show("Please select a test to update.");
                return;
            }

            if (string.IsNullOrWhiteSpace(TestNameTb.Text) || string.IsNullOrWhiteSpace(TestCostTb.Text) || string.IsNullOrWhiteSpace(TestTypeTb.Text) || string.IsNullOrWhiteSpace(PatNameTb.Text) || string.IsNullOrWhiteSpace(DocTb.Text))
            {
                MessageBox.Show("Missing information.");
                return;
            }

            try
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE tblTest SET testName=@TN, testCost=@TC, testType=@TT, patientName=@PN, refDoc=@RD, testConduct=@TCO WHERE testNum=@TKey", Con);
                cmd.Parameters.AddWithValue("@TN", TestNameTb.Text);
                cmd.Parameters.AddWithValue("@TC", TestCostTb.Text);
                cmd.Parameters.AddWithValue("@TT", TestTypeTb.Text);
                cmd.Parameters.AddWithValue("@PN", PatNameTb.Text);
                cmd.Parameters.AddWithValue("@RD", DocTb.Text);
                cmd.Parameters.AddWithValue("@TCO", TestConCb.Value);
                cmd.Parameters.AddWithValue("@TKey", Key);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Test updated successfully!");
                DisplayLab();
                clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                Con.Close();
            }
        }

        private void TestGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            {
                if (e.RowIndex >= 0) // Ensure a valid row is clicked
                {
                    DataGridViewRow row = TestGV.Rows[e.RowIndex];

                    if (row.Cells[0].Value != null)
                    {
                        Key = Convert.ToInt32(row.Cells[0].Value); // Assign ID to Key
                        TestNameTb.Text = row.Cells[1].Value.ToString();  // testName
                        TestCostTb.Text = row.Cells[2].Value.ToString();  // testCost
                        TestTypeTb.Text = row.Cells[3].Value.ToString();  // testType
                        PatNameTb.Text = row.Cells[4].Value.ToString();   // patientName
                        DocTb.Text = row.Cells[5].Value.ToString();       // refDoc
                        TestConCb.Value = Convert.ToDateTime(row.Cells[6].Value); // TestConductedOn (if present)
                    }
                    else
                    {
                        Key = 0;
                    }
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
