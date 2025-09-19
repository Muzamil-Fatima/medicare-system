using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClinicMangSystem
{
    public partial class HelpCenter : BaseForm
    {
        public HelpCenter()
        {
            InitializeComponent();
        }

        public BaseForm MidParent { get; internal set; }

        private void HelpCenter_Load(object sender, EventArgs e)
        {
        }
        private void btnFeedback_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Button Clicked!"); // Debugging ke liye message

            try
            {
                string url = "https://forms.gle/zZMDvcxbyocxANDHA";
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });

                MessageBox.Show("Link Open Attempted!"); // Check karo yeh show ho raha hai ya nahi
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not open the link. Error: " + ex.Message);
            }
        }

    }

}