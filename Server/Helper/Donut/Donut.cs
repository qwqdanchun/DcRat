using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Server.Helper.Donut.Structs;

namespace Server.Helper.Donut
{
    public class Donut
    {
        public static void Creat(string filepath)
        {
            DSConfig config = new Helper().InitStruct("DSConfig");
            string savepath = "";
            using (SaveFileDialog saveFileDialog1 = new SaveFileDialog())
            {
                saveFileDialog1.Filter = ".bin (*.bin)|*.bin";
                saveFileDialog1.InitialDirectory = Application.StartupPath;
                saveFileDialog1.OverwritePrompt = false;
                saveFileDialog1.FileName = "Client";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    savepath = saveFileDialog1.FileName;
                }
            }
            Helper.ParseArguments(filepath, savepath, ref config);
            Generator.Donut_Create(ref config);
            Marshal.FreeHGlobal(config.pic);
        }
    }
}
