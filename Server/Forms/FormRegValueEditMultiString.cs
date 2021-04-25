using Server.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Server.Helper.RegistrySeeker;

namespace Server.Forms
{
    public partial class FormRegValueEditMultiString : Form
    {
        private readonly RegValueData _value;

        public FormRegValueEditMultiString(RegValueData value)
        {
            _value = value;

            InitializeComponent();

            this.valueNameTxtBox.Text = value.Name;
            this.valueDataTxtBox.Text = string.Join("\r\n", Helper.ByteConverter.ToStringArray(value.Data));
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            _value.Data = Helper.ByteConverter.GetBytes(valueDataTxtBox.Text.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
            this.Tag = _value;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
