using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NCManagementSystem.Libraries.Controls.Panel
{
    [ToolboxBitmap(typeof(System.Windows.Forms.TableLayoutPanel)), ToolboxItemFilter("System.Windows.Forms")]
    public partial class FwTableLayoutPanel : System.Windows.Forms.TableLayoutPanel
    {
        #region [ Constructor ]
        public FwTableLayoutPanel()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();

            InitializeComponent();
        }
        #endregion

        #region [ Override Events / Events / Methods ]
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams _CreateParams = base.CreateParams;
                _CreateParams.ExStyle = _CreateParams.ExStyle | NativeMethods.WS_EX_COMPOSITED;
                return _CreateParams;
            }
        }
        #endregion
    }
}
