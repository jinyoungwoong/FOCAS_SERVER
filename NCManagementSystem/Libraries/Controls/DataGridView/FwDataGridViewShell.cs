using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using NCManagementSystem.Components.Helpers;

namespace NCManagementSystem.Libraries.Controls.DataGridView
{
    [ToolboxBitmap(typeof(System.Windows.Forms.DataGridView)), ToolboxItemFilter("System.Windows.Forms")]
    public partial class FwDataGridViewShell : UserControl
    {
        #region [ Constructor ]
        public FwDataGridViewShell()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.EnableNotifyMessage, true);
            UpdateStyles();

            InitializeComponent();

            Initialize();
        }
        #endregion

        #region [ Member Variables / Fields / Properties ]
        public event EventHandler OnSkipToPreviousClick;
        public event EventHandler OnToPreviousClick;
        public event EventHandler OnToNextClick;
        public event EventHandler OnSkipToNextClick;
        public event EventHandler OnRowsPerPageKeyEnter;
        public event EventHandler OnRowsPerPageKeyLeave;

        private bool m_IsKeyDown = false;
        [Browsable(false)]
        public bool IsPaging { get; set; } = false;
        [Browsable(false)]
        public int Page { get; set; } = 1;
        private int m_iPageMax = 0;
        private bool m_IsRowsPerPageChanged = false;

        private int m_iPageSize = 15;
        [Browsable(false)]
        public int PageSize
        {
            get { return m_iPageSize; }
            set
            {
                m_iPageSize = value;
                txtRowsPerPage.Text = m_iPageSize.ToString();
            }
        }

        [Browsable(false)]
        //[Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All)]
        public FwDataGridView GridView
        {
            get { return dgvGridView; }
            set
            {
                dgvGridView = value;
                dgvGridView.Invalidate();
            }
        }

        private bool m_IsBorder = false;
        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All)]
        [DefaultValue(false)]
        public bool IsBorder
        {
            get { return m_IsBorder; }
            set
            {
                m_IsBorder = value;
                if (m_IsBorder)
                {
                    Padding = new Padding(m_iBorderThickness);
                    BackColor = m_BorderColor;
                }
                else
                {
                    Padding = new Padding(0);
                }
                Invalidate();
            }
        }

        private Color m_BorderColor = SystemColors.WindowFrame;
        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.Repaint)]
        [DefaultValue(typeof(Color), "WindowFrame")]
        public Color BorderColor
        {
            get { return m_BorderColor; }
            set
            {
                m_BorderColor = value;
                BackColor = m_BorderColor;
                Invalidate();
            }
        }

        private int m_iBorderThickness = 1;
        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.Repaint)]
        [NotifyParentProperty(true)]
        [DefaultValue(1)]
        public int BorderThickness
        {
            get { return m_iBorderThickness; }
            set
            {
                m_iBorderThickness = value;
                if (m_IsBorder)
                {
                    Padding = new Padding(m_iBorderThickness);
                    BackColor = m_BorderColor;
                }
                else
                {
                    Padding = new Padding(0);
                }
                Invalidate();
            }
        }

        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.All)]
        public TopBarProperties TopBarProperties { get; set; }

        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.All)]
        public BottomBarProperties BottomBarProperties { get; set; }

        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.All)]
        public ScrollBarProperties ScrollBarProperties { get; set; }

        [Category(ControlConstsDefiner.PropertyWindow.Category.Extensions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.All)]
        public GridViewProperties GridViewProperties { get; set; }
        #endregion

        #region [ Override Events / Events / Methods ]
        private void Initialize()
        {
            try
            {
                TopBarProperties = new TopBarProperties();
                TopBarProperties.PropertyChanged += TopBarProperties_PropertyChanged;

                BottomBarProperties = new BottomBarProperties();
                BottomBarProperties.PropertyChanged += BottomBarProperties_PropertyChanged;

                ScrollBarProperties = new ScrollBarProperties();
                ScrollBarProperties.PropertyChanged += ScrollBarProperties_PropertyChanged;

                GridViewProperties = new GridViewProperties();
                GridViewProperties.PropertyChanged += GridViewProperties_PropertyChanged;

                dgvGridView.MouseWheel += dgvGridView_MouseWheel;

                pnlMain.Visible = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void TopBarProperties_PropertyChanged(object sender, ExtensionsPropertyEventArgs e)
        {
            try
            {
                string _sPropertyName = e.PropertyName;
                if (Enum.TryParse(_sPropertyName, out DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties _PropertyName))
                {
                    switch (_PropertyName)
                    {
                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.IsTopBar:
                            {
                                tlpTopBar.Visible = TopBarProperties.IsTopBar;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.TopBarBackColor:
                            {
                                tlpTopBar.BackColor = TopBarProperties.BackColor;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.TopBarHeight:
                            {
                                tlpTopBar.Height = TopBarProperties.Height;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.TopBarMargin:
                            {
                                tlpTopBar.Margin = TopBarProperties.Margin;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.TitleIcon:
                            {
                                if (TopBarProperties.TitleIcon != null)
                                {
                                    pnlTitleIcon.Width = 40;
                                }
                                else
                                {
                                    pnlTitleIcon.Width = 5;
                                }
                                pnlTitleIcon.BackgroundImage = TopBarProperties.TitleIcon;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.Title:
                            {
                                lblTitle.Text = TopBarProperties.Title;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.TitleFont:
                            {
                                lblTitle.Font = TopBarProperties.TitleFont;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.TitleForeColor:
                            {
                                lblTitle.ForeColor = TopBarProperties.TitleForeColor;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.IsPageController:
                            {
                                tlpPageController.Visible = TopBarProperties.IsPageController;
                                if (!TopBarProperties.IsPageController && TopBarProperties.IsPageControllerStatus)
                                {
                                    TopBarProperties.IsPageControllerStatus = false;
                                }
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.IsPageControllerStatus:
                            {
                                lblPageControllerStatus.Visible = TopBarProperties.IsPageControllerStatus;
                                if (TopBarProperties.IsPageControllerStatus)
                                {
                                    tlpPageController.Width = (300 + TopBarProperties.PageControllerContainerExtraWidth);
                                }
                                else
                                {
                                    tlpPageController.Width = 164;
                                }
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.PageControllerContainerExtraWidth:
                            {
                                if (TopBarProperties.IsPageControllerStatus)
                                {
                                    tlpPageController.Width = (300 + TopBarProperties.PageControllerContainerExtraWidth);
                                }
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.PageControllerStatusFormat:
                            {
                                lblPageControllerStatus.Text = string.Format(TopBarProperties.PageControllerStatusFormat, "0", "0");
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.PageControllerStatusFont:
                            {
                                lblPageControllerStatus.Font = TopBarProperties.PageControllerStatusFont;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.PageControllerStatusForeColor:
                            {
                                lblPageControllerStatus.ForeColor = TopBarProperties.PageControllerStatusForeColor;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.PageControllerWidth:
                            {
                                btnToPrevious.Width = btnSkipToPrevious.Width = TopBarProperties.PageControllerWidth;
                                btnToNext.Width = btnSkipToNext.Width = TopBarProperties.PageControllerWidth;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.PageControllerBackColor:
                            {
                                btnToPrevious.BackColor = btnSkipToPrevious.BackColor = TopBarProperties.PageControllerBackColor;
                                btnToNext.BackColor = btnSkipToNext.BackColor = TopBarProperties.PageControllerBackColor;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.PageControllerMouseDownBackColor:
                            {
                                btnToPrevious.FlatAppearance.MouseDownBackColor = TopBarProperties.PageControllerMouseDownBackColor;
                                btnSkipToPrevious.FlatAppearance.MouseDownBackColor = TopBarProperties.PageControllerMouseDownBackColor;
                                btnToNext.FlatAppearance.MouseDownBackColor = TopBarProperties.PageControllerMouseDownBackColor; 
                                btnSkipToNext.FlatAppearance.MouseDownBackColor = TopBarProperties.PageControllerMouseDownBackColor; 
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.PageControllerMouseOverBackColor:
                            {
                                btnToPrevious.FlatAppearance.MouseOverBackColor = TopBarProperties.PageControllerMouseOverBackColor;
                                btnSkipToPrevious.FlatAppearance.MouseOverBackColor = TopBarProperties.PageControllerMouseOverBackColor;
                                btnToNext.FlatAppearance.MouseOverBackColor = TopBarProperties.PageControllerMouseOverBackColor;
                                btnSkipToNext.FlatAppearance.MouseOverBackColor = TopBarProperties.PageControllerMouseOverBackColor;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.SkipToPreviousEnableIcon:
                            {
                                if (btnSkipToPrevious.Enabled)
                                {
                                    btnSkipToPrevious.Image = TopBarProperties.SkipToPreviousEnableIcon;
                                }
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.SkipToPreviousDisableIcon:
                            {
                                if (!btnSkipToPrevious.Enabled)
                                {
                                    btnSkipToPrevious.Image = TopBarProperties.SkipToPreviousDisableIcon;
                                }
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.ToPreviousEnableIcon:
                            {
                                if (btnToPrevious.Enabled)
                                {
                                    btnToPrevious.Image = TopBarProperties.ToPreviousEnableIcon;
                                }
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.ToPreviousDisableIcon:
                            {
                                if (!btnToPrevious.Enabled)
                                {
                                    btnToPrevious.Image = TopBarProperties.ToPreviousDisableIcon;
                                }
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.ToNextEnableIcon:
                            {
                                if (btnToNext.Enabled)
                                {
                                    btnToNext.Image = TopBarProperties.ToNextEnableIcon;
                                }
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.ToNextDisableIcon:
                            {
                                if (!btnToNext.Enabled)
                                {
                                    btnToNext.Image = TopBarProperties.ToNextDisableIcon;
                                }
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.SkipToNextEnableIcon:
                            {
                                if (btnSkipToNext.Enabled)
                                {
                                    btnSkipToNext.Image = TopBarProperties.SkipToNextEnableIcon;
                                }
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.SkipToNextDisableIcon:
                            {
                                if (!btnSkipToNext.Enabled)
                                {
                                    btnSkipToNext.Image = TopBarProperties.SkipToNextDisableIcon;
                                }
                            }
                            break;

                        default:
                            break;
                    }

                    Invalidate();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BottomBarProperties_PropertyChanged(object sender, ExtensionsPropertyEventArgs e)
        {
            try
            {
                string _sPropertyName = e.PropertyName;
                if (Enum.TryParse(_sPropertyName, out DataGridViewConstsDefiner.ExtensionsPropertyNames.BottomBarProperties _PropertyName))
                {
                    switch (_PropertyName)
                    {
                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.BottomBarProperties.IsBottomBar:
                            {
                                tlpBottomBar.Visible = BottomBarProperties.IsBottomBar;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.BottomBarProperties.BottomBarBackColor:
                            {
                                tlpBottomBar.BackColor = BottomBarProperties.BackColor;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.BottomBarProperties.BottomBarHeight:
                            {
                                tlpBottomBar.Height = BottomBarProperties.Height;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.BottomBarProperties.BottomBarMargin:
                            {
                                tlpBottomBar.Margin = BottomBarProperties.Margin;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.BottomBarProperties.PageStatusShortFormat:
                            {
                                lblPageStatus.Text = string.Format(BottomBarProperties.PageStatusShortFormat, "0");
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.BottomBarProperties.PageStatusLongFormat:
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.BottomBarProperties.PageStatusFont:
                            {
                                lblPageStatus.Font = BottomBarProperties.PageStatusFont;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.BottomBarProperties.PageStatusForeColor:
                            {
                                lblPageStatus.ForeColor = BottomBarProperties.PageStatusForeColor;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.BottomBarProperties.IsRowsPerPage:
                            {
                                pnlOuterRowsPerPage.Visible = BottomBarProperties.IsRowsPerPage;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.BottomBarProperties.RowsPerPageBackColor:
                            {
                                pnlRowsPerPage.BackColor = BottomBarProperties.RowsPerPageBackColor;
                                txtRowsPerPage.BackColor = BottomBarProperties.RowsPerPageBackColor;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.BottomBarProperties.RowsPerPageWidth:
                            {
                                pnlOuterRowsPerPage.Width = BottomBarProperties.RowsPerPageWidth;
                            }
                            break;

                        default:
                            break;
                    }

                    Invalidate();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ScrollBarProperties_PropertyChanged(object sender, ExtensionsPropertyEventArgs e)
        {
            try
            {
                string _sPropertyName = e.PropertyName;
                if (Enum.TryParse(_sPropertyName, out DataGridViewConstsDefiner.ExtensionsPropertyNames.ScrollBarProperties _PropertyName))
                {
                    switch (_PropertyName)
                    {
                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.ScrollBarProperties.VScrollBarControllerWidth:
                            {
                                tlpVScroll.Width = pnlScrollNC.Width = ScrollBarProperties.VScrollBarControllerWidth;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.ScrollBarProperties.VScrollBarControllerHeight:
                            {
                                btnToUp.Height = btnToDown.Height = ScrollBarProperties.VScrollBarControllerHeight;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.ScrollBarProperties.HScrollBarControllerWidth:
                            {
                                btnToForward.Width = btnToBackward.Width = ScrollBarProperties.HScrollBarControllerWidth;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.ScrollBarProperties.HScrollBarControllerHeight:
                            {
                                tlpHScroll.Height = pnlScrollNC.Height = ScrollBarProperties.HScrollBarControllerHeight;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.ScrollBarProperties.ScrollControllerBackColor:
                            {
                                btnToUp.BackColor = btnToDown.BackColor = ScrollBarProperties.ScrollControllerBackColor;
                                btnToForward.BackColor = btnToBackward.BackColor = ScrollBarProperties.ScrollControllerBackColor;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.ScrollBarProperties.ScrollControllerMouseDownBackColor:
                            {
                                btnToUp.FlatAppearance.MouseDownBackColor = ScrollBarProperties.ScrollControllerMouseDownBackColor;
                                btnToDown.FlatAppearance.MouseDownBackColor = ScrollBarProperties.ScrollControllerMouseDownBackColor;
                                btnToForward.FlatAppearance.MouseDownBackColor = ScrollBarProperties.ScrollControllerMouseDownBackColor;
                                btnToBackward.FlatAppearance.MouseDownBackColor = ScrollBarProperties.ScrollControllerMouseDownBackColor;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.ScrollBarProperties.ScrollControllerMouseOverBackColor:
                            {
                                btnToUp.FlatAppearance.MouseOverBackColor = ScrollBarProperties.ScrollControllerMouseOverBackColor;
                                btnToDown.FlatAppearance.MouseOverBackColor = ScrollBarProperties.ScrollControllerMouseOverBackColor;
                                btnToForward.FlatAppearance.MouseOverBackColor = ScrollBarProperties.ScrollControllerMouseOverBackColor;
                                btnToBackward.FlatAppearance.MouseOverBackColor = ScrollBarProperties.ScrollControllerMouseOverBackColor;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.ScrollBarProperties.ToUpIcon:
                            {
                                btnToUp.Image = ScrollBarProperties.ToUpIcon;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.ScrollBarProperties.ToDownIcon:
                            {
                                btnToDown.Image = ScrollBarProperties.ToDownIcon;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.ScrollBarProperties.ToForwardIcon:
                            {
                                btnToForward.Image = ScrollBarProperties.ToForwardIcon;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.ScrollBarProperties.ToBackwardIcon:
                            {
                                btnToBackward.Image = ScrollBarProperties.ToBackwardIcon;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.ScrollBarProperties.ScrollBars:
                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.ScrollBarProperties.SmallChangePercentageToLargeChange:
                        default:
                            break;
                    }

                    Invalidate();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GridViewProperties_PropertyChanged(object sender, ExtensionsPropertyEventArgs e)
        {
            try
            {
                string _sPropertyName = e.PropertyName;
                if (Enum.TryParse(_sPropertyName, out DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties _PropertyName))
                {
                    switch (_PropertyName)
                    {
                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.GridViewBackColor:
                            {
                                dgvGridView.BackgroundColor = GridViewProperties.BackColor;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.GridViewRowHeight:
                            {
                                dgvGridView.RowHeight = GridViewProperties.RowHeight;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.GridViewIsMultiSelect:
                            {
                                dgvGridView.MultiSelect = GridViewProperties.IsMultiSelect;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.GridViewIsAutoSizeColumn:
                            {
                                dgvGridView.IsAutoSizeColumn = GridViewProperties.IsAutoSizeColumn;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.GridViewCellBorderStyle:
                            {
                                dgvGridView.CellBorderStyle = GridViewProperties.CellBorderStyle;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.GridViewGridColor:
                            {
                                dgvGridView.GridColor = GridViewProperties.GridColor;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.GridViewIsColumnHeadersVisible:
                            {
                                dgvGridView.ColumnHeadersVisible = GridViewProperties.IsColumnHeadersVisible;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.GridViewColumnHeadersHeight:
                            {
                                dgvGridView.ColumnHeadersHeight = GridViewProperties.ColumnHeadersHeight;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.GridViewIsRowHeadersVisible:
                            {
                                dgvGridView.RowHeadersVisible = GridViewProperties.IsRowHeadersVisible;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.GridViewRowHeadersWidth:
                            {
                                dgvGridView.RowHeadersWidth = GridViewProperties.RowHeadersWidth;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.HeaderFont:
                            {
                                dgvGridView.HeaderProperties.Font = GridViewProperties.HeaderProperties.Font;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.IsVisibleSelectedRowNumber:
                            {
                                dgvGridView.HeaderProperties.IsVisibleSelectedRowNumber = GridViewProperties.HeaderProperties.IsVisibleSelectedRowNumber;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.HeaderBorderColor:
                            {
                                dgvGridView.HeaderProperties.BorderColor = GridViewProperties.HeaderProperties.BorderColor;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.ColumnFirstBackColor:
                            {
                                dgvGridView.HeaderProperties.ColumnFirstBackColor = GridViewProperties.HeaderProperties.ColumnFirstBackColor;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.ColumnLastBackColor:
                            {
                                dgvGridView.HeaderProperties.ColumnLastBackColor = GridViewProperties.HeaderProperties.ColumnLastBackColor;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.ColumnForeColor:
                            {
                                dgvGridView.HeaderProperties.ColumnForeColor = GridViewProperties.HeaderProperties.ColumnForeColor;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.RowFirstBackColor:
                            {
                                dgvGridView.HeaderProperties.RowFirstBackColor = GridViewProperties.HeaderProperties.RowFirstBackColor;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.RowLastBackColor:
                            {
                                dgvGridView.HeaderProperties.RowLastBackColor = GridViewProperties.HeaderProperties.RowLastBackColor;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.RowForeColor:
                            {
                                dgvGridView.HeaderProperties.RowForeColor = GridViewProperties.HeaderProperties.RowForeColor;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.RowSelectionFirstBackColor:
                            {
                                dgvGridView.HeaderProperties.RowSelectionFirstBackColor = GridViewProperties.HeaderProperties.RowSelectionFirstBackColor;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.RowSelectionLastBackColor:
                            {
                                dgvGridView.HeaderProperties.RowSelectionLastBackColor = GridViewProperties.HeaderProperties.RowSelectionLastBackColor;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.RowSelectionForeColor:
                            {
                                dgvGridView.HeaderProperties.RowSelectionForeColor = GridViewProperties.HeaderProperties.RowSelectionForeColor;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.NCFirstBackColor:
                            {
                                dgvGridView.HeaderProperties.NCFirstBackColor = GridViewProperties.HeaderProperties.NCFirstBackColor;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.NCLastBackColor:
                            {
                                dgvGridView.HeaderProperties.NCLastBackColor = GridViewProperties.HeaderProperties.NCLastBackColor;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.GridViewCellBackColor:
                            {
                                dgvGridView.DefaultCellStyle.BackColor = GridViewProperties.CellBackColor;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.GridViewCellForeColor:
                            {
                                dgvGridView.DefaultCellStyle.ForeColor = GridViewProperties.CellForeColor;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.CellFont:
                            {
                                dgvGridView.CellProperties.Font = GridViewProperties.CellProperties.Font;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.IsCheckBoxCellInSquares:
                            {
                                dgvGridView.CellProperties.IsCheckBoxCellInSquares = GridViewProperties.CellProperties.IsCheckBoxCellInSquares;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.CellSelectionBackColor:
                            {
                                dgvGridView.CellProperties.SelectionBackColor = GridViewProperties.CellProperties.SelectionBackColor;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.CellSelectionForeColor:
                            {
                                dgvGridView.CellProperties.SelectionForeColor = GridViewProperties.CellProperties.SelectionForeColor;
                            }
                            break;

                        case DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.GridViewAlternatingRowsBackColor:
                            {
                                dgvGridView.AlternatingRowsDefaultCellStyle.BackColor = GridViewProperties.AlternatingRowsBackColor;
                            }
                            break;

                        default:
                            break;
                    }

                    Invalidate();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            try
            {
                base.OnSizeChanged(e);

                if (!DesignMode && IsHandleCreated)
                {
                    SetVScrollBar();
                    if (dgvGridView.Rows.Count > 0 && dgvGridView.Rows.Count < vscVScrollBar.Value)
                    {
                        dgvGridView.FirstDisplayedScrollingRowIndex = vscVScrollBar.Value;
                    }

                    if (dgvGridView.ColumnCount > 0)
                    {
                        SetHScrollBar();
                        dgvGridView.HorizontalScrollingOffset = hscHScrollBar.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            try
            {
                base.OnVisibleChanged(e);

                if (!Visible)
                {
                    return;
                }

                SetVScrollBar();
                if (dgvGridView.Rows.Count > 0 && dgvGridView.Rows.Count < vscVScrollBar.Value)
                {
                    dgvGridView.FirstDisplayedScrollingRowIndex = vscVScrollBar.Value;
                }

                if (dgvGridView.ColumnCount > 0)
                {
                    SetHScrollBar();
                    dgvGridView.HorizontalScrollingOffset = hscHScrollBar.Value;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetDataSource(DataTable dataTable)
        {
            try
            {
                using (SuspendHelper.BeginSuspend(this))
                {
                    dgvGridView.SetDataSource(dataTable);
                    SetVScrollBar();
                    SetHScrollBar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ClearDataSource()
        {
            try
            {
                using (SuspendHelper.BeginSuspend(this))
                {
                    dgvGridView.ClearDataSource();
                    SetVScrollBar();
                    SetHScrollBar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InitializeScrollBars()
        {
            try
            {
                using (SuspendHelper.BeginSuspend(this))
                {
                    SetVScrollBar();
                    SetHScrollBar();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void SetVScrollBar()
        {
            try
            {
                vscVScrollBar.Minimum = 0;
                int _iRowDissidence = (dgvGridView.Rows.Count - dgvGridView.DisplayedRowCount(false));
                vscVScrollBar.LargeChange = (dgvGridView.Rows.Count - _iRowDissidence);
                int _iSmallChange = (int)((vscVScrollBar.LargeChange * ScrollBarProperties.SmallChangePercentageToLargeChange) / 100);
                if (_iSmallChange <= 0)
                {
                    _iSmallChange = 1;
                }
                vscVScrollBar.SmallChange = _iSmallChange;
                vscVScrollBar.Maximum = (dgvGridView.Rows.Count - 1);

                if (dgvGridView.DisplayedRowCount(false).Equals(dgvGridView.Rows.Count))
                {
                    vscVScrollBar.Value = vscVScrollBar.Minimum;

                    switch (ScrollBarProperties.ScrollBars)
                    {
                        case ScrollBars.Vertical:
                            {
                                tlpVScroll.Visible = false;
                            }
                            break;

                        case ScrollBars.Both:
                            {
                                tlpVScroll.Visible = false;
                                pnlScrollNC.Visible = false;
                            }
                            break;

                        case ScrollBars.Horizontal:
                        default:
                            break;
                    }
                    vscVScrollBar.Enabled = false;
                    btnToUp.Enabled = btnToDown.Enabled = false;
                }
                else
                {
                    switch (ScrollBarProperties.ScrollBars)
                    {
                        case ScrollBars.Vertical:
                            {
                                tlpVScroll.Visible = true;
                            }
                            break;

                        case ScrollBars.Both:
                            {
                                tlpVScroll.Visible = true;
                            }
                            break;

                        case ScrollBars.Horizontal:
                        default:
                            break;
                    }
                    vscVScrollBar.Enabled = true;
                    btnToUp.Enabled = btnToDown.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetHScrollBar()
        {
            try
            {
                if (!dgvGridView.AutoSizeColumnsMode.Equals(DataGridViewAutoSizeColumnsMode.Fill))
                {
                    int _iWidthDissidence = (dgvGridView.PreferredSize.Width - dgvGridView.ClientSize.Width);
                    hscHScrollBar.LargeChange = (dgvGridView.PreferredSize.Width - _iWidthDissidence);
                    int _iSmallChange = (int)((hscHScrollBar.LargeChange * ScrollBarProperties.SmallChangePercentageToLargeChange) / 100);
                    if (_iSmallChange <= 0)
                    {
                        _iSmallChange = 1;
                    }
                    hscHScrollBar.SmallChange = _iSmallChange;
                    hscHScrollBar.Maximum = dgvGridView.PreferredSize.Width;

                    if (dgvGridView.DisplayedColumnCount(false).Equals(dgvGridView.GetVisibleColumnCount()))
                    {
                        switch (ScrollBarProperties.ScrollBars)
                        {
                            case ScrollBars.Horizontal:
                                {
                                    tlpHScroll.Visible = false;
                                }
                                break;

                            case ScrollBars.Both:
                                {
                                    tlpHScroll.Visible = false;
                                    pnlScrollNC.Visible = false;
                                }
                                break;

                            case ScrollBars.Vertical:
                            default:
                                break;
                        }
                        hscHScrollBar.Enabled = false;
                        btnToForward.Enabled = btnToBackward.Enabled = false;
                    }
                    else
                    {
                        hscHScrollBar.Value = dgvGridView.HorizontalScrollingOffset;
                        switch (ScrollBarProperties.ScrollBars)
                        {
                            case ScrollBars.Horizontal:
                                {
                                    tlpHScroll.Visible = true;
                                }
                                break;

                            case ScrollBars.Both:
                                {
                                    tlpHScroll.Visible = true;
                                    if (!tlpVScroll.Visible)
                                    {
                                        pnlScrollNC.Visible = false;
                                    }
                                    else
                                    {
                                        pnlScrollNC.Visible = true;
                                    }
                                }
                                break;

                            case ScrollBars.Vertical:
                            default:
                                break;
                        }
                        hscHScrollBar.Enabled = true;
                        btnToForward.Enabled = btnToBackward.Enabled = true;
                    }
                }
                else
                {
                    switch (ScrollBarProperties.ScrollBars)
                    {
                        case ScrollBars.Horizontal:
                            {
                                tlpHScroll.Visible = false;
                            }
                            break;

                        case ScrollBars.Both:
                            {
                                tlpHScroll.Visible = false;
                                pnlScrollNC.Visible = false;
                            }
                            break;

                        case ScrollBars.Vertical:
                        default:
                            break;
                    }
                    hscHScrollBar.Enabled = false;
                    btnToForward.Enabled = btnToBackward.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void vscVScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                dgvGridView.FirstDisplayedScrollingRowIndex = e.NewValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnToUp_Click(object sender, EventArgs e)
        {
            try
            {
                int _iMinimum = vscVScrollBar.Minimum;
                dgvGridView.FirstDisplayedScrollingRowIndex = _iMinimum;
                vscVScrollBar.Value = dgvGridView.FirstDisplayedScrollingRowIndex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnToDown_Click(object sender, EventArgs e)
        {
            try
            {
                int _iMaximum = vscVScrollBar.Maximum;
                dgvGridView.FirstDisplayedScrollingRowIndex = _iMaximum;
                vscVScrollBar.Value = dgvGridView.FirstDisplayedScrollingRowIndex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void hscHScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                dgvGridView.HorizontalScrollingOffset = e.NewValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnToForward_Click(object sender, EventArgs e)
        {
            try
            {
                int _iMinimum = hscHScrollBar.Minimum;
                dgvGridView.HorizontalScrollingOffset = _iMinimum;
                hscHScrollBar.Value = _iMinimum;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnToBackward_Click(object sender, EventArgs e)
        {
            try
            {
                int _iMaximum = hscHScrollBar.Maximum;
                dgvGridView.HorizontalScrollingOffset = _iMaximum;
                hscHScrollBar.Value = _iMaximum;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void dgvGridView_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            try
            {
                SetHScrollBar();
                dgvGridView.HorizontalScrollingOffset = hscHScrollBar.Value;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void dgvGridView_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                m_IsKeyDown = e.Control;

                if (ScrollBarProperties.ScrollBars.Equals(ScrollBars.None))
                {
                    return;
                }

                SetHScrollBar();
                vscVScrollBar.Value = dgvGridView.FirstDisplayedScrollingRowIndex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void dgvGridView_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                m_IsKeyDown = e.Control;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void dgvGridView_MouseWheel(object sender, MouseEventArgs e)
        {
            try
            {
                if (ScrollBarProperties.ScrollBars.Equals(ScrollBars.None))
                {
                    return;
                }

                if (e.Delta > 0)
                {
                    if (m_IsKeyDown)
                    {
                        if (ScrollBarProperties.ScrollBars.Equals(ScrollBars.Horizontal) || ScrollBarProperties.ScrollBars.Equals(ScrollBars.Both))
                        {
                            if ((hscHScrollBar.LargeChange + hscHScrollBar.Value) < hscHScrollBar.Maximum)
                            {
                                dgvGridView.HorizontalScrollingOffset += 15;
                                hscHScrollBar.Value += 15;
                            }
                        }
                    }
                    else
                    {
                        if (ScrollBarProperties.ScrollBars.Equals(ScrollBars.Vertical) || ScrollBarProperties.ScrollBars.Equals(ScrollBars.Both))
                        {
                            if (dgvGridView.Rows.Count > 0)
                            {
                                if (dgvGridView.FirstDisplayedScrollingRowIndex > 1)
                                {
                                    dgvGridView.FirstDisplayedScrollingRowIndex -= 1;
                                    vscVScrollBar.Value -= 1;
                                }
                                else
                                {
                                    dgvGridView.FirstDisplayedScrollingRowIndex = 0;
                                    vscVScrollBar.Value = 0;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (m_IsKeyDown)
                    {
                        if (ScrollBarProperties.ScrollBars.Equals(ScrollBars.Horizontal) || ScrollBarProperties.ScrollBars.Equals(ScrollBars.Both))
                        {
                            if (dgvGridView.HorizontalScrollingOffset > 15)
                            {
                                dgvGridView.HorizontalScrollingOffset -= 15;
                                hscHScrollBar.Value -= 15;
                            }
                            else
                            {
                                dgvGridView.HorizontalScrollingOffset = 0;
                                hscHScrollBar.Value = 0;
                            }
                        }
                    }
                    else
                    {
                        if (ScrollBarProperties.ScrollBars.Equals(ScrollBars.Vertical) || ScrollBarProperties.ScrollBars.Equals(ScrollBars.Both))
                        {
                            if (dgvGridView.Rows.Count > 0)
                            {
                                if ((vscVScrollBar.LargeChange + dgvGridView.FirstDisplayedScrollingRowIndex) < dgvGridView.Rows.Count)
                                {
                                    dgvGridView.FirstDisplayedScrollingRowIndex += 1;
                                    vscVScrollBar.Value += 1;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetPageController(DataGridViewConstsDefiner.PagingSteps step, string pageMax = "", int itemCount = 0, string itemMax = "")
        {
            try
            {
                switch (step)
                {
                    case DataGridViewConstsDefiner.PagingSteps.Pre:
                        {
                            if (!IsPaging)
                            {
                                Page = 1;
                                m_iPageMax = 0;
                                m_IsRowsPerPageChanged = false;
                                Invoke(new Action(() =>
                                {
                                    btnToPrevious.Enabled = false;
                                    btnSkipToPrevious.Enabled = false;
                                    btnToNext.Enabled = false;
                                    btnSkipToNext.Enabled = false;
                                }));
                            }
                        }
                        break;

                    case DataGridViewConstsDefiner.PagingSteps.Post:
                        {
                            if (!int.TryParse(pageMax, out m_iPageMax))
                            {
                                m_iPageMax = 0;
                            }

                            if (m_iPageMax > 0 && Page < m_iPageMax)
                            {
                                Invoke(new Action(() =>
                                {
                                    btnToNext.Enabled = true;
                                    btnSkipToNext.Enabled = true;
                                }));
                            }
                            else
                            {
                                Invoke(new Action(() =>
                                {
                                    btnToNext.Enabled = false;
                                    btnSkipToNext.Enabled = false;
                                }));
                            }

                            if (Page > 1)
                            {
                                Invoke(new Action(() =>
                                {
                                    btnToPrevious.Enabled = true;
                                    btnSkipToPrevious.Enabled = true;
                                }));
                            }
                            else
                            {
                                Invoke(new Action(() =>
                                {
                                    btnToPrevious.Enabled = false;
                                    btnSkipToPrevious.Enabled = false;
                                }));
                            }

                            SetPageControllerStatus(itemCount, itemMax);
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetPageControllerStatus(int itemCount, string itemMax)
        {
            try
            {
                string _sStatus = string.Empty;
                if (itemCount.Equals(0))
                {
                    _sStatus = string.Format(BottomBarProperties.PageStatusShortFormat, "0");
                }
                else if (itemCount > 0)
                {
                    int _iStartIdx = (1 + ((Page - 1) * m_iPageSize));
                    int _iLastIdx = (itemCount + ((Page - 1) * m_iPageSize));
                    if (!int.TryParse(itemMax, out int _iItemMax))
                    {
                        _iItemMax = 0;
                    }
                    _sStatus = string.Format(BottomBarProperties.PageStatusLongFormat, _iStartIdx, _iLastIdx, _iItemMax, Page, m_iPageMax);
                }
                else
                {
                    _sStatus = string.Format(BottomBarProperties.PageStatusShortFormat, "0");
                }

                if (BottomBarProperties.IsBottomBar)
                {
                    Invoke(new Action(() =>
                    {
                        lblPageStatus.Text = _sStatus;
                    }));
                }

                if (TopBarProperties.IsPageControllerStatus)
                {
                    Invoke(new Action(() =>
                    {
                        if (m_iPageMax > 0)
                        {
                            lblPageControllerStatus.Text = string.Format(TopBarProperties.PageControllerStatusFormat, Page, m_iPageMax);
                        }
                        else
                        {
                            lblPageControllerStatus.Text = string.Format(TopBarProperties.PageControllerStatusFormat, "0", "0");
                        }
                    }));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetPageItemCountStatus(int itemCount)
        {
            try
            {
                string _sStatus = string.Empty;
                if (itemCount.Equals(0))
                {
                    _sStatus = string.Format(BottomBarProperties.PageStatusShortFormat, "0");
                }
                else if (itemCount > 0)
                {
                    _sStatus = string.Format(BottomBarProperties.PageStatusShortFormat, itemCount);
                }
                else
                {
                    _sStatus = string.Format(BottomBarProperties.PageStatusShortFormat, "0");
                }

                if (BottomBarProperties.IsBottomBar)
                {
                    Invoke(new Action(() =>
                    {
                        lblPageStatus.Text = _sStatus;
                    }));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnSkipToPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                IsPaging = true;
                if (m_IsRowsPerPageChanged)
                {
                    Page = 1;
                    m_iPageMax = 0;
                    m_IsRowsPerPageChanged = false;
                }
                else
                {
                    Page = 1;
                    Invoke(new Action(() =>
                    {
                        btnToPrevious.Enabled = false;
                        btnSkipToPrevious.Enabled = false;
                    }));
                }

                OnSkipToPreviousClick?.Invoke(sender, e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnToPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                IsPaging = true;
                if (m_IsRowsPerPageChanged)
                {
                    Page = 1;
                    m_iPageMax = 0;
                    m_IsRowsPerPageChanged = false;
                }
                else
                {
                    Page--;
                    if (Page <= 1)
                    {
                        Invoke(new Action(() =>
                        {
                            btnToPrevious.Enabled = false;
                            btnSkipToPrevious.Enabled = false;
                        }));
                    }
                }

                OnToPreviousClick?.Invoke(sender, e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnToNext_Click(object sender, EventArgs e)
        {
            try
            {
                IsPaging = true;
                if (m_IsRowsPerPageChanged)
                {
                    Page = 1;
                    m_iPageMax = 0;
                    m_IsRowsPerPageChanged = false;
                }
                else
                {
                    Page++;
                    if (Page.Equals(m_iPageMax))
                    {
                        Invoke(new Action(() =>
                        {
                            btnToNext.Enabled = false;
                            btnSkipToNext.Enabled = false;
                        }));
                    }
                }

                OnToNextClick?.Invoke(sender, e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnSkipToNext_Click(object sender, EventArgs e)
        {
            try
            {
                IsPaging = true;
                if (m_IsRowsPerPageChanged)
                {
                    Page = 1;
                    m_iPageMax = 0;
                    m_IsRowsPerPageChanged = false;
                }
                else
                {
                    Page = m_iPageMax;
                }
                Invoke(new Action(() =>
                {
                    btnToNext.Enabled = false;
                    btnSkipToNext.Enabled = false;
                }));

                OnSkipToNextClick?.Invoke(sender, e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PageControllers_EnabledChanged(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.Button _Sender = (sender as System.Windows.Forms.Button);
                if (_Sender.Enabled)
                {
                    if (_Sender.Equals(btnToPrevious))
                    {
                        _Sender.Image = TopBarProperties.ToPreviousEnableIcon;
                    }
                    else if (_Sender.Equals(btnSkipToPrevious))
                    {
                        _Sender.Image = TopBarProperties.SkipToPreviousEnableIcon;
                    }
                    else if (_Sender.Equals(btnToNext))
                    {
                        _Sender.Image = TopBarProperties.ToNextEnableIcon;
                    }
                    else if (_Sender.Equals(btnSkipToNext))
                    {
                        _Sender.Image = TopBarProperties.SkipToNextEnableIcon;
                    }
                }
                else
                {
                    if (_Sender.Equals(btnToPrevious))
                    {
                        _Sender.Image = TopBarProperties.ToPreviousDisableIcon;
                    }
                    else if (_Sender.Equals(btnSkipToPrevious))
                    {
                        _Sender.Image = TopBarProperties.SkipToPreviousDisableIcon;
                    }
                    else if (_Sender.Equals(btnToNext))
                    {
                        _Sender.Image = TopBarProperties.ToNextDisableIcon;
                    }
                    else if (_Sender.Equals(btnSkipToNext))
                    {
                        _Sender.Image = TopBarProperties.SkipToNextDisableIcon;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void txtRowsPerPage_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData.Equals(Keys.Enter))
                {
                    int _iPrePageSize = m_iPageSize;
                    if (!int.TryParse(txtRowsPerPage.Text.Trim(), out m_iPageSize) || m_iPageSize.Equals(0))
                    {
                        PageSize = 15;
                    }
                    if (!_iPrePageSize.Equals(m_iPageSize))
                    {
                        m_IsRowsPerPageChanged = true;
                    }
                }
                else if (e.Control && (e.KeyData.Equals((Keys.Control | Keys.V))))
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
                else if (e.KeyData.Equals(Keys.Escape))
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void txtRowsPerPage_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    if (e.KeyChar.Equals('-'))
                    {
                        e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void txtRowsPerPage_Enter(object sender, EventArgs e)
        {
            try
            {
                OnRowsPerPageKeyEnter?.Invoke(sender, e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void txtRowsPerPage_Leave(object sender, EventArgs e)
        {
            try
            {
                OnRowsPerPageKeyLeave?.Invoke(sender, e);

                int _iPrePageSize = m_iPageSize;
                if (!int.TryParse(txtRowsPerPage.Text.Trim(), out m_iPageSize) || m_iPageSize.Equals(0))
                {
                    PageSize = 15;
                }
                if (!_iPrePageSize.Equals(m_iPageSize))
                {
                    m_IsRowsPerPageChanged = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
        #endregion
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class GridViewProperties
    {
        public GridViewProperties()
        {
            HeaderProperties = new HeaderProperties();
            HeaderProperties.PropertyChanged += OnSubPropertyChanged;

            CellProperties = new CellProperties();
            CellProperties.PropertyChanged += OnSubPropertyChanged;
        }

        public override string ToString()
        {
            return string.Empty;
        }

        private Color m_GridViewBackColor = SystemColors.AppWorkspace;
        [DefaultValue(typeof(Color), "AppWorkspace")]
        public Color BackColor
        {
            get { return m_GridViewBackColor; }
            set
            {
                m_GridViewBackColor = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.GridViewBackColor.ToString());
            }
        }

        private int m_GridViewRowHeight = 50;
        [DefaultValue(50)]
        public int RowHeight
        {
            get { return m_GridViewRowHeight; }
            set
            {
                m_GridViewRowHeight = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.GridViewRowHeight.ToString());
            }
        }

        private bool m_GridViewIsMultiSelect = true;
        [DefaultValue(true)]
        public bool IsMultiSelect
        {
            get { return m_GridViewIsMultiSelect; }
            set
            {
                m_GridViewIsMultiSelect = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.GridViewIsMultiSelect.ToString());
            }
        }

        private bool m_GridViewIsAutoSizeColumn = false;
        [DefaultValue(false)]
        public bool IsAutoSizeColumn
        {
            get { return m_GridViewIsAutoSizeColumn; }
            set
            {
                m_GridViewIsAutoSizeColumn = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.GridViewIsAutoSizeColumn.ToString());
            }
        }

        private DataGridViewCellBorderStyle m_GridViewCellBorderStyle = DataGridViewCellBorderStyle.None;
        [DefaultValue(typeof(DataGridViewCellBorderStyle), "None")]
        public DataGridViewCellBorderStyle CellBorderStyle
        {
            get { return m_GridViewCellBorderStyle; }
            set
            {
                m_GridViewCellBorderStyle = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.GridViewCellBorderStyle.ToString());
            }
        }

        private Color m_GridViewGridColor = SystemColors.ControlDark;
        [DefaultValue(typeof(Color), "ControlDark")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.Repaint)]
        public Color GridColor
        {
            get { return m_GridViewGridColor; }
            set
            {
                m_GridViewGridColor = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.GridViewGridColor.ToString());
            }
        }

        private bool m_IsGridViewColumnHeadersVisible = true;
        [DefaultValue(true)]
        public bool IsColumnHeadersVisible
        {
            get { return m_IsGridViewColumnHeadersVisible; }
            set
            {
                m_IsGridViewColumnHeadersVisible = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.GridViewIsColumnHeadersVisible.ToString());
            }
        }

        private int m_iGridViewColumnHeadersHeight = 50;
        [DefaultValue(50)]
        public int ColumnHeadersHeight
        {
            get { return m_iGridViewColumnHeadersHeight; }
            set
            {
                m_iGridViewColumnHeadersHeight = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.GridViewColumnHeadersHeight.ToString());
            }
        }

        private bool m_GridViewIsRowHeadersVisible = true;
        [DefaultValue(true)]
        public bool IsRowHeadersVisible
        {
            get { return m_GridViewIsRowHeadersVisible; }
            set
            {
                m_GridViewIsRowHeadersVisible = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.GridViewIsRowHeadersVisible.ToString());
            }
        }

        private int m_iGridViewRowHeadersWidth = 50;
        [DefaultValue(50)]
        public int RowHeadersWidth
        {
            get { return m_iGridViewRowHeadersWidth; }
            set
            {
                m_iGridViewRowHeadersWidth = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.GridViewRowHeadersWidth.ToString());
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.All)]
        public HeaderProperties HeaderProperties { get; set; }

        private Color m_GridViewCellBackColor = SystemColors.Window;
        [DefaultValue(typeof(Color), "Window")]
        public Color CellBackColor
        {
            get { return m_GridViewCellBackColor; }
            set
            {
                m_GridViewCellBackColor = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.GridViewCellBackColor.ToString());
            }
        }

        private Color m_GridViewCellForeColor = SystemColors.ControlText;
        [DefaultValue(typeof(Color), "ControlText")]
        public Color CellForeColor
        {
            get { return m_GridViewCellForeColor; }
            set
            {
                m_GridViewCellForeColor = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.GridViewCellForeColor.ToString());
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.All)]
        public CellProperties CellProperties { get; set; }

        private Color m_GridViewAlternatingRowsBackColor = SystemColors.Window;
        [DefaultValue(typeof(Color), "Window")]
        public Color AlternatingRowsBackColor
        {
            get { return m_GridViewAlternatingRowsBackColor; }
            set
            {
                m_GridViewAlternatingRowsBackColor = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.GridViewProperties.GridViewAlternatingRowsBackColor.ToString());
            }
        }

        private void OnSubPropertyChanged(object sender, ExtensionsPropertyEventArgs e)
        {
            try
            {
                OnPropertyChanged(e.PropertyName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Browsable(false)]
        public event EventHandler<ExtensionsPropertyEventArgs> PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new ExtensionsPropertyEventArgs(propertyName));
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ScrollBarProperties
    {
        public ScrollBarProperties()
        {
        }

        public override string ToString()
        {
            return string.Empty;
        }

        private ScrollBars m_ScrollBars = ScrollBars.None;
        [DefaultValue(typeof(ScrollBars), "None")]
        public ScrollBars ScrollBars
        {
            get { return m_ScrollBars; }
            set
            {
                m_ScrollBars = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.ScrollBarProperties.ScrollBars.ToString());
            }
        }

        private decimal m_SmallChangePercentageToLargeChange = 1M;
        [DefaultValue(typeof(decimal), "1")]
        public decimal SmallChangePercentageToLargeChange
        {
            get { return m_SmallChangePercentageToLargeChange; }
            set
            {
                m_SmallChangePercentageToLargeChange = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.ScrollBarProperties.SmallChangePercentageToLargeChange.ToString());
            }
        }

        private int m_VScrollBarControllerWidth = 36;
        [DefaultValue(36)]
        public int VScrollBarControllerWidth
        {
            get { return m_VScrollBarControllerWidth; }
            set
            {
                m_VScrollBarControllerWidth = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.ScrollBarProperties.VScrollBarControllerWidth.ToString());
            }
        }

        private int m_VScrollBarControllerHeight = 36;
        [DefaultValue(36)]
        public int VScrollBarControllerHeight
        {
            get { return m_VScrollBarControllerHeight; }
            set
            {
                m_VScrollBarControllerHeight = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.ScrollBarProperties.VScrollBarControllerHeight.ToString());
            }
        }

        private int m_HScrollBarControllerWidth = 36;
        [DefaultValue(36)]
        public int HScrollBarControllerWidth
        {
            get { return m_HScrollBarControllerWidth; }
            set
            {
                m_HScrollBarControllerWidth = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.ScrollBarProperties.HScrollBarControllerWidth.ToString());
            }
        }

        private int m_HScrollBarControllerHeight = 36;
        [DefaultValue(36)]
        public int HScrollBarControllerHeight
        {
            get { return m_HScrollBarControllerHeight; }
            set
            {
                m_HScrollBarControllerHeight = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.ScrollBarProperties.HScrollBarControllerHeight.ToString());
            }
        }

        private Color m_ScrollControllerBackColor = SystemColors.ControlDark;
        [DefaultValue(typeof(Color), "ControlDark")]
        public Color ScrollControllerBackColor
        {
            get { return m_ScrollControllerBackColor; }
            set
            {
                m_ScrollControllerBackColor = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.ScrollBarProperties.ScrollControllerBackColor.ToString());
            }
        }

        private Color m_ScrollControllerMouseDownBackColor = SystemColors.ControlDarkDark;
        [DefaultValue(typeof(Color), "ControlDarkDark")]
        public Color ScrollControllerMouseDownBackColor
        {
            get { return m_ScrollControllerMouseDownBackColor; }
            set
            {
                m_ScrollControllerMouseDownBackColor = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.ScrollBarProperties.ScrollControllerMouseDownBackColor.ToString());
            }
        }

        private Color m_ScrollControllerMouseOverBackColor = SystemColors.ControlDarkDark;
        [DefaultValue(typeof(Color), "ControlDarkDark")]
        public Color ScrollControllerMouseOverBackColor
        {
            get { return m_ScrollControllerMouseOverBackColor; }
            set
            {
                m_ScrollControllerMouseOverBackColor = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.ScrollBarProperties.ScrollControllerMouseOverBackColor.ToString());
            }
        }

        private Image m_imgToUpIcon = Properties.Resources.ScrollToTop_36W;
        public Image ToUpIcon
        {
            get { return m_imgToUpIcon; }
            set
            {
                m_imgToUpIcon = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.ScrollBarProperties.ToUpIcon.ToString());
            }
        }

        private Image m_imgToDownIcon = Properties.Resources.ScrollToBottom_36W;
        public Image ToDownIcon
        {
            get { return m_imgToDownIcon; }
            set
            {
                m_imgToDownIcon = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.ScrollBarProperties.ToDownIcon.ToString());
            }
        }

        private Image m_imgToForwardIcon = Properties.Resources.ScrollToForward_36W;
        public Image ToForwardIcon
        {
            get { return m_imgToForwardIcon; }
            set
            {
                m_imgToForwardIcon = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.ScrollBarProperties.ToForwardIcon.ToString());
            }
        }

        private Image m_imgToBackwardIcon = Properties.Resources.ScrollToBackward_36W;
        public Image ToBackwardIcon
        {
            get { return m_imgToBackwardIcon; }
            set
            {
                m_imgToBackwardIcon = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.ScrollBarProperties.ToBackwardIcon.ToString());
            }
        }

        [Browsable(false)]
        public event EventHandler<ExtensionsPropertyEventArgs> PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new ExtensionsPropertyEventArgs(propertyName));
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class TopBarProperties
    {
        public TopBarProperties()
        {
        }

        public override string ToString()
        {
            return string.Empty;
        }

        private bool m_IsTopBar = false;
        [DefaultValue(false)]
        public bool IsTopBar
        {
            get { return m_IsTopBar; }
            set
            {
                m_IsTopBar = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.IsTopBar.ToString());
            }
        }

        private Color m_TopBarBackColor = SystemColors.Control;
        [DefaultValue(typeof(Color), "Control")]
        public Color BackColor
        {
            get { return m_TopBarBackColor; }
            set
            {
                m_TopBarBackColor = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.TopBarBackColor.ToString());
            }
        }

        private int m_TopBarHeight = 40;
        [DefaultValue(40)]
        public int Height
        {
            get { return m_TopBarHeight; }
            set
            {
                m_TopBarHeight = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.TopBarHeight.ToString());
            }
        }

        private Padding m_TopBarMargin = new Padding(0);
        [DefaultValue(typeof(Padding), "0, 0, 0, 0")]
        public Padding Margin
        {
            get { return m_TopBarMargin; }
            set
            {
                m_TopBarMargin = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.TopBarMargin.ToString());
            }
        }

        private Image m_imgTitleIcon = null;
        public Image TitleIcon
        {
            get { return m_imgTitleIcon; }
            set
            {
                m_imgTitleIcon = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.TitleIcon.ToString());
            }
        }

        private string m_sTitle = string.Empty;
        [DefaultValue("")]
        public string Title
        {
            get { return m_sTitle; }
            set
            {
                m_sTitle = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.Title.ToString());
            }
        }

        private Font m_TitleFont = new Font("Microsoft Sans Serif", 9F);
        [DefaultValue(typeof(Font), "Microsoft Sans Serif, 9pt")]
        public Font TitleFont
        {
            get { return m_TitleFont; }
            set
            {
                m_TitleFont = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.TitleFont.ToString());
            }
        }

        private Color m_TitleForeColor = SystemColors.ControlText;
        [DefaultValue(typeof(Color), "ControlText")]
        public Color TitleForeColor
        {
            get { return m_TitleForeColor; }
            set
            {
                m_TitleForeColor = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.TitleForeColor.ToString());
            }
        }

        private bool m_IsPageController = false;
        [DefaultValue(false)]
        public bool IsPageController
        {
            get { return m_IsPageController; }
            set
            {
                m_IsPageController = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.IsPageController.ToString());
            }
        }

        private bool m_IsPageControllerStatus = false;
        [DefaultValue(false)]
        public bool IsPageControllerStatus
        {
            get { return m_IsPageControllerStatus; }
            set
            {
                m_IsPageControllerStatus = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.IsPageControllerStatus.ToString());
            }
        }

        private int m_PageControllerContainerExtraWidth = 0;
        [DefaultValue(0)]
        public int PageControllerContainerExtraWidth
        {
            get { return m_PageControllerContainerExtraWidth; }
            set
            {
                m_PageControllerContainerExtraWidth = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.PageControllerContainerExtraWidth.ToString());
            }
        }

        private string m_PageControllerStatusFormat = "Page {0} of {1}";
        [DefaultValue("Page {0} of {1}")]
        public string PageControllerStatusFormat
        {
            get { return m_PageControllerStatusFormat; }
            set
            {
                m_PageControllerStatusFormat = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.PageControllerStatusFormat.ToString());
            }
        }

        private Font m_PageControllerStatusFont = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold);
        [DefaultValue(typeof(Font), "Microsoft Sans Serif, 9.75pt, style=Bold")]
        public Font PageControllerStatusFont
        {
            get { return m_PageControllerStatusFont; }
            set
            {
                m_PageControllerStatusFont = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.PageControllerStatusFont.ToString());
            }
        }

        private Color m_PageControllerStatusForeColor = SystemColors.GrayText;
        [DefaultValue(typeof(Color), "GrayText")]
        public Color PageControllerStatusForeColor
        {
            get { return m_PageControllerStatusForeColor; }
            set
            {
                m_PageControllerStatusForeColor = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.PageControllerStatusForeColor.ToString());
            }
        }

        private int m_PageControllerWidth = 50;
        [DefaultValue(50)]
        public int PageControllerWidth
        {
            get { return m_PageControllerWidth; }
            set
            {
                m_PageControllerWidth = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.PageControllerWidth.ToString());
            }
        }

        private Color m_PageControllerBackColor = SystemColors.ControlDark;
        [DefaultValue(typeof(Color), "ControlDark")]
        public Color PageControllerBackColor
        {
            get { return m_PageControllerBackColor; }
            set
            {
                m_PageControllerBackColor = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.PageControllerBackColor.ToString());
            }
        }

        private Color m_PageControllerMouseDownBackColor = SystemColors.ControlDarkDark;
        [DefaultValue(typeof(Color), "ControlDarkDark")]
        public Color PageControllerMouseDownBackColor
        {
            get { return m_PageControllerMouseDownBackColor; }
            set
            {
                m_PageControllerMouseDownBackColor = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.PageControllerMouseDownBackColor.ToString());
            }
        }

        private Color m_PageControllerMouseOverBackColor = SystemColors.ControlDarkDark;
        [DefaultValue(typeof(Color), "ControlDarkDark")]
        public Color PageControllerMouseOverBackColor
        {
            get { return m_PageControllerMouseOverBackColor; }
            set
            {
                m_PageControllerMouseOverBackColor = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.PageControllerMouseOverBackColor.ToString());
            }
        }

        private Image m_imgSkipToPreviousEnableIcon = Properties.Resources.SkipToBackward_36W;
        public Image SkipToPreviousEnableIcon
        {
            get { return m_imgSkipToPreviousEnableIcon; }
            set
            {
                m_imgSkipToPreviousEnableIcon = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.SkipToPreviousEnableIcon.ToString());
            }
        }

        private Image m_imgSkipToPreviousDisableIcon = Properties.Resources.SkipToBackward_36G;
        public Image SkipToPreviousDisableIcon
        {
            get { return m_imgSkipToPreviousDisableIcon; }
            set
            {
                m_imgSkipToPreviousDisableIcon = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.SkipToPreviousDisableIcon.ToString());
            }
        }

        private Image m_imgToPreviousEnableIcon = Properties.Resources.ToBackward_36W;
        public Image ToPreviousEnableIcon
        {
            get { return m_imgToPreviousEnableIcon; }
            set
            {
                m_imgToPreviousEnableIcon = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.ToPreviousEnableIcon.ToString());
            }
        }

        private Image m_imgToPreviousDisableIcon = Properties.Resources.ToBackward_36G;
        public Image ToPreviousDisableIcon
        {
            get { return m_imgToPreviousDisableIcon; }
            set
            {
                m_imgToPreviousDisableIcon = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.ToPreviousDisableIcon.ToString());
            }
        }

        private Image m_imgToNextEnableIcon = Properties.Resources.ToForward_36W;
        public Image ToNextEnableIcon
        {
            get { return m_imgToNextEnableIcon; }
            set
            {
                m_imgToNextEnableIcon = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.ToNextEnableIcon.ToString());
            }
        }

        private Image m_imgToNextDisableIcon = Properties.Resources.ToForward_36G;
        public Image ToNextDisableIcon
        {
            get { return m_imgToNextDisableIcon; }
            set
            {
                m_imgToNextDisableIcon = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.ToNextDisableIcon.ToString());
            }
        }

        private Image m_imgSkipToNextEnableIcon = Properties.Resources.SkipToForward_36W;
        public Image SkipToNextEnableIcon
        {
            get { return m_imgSkipToNextEnableIcon; }
            set
            {
                m_imgSkipToNextEnableIcon = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.SkipToNextEnableIcon.ToString());
            }
        }

        private Image m_imgSkipToNextDisableIcon = Properties.Resources.SkipToForward_36G;
        public Image SkipToNextDisableIcon
        {
            get { return m_imgSkipToNextDisableIcon; }
            set
            {
                m_imgSkipToNextDisableIcon = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.TopBarProperties.SkipToNextDisableIcon.ToString());
            }
        }

        [Browsable(false)]
        public event EventHandler<ExtensionsPropertyEventArgs> PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new ExtensionsPropertyEventArgs(propertyName));
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class BottomBarProperties
    {
        public BottomBarProperties()
        {
        }

        public override string ToString()
        {
            return string.Empty;
        }

        private bool m_IsBottomBar = false;
        [DefaultValue(false)]
        public bool IsBottomBar
        {
            get { return m_IsBottomBar; }
            set
            {
                m_IsBottomBar = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.BottomBarProperties.IsBottomBar.ToString());
            }
        }

        private Color m_BottomBarBackColor = SystemColors.Control;
        [DefaultValue(typeof(Color), "Control")]
        public Color BackColor
        {
            get { return m_BottomBarBackColor; }
            set
            {
                m_BottomBarBackColor = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.BottomBarProperties.BottomBarBackColor.ToString());
            }
        }

        private int m_BottomBarHeight = 30;
        [DefaultValue(30)]
        public int Height
        {
            get { return m_BottomBarHeight; }
            set
            {
                m_BottomBarHeight = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.BottomBarProperties.BottomBarHeight.ToString());
            }
        }

        private Padding m_BottomBarMargin = new Padding(0);
        [DefaultValue(typeof(Padding), "0, 0, 0, 0")]
        public Padding Margin
        {
            get { return m_BottomBarMargin; }
            set
            {
                m_BottomBarMargin = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.BottomBarProperties.BottomBarMargin.ToString());
            }
        }

        private string m_sPageStatusShortFormat = "Items {0}.";
        [DefaultValue("Items {0}.")]
        public string PageStatusShortFormat
        {
            get { return m_sPageStatusShortFormat; }
            set
            {
                m_sPageStatusShortFormat = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.BottomBarProperties.PageStatusShortFormat.ToString());
            }
        }

        private string m_sPageStatusLongFormat = "Items {0}-{1} of {2}, Page {3} of {4}.";
        [DefaultValue("Items {0}-{1} of {2}, Page {3} of {4}.")]
        public string PageStatusLongFormat
        {
            get { return m_sPageStatusLongFormat; }
            set
            {
                m_sPageStatusLongFormat = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.BottomBarProperties.PageStatusLongFormat.ToString());
            }
        }

        private Font m_PageStatusFont = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold);
        [DefaultValue(typeof(Font), "Microsoft Sans Serif, 9.75pt, style=Bold")]
        public Font PageStatusFont
        {
            get { return m_PageStatusFont; }
            set
            {
                m_PageStatusFont = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.BottomBarProperties.PageStatusFont.ToString());
            }
        }

        private Color m_PageStatusForeColor = SystemColors.GrayText;
        [DefaultValue(typeof(Color), "GrayText")]
        public Color PageStatusForeColor
        {
            get { return m_PageStatusForeColor; }
            set
            {
                m_PageStatusForeColor = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.BottomBarProperties.PageStatusForeColor.ToString());
            }
        }

        private bool m_IsRowsPerPage = false;
        [DefaultValue(false)]
        public bool IsRowsPerPage
        {
            get { return m_IsRowsPerPage; }
            set
            {
                m_IsRowsPerPage = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.BottomBarProperties.IsRowsPerPage.ToString());
            }
        }

        private Color m_RowsPerPageBackColor = SystemColors.Window;
        [DefaultValue(typeof(Color), "Window")]
        public Color RowsPerPageBackColor
        {
            get { return m_RowsPerPageBackColor; }
            set
            {
                m_RowsPerPageBackColor = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.BottomBarProperties.RowsPerPageBackColor.ToString());
            }
        }

        private int m_RowsPerPageWidth = 42;
        [DefaultValue(42)]
        public int RowsPerPageWidth
        {
            get { return m_RowsPerPageWidth; }
            set
            {
                m_RowsPerPageWidth = value;
                OnPropertyChanged(DataGridViewConstsDefiner.ExtensionsPropertyNames.BottomBarProperties.RowsPerPageWidth.ToString());
            }
        }

        [Browsable(false)]
        public event EventHandler<ExtensionsPropertyEventArgs> PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new ExtensionsPropertyEventArgs(propertyName));
        }
    }
}