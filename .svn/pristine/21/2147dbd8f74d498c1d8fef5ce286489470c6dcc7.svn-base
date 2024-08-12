using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NCManagementSystem.Libraries.Controls.Panel
{
    [ToolboxBitmap(typeof(FlowLayoutPanel)), ToolboxItemFilter("System.Windows.Forms")]
    public partial class FwFlowLayoutPanel : FlowLayoutPanel
    {
        public FwFlowLayoutPanel()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();

            InitializeComponent();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams _CreateParams = base.CreateParams;
                _CreateParams.ExStyle = _CreateParams.ExStyle | NativeMethods.WS_EX_COMPOSITED;
                return _CreateParams;
            }
        }
    }
}
