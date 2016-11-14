using System;
using System.Windows.Forms;

namespace SharedUtilitys.Exceptions
{
    public partial class ErrorMessageForm : Form
    {
        public bool IsContinue { get; private set; }

        public ErrorMessageForm()
        {
            InitializeComponent();
        }

        public string ErrorDetail { get; set; }

        public bool IsLogWritten { get; set; }

        private void ErrorMessageForm_Load(object sender, EventArgs e)
        {
            btnContinue.Visible = false;
            txtDetail.Visible = false;
            txtDetail.Text = ErrorDetail;

            Height = 215;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnShowDetail_Click(object sender, EventArgs e)
        {
            if (txtDetail.Visible == false)
            {
                txtDetail.Visible = true;
                Height = 415;
            }
            else
            {
                txtDetail.Visible = false;
                Height = 215;
            }
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            IsContinue = true;
            Close();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.F2 && e.Shift == true)
            {
                btnContinue.Visible = true;
            }
        }
    }
}
