using System.Windows.Forms;

namespace Server.Helper
{
    public class RegistryTreeView : TreeView
    {
        public RegistryTreeView()
        {
            //Enable double buffering and ignore WM_ERASEBKGND to reduce flicker
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
        }
    }
}
