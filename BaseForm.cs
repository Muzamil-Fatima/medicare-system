using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClinicMangSystem
{
    public partial class BaseForm: Form
    {
        Home Home;
        Patients Patient;
        ViewAllPatient ViewAllPatient;
        Appointment Appointment;
        Doctors doctors;
        ViewAllDoctor ViewAllDoctor;
        AssignPatient assignpatient;
        Receptionists Receptionist;
        BillingSystem BillingSystem;
        ViewAllReceptionists ViewAllReceptionists;
        OrderTest OrderTest;
        Lab Lab;
        HelpCenter HelpCenter;
        Settings Settings;
        public BaseForm()
        {
            InitializeComponent();
        }

      

        bool menuExpand = false;
        private void menuTransition_Tick(object sender, EventArgs e)
        {
            if (menuExpand == false)
            {
                pnPatient.Height += 10;
                if(pnPatient.Height >= 250)
                {
                    menuTransition.Stop();
                    menuExpand = true;
                }
            }
            else
            {
                pnPatient.Height -= 10;
                if(pnPatient.Height <= 53)
                {
                    menuTransition.Stop();
                    menuExpand = false;
                }
            }
        }

        private void btnPatient_Click(object sender, EventArgs e)
        {
            InitializeSidebarHoverEffect(sidebar);
            menuTransition.Start();
        }
        private void BaseForm_Load(object sender, EventArgs e)
        {
            InitializeSidebarHoverEffect(this);
            // Adjust Left Panel (Keep same width but change height)
            sidebar.Height = this.Height - 55;

            // Adjust Header Panel (Expand width)
            pnHeader.Width = this.Width;
        }
        bool menuExpand1 = false;
        private void menuTransition1_Tick(object sender, EventArgs e)
        {
            if (menuExpand1 == false)
            {
                pnDoctor.Height += 10;
                if (pnDoctor.Height >= 250)
                {
                    menuTransition1.Stop();
                    menuExpand1 = true;
                }
            }
            else
            {
                pnDoctor.Height -= 10;
                if (pnDoctor.Height <= 53)
                {
                    menuTransition1.Stop();
                    menuExpand1 = false;
                }
            }
        }
        private void btnDoctor_Click(object sender, EventArgs e)
        {
            menuTransition1.Start();
        }
        bool menuExpand2 = false;
        private void menuTranstion2_Tick(object sender, EventArgs e)
        {
            if (menuExpand2 == false)
            {
                pnLaboratory.Height += 10;
                if (pnLaboratory.Height >= 250)
                {
                    menuTransition2.Stop();
                    menuExpand2 = true;
                }
            }
            else
            {
                pnLaboratory.Height -= 10;
                if (pnLaboratory.Height <= 53)
                {
                    menuTransition2.Stop();
                    menuExpand2 = false;
                }
            }
        }
        private void btnLaboratory_Click(object sender, EventArgs e)
        {
            menuTransition2.Start();
        }
        bool menuExpand3 = false;
        private void menuTransition3_Tick(object sender, EventArgs e)
        {
            if (menuExpand3 == false)
            {
                pnReception.Height += 10;
                if (pnReception.Height >= 250)
                {
                    menuTransition3.Stop();
                    menuExpand3 = true;
                }
            }
            else
            {
                pnReception.Height -= 10;
                if (pnReception.Height <= 53)
                {
                    menuTransition3.Stop();
                    menuExpand3 = false;
                }
            }
        }
        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void btnReceptionists_Click(object sender, EventArgs e)
        {
            menuTransition3.Start();
        }

        bool sidebarExpand = true;
        private void sidebarTransition_Tick(object sender, EventArgs e)
        {
            int speed = 15;
            if (sidebarExpand)
            {
                sidebar.Width -= speed;
                if (sidebar.Width <= 67)
                {
                    sidebarExpand = false;
                    sidebarTransition.Stop();

                    panel5.Width = sidebar.Width;
                    pnPatient.Width = sidebar.Width;
                    pnDoctor.Width = sidebar.Width;
                    pnLaboratory.Width = sidebar.Width;
                    pnReception.Width = sidebar.Width;
                    pnSetting.Width = sidebar.Width;
                    pnHelpCenter.Width = sidebar.Width;
                    //pnApprance.Width = sidebar.Width;
                    pnLogout.Width = sidebar.Width;
                }
            }
            else
            {
                sidebar.Width += speed;
                if (sidebar.Width >= 248)
                {
                    sidebarExpand = true;
                    sidebarTransition.Stop();

                    panel5.Width = sidebar.Width;
                    pnPatient.Width = sidebar.Width;
                    pnDoctor.Width = sidebar.Width;
                    pnLaboratory.Width = sidebar.Width;
                    pnReception.Width = sidebar.Width;
                    pnSetting.Width = sidebar.Width;
                    pnHelpCenter.Width = sidebar.Width;
                    //pnApprance.Width = sidebar.Width;
                    pnLogout.Width = sidebar.Width;

                }
            }
        }
        private void btnHam_Click(object sender, EventArgs e)
        {
            sidebarTransition.Start();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            if(Home == null)
            {
                Home = new Home();
                Home.FormClosed += Home_FormClosed;
                Home.MidParent = this;
                Home.Show();
            }
            else
            {
                Home.Activate();
            }  
        }
        private void Home_FormClosed(object sender, FormClosedEventArgs e)
        {
            Home = null;
        }

        private void btnAddNewPatient_Click(object sender, EventArgs e)
        {
            if (Patient == null)
            {
                Patient = new Patients();
                Patient.FormClosed += Patients_FormClosed;
                Patient.MidParent = this;
                Patient.Show();
            }
            else
            {
                Patient.Activate();
            }
        }

        private void Patients_FormClosed(object sender, FormClosedEventArgs e)
        {
            Patient = null;
        }
        private void btnViewAllPatient_Click(object sender, EventArgs e)
        {

            if (ViewAllPatient == null)
            {
                ViewAllPatient = new ViewAllPatient();
                ViewAllPatient.FormClosed += ViewAllPatient_FormClosed;
                ViewAllPatient.MidParent = this;
                ViewAllPatient.Show();
            }
            else
            {
                ViewAllPatient.Activate();
            }
        }

        private void ViewAllPatient_FormClosed(object sender, FormClosedEventArgs e)
        {
            ViewAllPatient = null;
        }

        private void btnAddNewDoctor_Click(object sender, EventArgs e)
        {

            if (doctors == null)
            {
                doctors = new Doctors();
                doctors.FormClosed += doctors_FormClosed;
                doctors.MidParent = this;
                doctors.Show();
            }
            else
            {
                doctors.Activate();
            }
        }

        private void doctors_FormClosed(object sender, FormClosedEventArgs e)
        {
            doctors = null;
        }

        private void btnAssignPatient_Click(object sender, EventArgs e)
        {
            if (assignpatient == null)
            {
                assignpatient = new AssignPatient();
                assignpatient.FormClosed += assignpatient_FormClosed;
                assignpatient.MidParent = this;
                assignpatient.Show();
            }
            else
            {
                assignpatient.Activate();
            }
        }
        private void assignpatient_FormClosed(object sender, FormClosedEventArgs e)
        {
            assignpatient = null;
        }

        private void btnAllDoctors_Click(object sender, EventArgs e)
        {
            if (ViewAllDoctor == null)
            {
                ViewAllDoctor = new ViewAllDoctor();
                ViewAllDoctor.FormClosed += ViewAllDoctor_FormClosed;
                ViewAllDoctor.MidParent = this;
                ViewAllDoctor.Show();
            }
            else
            {
                ViewAllDoctor.Activate();
            }
        }
        private void ViewAllDoctor_FormClosed(object sender, FormClosedEventArgs e)
        {
            ViewAllDoctor = null;
        }

        private void btnNewReception_Click(object sender, EventArgs e)
        {
            menuTransition1.Start();
        }

        private void btnViewAllReceptionist_Click(object sender, EventArgs e)
        {
           if(ViewAllReceptionists == null)
            {
                ViewAllReceptionists = new ViewAllReceptionists();
                ViewAllReceptionists.FormClosed += ViewAllReceptionists_FormClosed;
                ViewAllReceptionists.MidParent = this;
                ViewAllReceptionists.Show();
            }
            else
            {
                ViewAllReceptionists.Activate();
            }
        }

        private void ViewAllReceptionists_FormClosed(object sender, FormClosedEventArgs e)
        {
            ViewAllReceptionists = null;
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            // Login Form ko Open Karo
            Form1 loginForm = new Form1();
            loginForm.Show();

            // Saare Open Forms Close Karo, Sirf Login Form Chor Kar
            foreach (Form form in Application.OpenForms.Cast<Form>().ToList())
            {
                if (form.Name != "Form1") // Login Form ka naam check karo
                {
                    form.Close();
                }
            }
        }
        private void btnAppointments_Click(object sender, EventArgs e)
        {
            if (Appointment == null)
            {
                Appointment = new Appointment();
                Appointment.FormClosed += Appointment_FormClosed;
                Appointment.MidParent = this;
                Appointment.Show();
            }
            else
            {
                Appointment.Activate();
            }
        }
        private void Appointment_FormClosed(object sender, FormClosedEventArgs e)
        {
            Appointment = null;
        }
        private void btnAddNewReception_Click(object sender, EventArgs e)
        {
            if (Receptionist == null)
            {
                Receptionist = new Receptionists();
                Receptionist.FormClosed += Receptionist_FormClosed;
                Receptionist.MidParent = this;
                Receptionist.Show();
            }
            else
            {
                Receptionist.Activate();
            }
        }

        private void Receptionist_FormClosed(object sender, FormClosedEventArgs e)
        {
            Receptionist = null;
        }

        private void sidebar_MouseEnter(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                btn.BackColor = Color.BlueViolet;  
                btn.ForeColor = Color.White; 
            }
        }

        private void sidebar_MouseLeave(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                btn.BackColor = Color.FromArgb(42, 62, 82); 
                btn.ForeColor = Color.White; 
            }
        }

        private void InitializeSidebarHoverEffect(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is Button btn)
                {
                    btn.MouseEnter += sidebar_MouseEnter;
                    btn.MouseLeave += sidebar_MouseLeave;
                    btn.FlatStyle = FlatStyle.Flat; 
                }
                else if (ctrl is Panel panel)
                {
                    InitializeSidebarHoverEffect(panel); 
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Exit Confirmation",
                                                  MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void btnBillingPayment_Click(object sender, EventArgs e)
        {
            if (BillingSystem == null)
            {
                BillingSystem = new BillingSystem();
                BillingSystem.FormClosed += Receptionist_FormClosed;
                BillingSystem.MidParent = this;
                BillingSystem.Show();
            }
            else
            {
                Receptionist.Activate();
            }
        }

        private void btnAppearance_Click(object sender, EventArgs e)
        {
           
        }

        private void btnOrderTest_Click(object sender, EventArgs e)
        {
            if (OrderTest == null)
            {
                OrderTest = new OrderTest();
                OrderTest.FormClosed += OrderTest_FormClosed;
                OrderTest.MidParent = this;
                OrderTest.Show();
            }
            else
            {
                OrderTest.Activate();
            }
        }

        private void OrderTest_FormClosed(object sender, FormClosedEventArgs e)
        {
            OrderTest = null;
        }

        private void btnAddLab_Click(object sender, EventArgs e)
        {
            if (Lab == null)
            {
                Lab = new Lab();
                Lab.FormClosed += Lab_FormClosed;
                Lab.MidParent = this;
                Lab.Show();
            }
            else
            {
                Lab.Activate();
            }
        }

        private void Lab_FormClosed(object sender, FormClosedEventArgs e)
        {
            Lab = null;
        }
        
        private void btnHelpCenter_Click(object sender, EventArgs e)
        {
            if(HelpCenter == null)
            {
                HelpCenter = new HelpCenter();
                HelpCenter.FormClosed += HelpCenter_FormClosed;
                HelpCenter.MidParent = this;
                HelpCenter.Show();
            }
            else
            {
                HelpCenter.Activate();
            }
        }
        private void HelpCenter_FormClosed(object sender, FormClosedEventArgs e)
        {
            HelpCenter = null;
        }
        private void btnSetting_Click(object sender, EventArgs e)
        {
            if(Settings == null)
            {
                Settings = new Settings();
                Settings.FormClosed += Settings_FormClosed;
                Settings.MidParent = this;
                Settings.Show();
            }
            else
            {
                Settings.Activate();
            }
        }
        private void Settings_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings = null;
        }
    }
}