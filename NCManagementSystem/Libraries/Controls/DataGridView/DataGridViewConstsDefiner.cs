using System;
using NCManagementSystem.Libraries.Controls.DataGridView.Components;

namespace NCManagementSystem.Libraries.Controls.DataGridView
{
    public class DataGridViewConstsDefiner
    {
        public delegate void RowsPerPageKeyEnterEventHandler(object sender);
        public delegate void RowsPerPageKeyLeaveEventHandler(object sender);

        public enum ColumnAlignment
        {
            left,
            center,
            right,
        }

        public enum PagingSteps
        {
            Pre,
            Post
        }

        internal struct ExtensionsPropertyNames
        {
            internal enum HeaderProperties
            {
                HeaderFont,
                IsVisibleSelectedRowNumber,
                HeaderBorderColor,
                ColumnFirstBackColor,
                ColumnLastBackColor,
                ColumnForeColor,
                RowFirstBackColor,
                RowLastBackColor,
                RowForeColor,
                RowSelectionFirstBackColor,
                RowSelectionLastBackColor,
                RowSelectionForeColor,
                NCFirstBackColor,
                NCLastBackColor
            }

            internal enum CellProperties
            {
                CellFont,
                IsCheckBoxCellInSquares,
                CellSelectionBackColor,
                CellSelectionForeColor
            }

            internal enum TopBarProperties
            {
                IsTopBar,
                TopBarBackColor,
                TopBarHeight,
                TopBarMargin,
                TitleIcon,
                Title,
                TitleFont,
                TitleForeColor,
                IsPageController,
                IsPageControllerStatus,
                PageControllerContainerExtraWidth,
                PageControllerStatusFormat,
                PageControllerStatusFont,
                PageControllerStatusForeColor,
                PageControllerWidth,
                PageControllerBackColor,
                PageControllerMouseDownBackColor,
                PageControllerMouseOverBackColor,
                SkipToPreviousEnableIcon,
                SkipToPreviousDisableIcon,
                ToPreviousEnableIcon,
                ToPreviousDisableIcon,
                ToNextEnableIcon,
                ToNextDisableIcon,
                SkipToNextEnableIcon,
                SkipToNextDisableIcon
            }

            internal enum BottomBarProperties
            {
                IsBottomBar,
                BottomBarBackColor,
                BottomBarHeight,
                BottomBarMargin,
                PageStatusShortFormat,
                PageStatusLongFormat,
                PageStatusFont,
                PageStatusForeColor,
                IsRowsPerPage,
                RowsPerPageBackColor,
                RowsPerPageWidth
            }

            internal enum ScrollBarProperties
            {
                ScrollBars,
                SmallChangePercentageToLargeChange,
                VScrollBarControllerWidth,
                VScrollBarControllerHeight,
                HScrollBarControllerWidth,
                HScrollBarControllerHeight,
                ScrollControllerBackColor,
                ScrollControllerMouseDownBackColor,
                ScrollControllerMouseOverBackColor,
                ToUpIcon,
                ToDownIcon,
                ToForwardIcon,
                ToBackwardIcon
            }

            internal enum GridViewProperties
            {
                GridViewBackColor,
                GridViewRowHeight,
                GridViewIsMultiSelect,
                GridViewIsAutoSizeColumn,
                GridViewCellBorderStyle,
                GridViewGridColor,
                GridViewIsColumnHeadersVisible,
                GridViewColumnHeadersHeight,
                GridViewIsRowHeadersVisible,
                GridViewRowHeadersWidth,
                HeaderFont,
                IsVisibleSelectedRowNumber,
                HeaderBorderColor,
                ColumnFirstBackColor,
                ColumnLastBackColor,
                ColumnForeColor,
                RowFirstBackColor,
                RowLastBackColor,
                RowForeColor,
                RowSelectionFirstBackColor,
                RowSelectionLastBackColor,
                RowSelectionForeColor,
                NCFirstBackColor,
                NCLastBackColor,
                GridViewCellBackColor,
                GridViewCellForeColor,
                CellFont,
                IsCheckBoxCellInSquares,
                CellSelectionBackColor,
                CellSelectionForeColor,
                GridViewAlternatingRowsBackColor
            }
        }
    }
}
