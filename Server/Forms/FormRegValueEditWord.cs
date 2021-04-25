using Microsoft.Win32;
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
    public partial class FormRegValueEditWord : Form
    {
        private readonly RegValueData _value;

        private const string DWORD_WARNING = "The decimal value entered is greater than the maximum value of a DWORD (32-bit number). Should the value be truncated in order to continue?";
        private const string QWORD_WARNING = "The decimal value entered is greater than the maximum value of a QWORD (64-bit number). Should the value be truncated in order to continue?";

        public FormRegValueEditWord(RegValueData value)
        {
            _value = value;

            InitializeComponent();

            this.valueNameTxtBox.Text = value.Name;

            if (value.Kind == RegistryValueKind.DWord)
            {
                this.Text = "Edit DWORD (32-bit) Value";
                this.valueDataTxtBox.Type = WordTextBox.WordType.DWORD;
                this.valueDataTxtBox.Text = Helper.ByteConverter.ToUInt32(value.Data).ToString("x");
            }
            else
            {
                this.Text = "Edit QWORD (64-bit) Value";
                this.valueDataTxtBox.Type = WordTextBox.WordType.QWORD;
                this.valueDataTxtBox.Text = Helper.ByteConverter.ToUInt64(value.Data).ToString("x");
            }
        }

        private void radioHex_CheckboxChanged(object sender, EventArgs e)
        {
            if (valueDataTxtBox.IsHexNumber == radioHexa.Checked)
                return;

            if (valueDataTxtBox.IsConversionValid() || IsOverridePossible())
                valueDataTxtBox.IsHexNumber = radioHexa.Checked;
            else
                radioDecimal.Checked = true;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (valueDataTxtBox.IsConversionValid() || IsOverridePossible())
            {
                _value.Data = _value.Kind == RegistryValueKind.DWord
                    ? Helper.ByteConverter.GetBytes(valueDataTxtBox.UIntValue)
                    : Helper.ByteConverter.GetBytes(valueDataTxtBox.ULongValue);
                this.Tag = _value;
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.DialogResult = DialogResult.None;
            }

            this.Close();
        }

        private DialogResult ShowWarning(string msg, string caption)
        {
            return MessageBox.Show(msg, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        }

        private bool IsOverridePossible()
        {
            string message = _value.Kind == RegistryValueKind.DWord ? DWORD_WARNING : QWORD_WARNING;

            return ShowWarning(message, "Overflow") == DialogResult.Yes;
        }
    }
}
