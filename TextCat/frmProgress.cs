using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TextCat
{
    public partial class frmProgress : Form
    {
        public event EventHandler FormDead;

        public frmProgress()
        {
            InitializeComponent();
        }

        private void frmProgress_Load(object sender, EventArgs e)
        {

        }

        private delegate void DelegateCloseMe();
        public void CloseMe()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DelegateCloseMe(CloseMe));
                return;
            }
            EventHandler handler = FormDead;
            if (handler != null) { handler(this, null); }
            this.Close();
        }
    }
}
