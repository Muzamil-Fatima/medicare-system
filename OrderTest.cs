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
    public partial class OrderTest : BaseForm
    {
        public OrderTest()
        {
            InitializeComponent();
            DisplayOrd();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=DESKTOP-JLQCCR6;Initial Catalog=ClinicManagement; Integrated Security=true");
        private void DisplayOrd()
        {
            Con.Open();
            string Query = "select*from  tblOrder";
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
            TestCb.SelectedIndex = 0;
            PayCb.SelectedIndex = 0;
            DateTime selectedDateOrd = TestOrd.Value;
            DateTime selectedDateCon = TestCon.Value;
            docId.Text = "";
            patId.Text = "";
            Key = 0;
        }

        private void OrderTest_Load(object sender, EventArgs e)
        {

        }

        private void DelBtn_Click(object sender, EventArgs e)
        {
            if (Key == 0)
            {
                MessageBox.Show("Please select a record to delete.");
                return;
            }
            try
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM tblOrder WHERE orderId=@OKey", Con);
                cmd.Parameters.AddWithValue("@OKey", Key);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Selected Record Deleted");
                    DisplayOrd();
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
                DisplayOrd();
                clear();
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
                        TestCb.SelectedItem = row.Cells[5].Value.ToString();  // Test Status
                        PayCb.SelectedItem = row.Cells[6].Value.ToString();  // Payment Status
                        TestOrd.Value = Convert.ToDateTime(row.Cells[3].Value);  // Test Ordered On
                        TestCon.Value = Convert.ToDateTime(row.Cells[4].Value);  // Test Conducted On
                        docId.Text = row.Cells[1].Value.ToString();  // Doctor ID
                        patId.Text = row.Cells[2].Value.ToString();  // Patient ID
                    }

                    else
                    {
                        Key = 0;
                    }
                }
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {

            {
                if (Key == 0)
                {
                    MessageBox.Show("Please select a Test to update");
                    return;
                }

                if (TestCb.SelectedItem == null ||
                      PayCb.SelectedItem == null ||
             string.IsNullOrWhiteSpace(docId.Text) ||
             string.IsNullOrWhiteSpace(patId.Text) ||
                (TestOrd.Value == TestOrd.MinDate) ||
                TestCon.Value == TestCon.MinDate)
                {
                    MessageBox.Show("Missing Information", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Update tblOrder set  testStatus=@TS,paymentStatus=@PS,testOrd=@TO,testCon=@TC,docId=@DI,patId=@PI where orderId=@OKey", Con);
                    cmd.Parameters.AddWithValue("@TS", TestCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@PS", PayCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@TO", TestOrd.Value);
                    cmd.Parameters.AddWithValue("@TC", TestCon.Value);
                    cmd.Parameters.AddWithValue("@DI", docId.Text);
                    cmd.Parameters.AddWithValue("@PI", patId.Text);
                    cmd.Parameters.AddWithValue("@OKey", Key);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Information Updated");
                        DisplayOrd();
                    }
                    else
                    {
                        MessageBox.Show("Update Failed. Make sure the selected Test exists");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    Con.Close();
                    DisplayOrd();
                    clear();
                }
            }
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            if
(TestCb.SelectedIndex == -1 || PayCb.SelectedIndex == -1 || TestOrd.Value == DateTime.MinValue || TestCon.Value == DateTime.MinValue || docId.Text == "" || patId.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into tblOrder(testStatus,paymentStatus,testOrd,testCon,docId,patId) values(@TS,@PS,@TO,@TC,@DI,@PI)", Con);
                    cmd.Parameters.AddWithValue("@TS", TestCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@PS", PayCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@TO", TestOrd.Value);
                    cmd.Parameters.AddWithValue("@TC", TestCon.Value);
                    cmd.Parameters.AddWithValue("@DI", docId.Text);
                    cmd.Parameters.AddWithValue("@PI", patId.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Information added");
                    Con.Close();
                    DisplayOrd();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                    Con.Close();
                    clear();
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
