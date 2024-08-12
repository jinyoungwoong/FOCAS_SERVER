namespace NCManagementSystem.Libraries.Controls.CheckBox
{
    public class CheckBoxConstsDefiner
    {
        public enum MouseState : byte
        {
            None = 0,
            Hover = 1,
            Down = 2
        }

        internal struct ExtensionsPropertyNames
        {
            internal enum CheckBoxSquareProperties
            {
                CheckBoxSquareBackColor,
                CheckBoxSquareMargin,
                IsCheckBoxSquareAutoSize,
                CheckBoxSquareSize,
                CheckBoxSquareBorderColorNormal,
                CheckBoxSquareBorderColorMouseOver,
                CheckBoxSquareBorderColorChecked,
                CheckBoxSquareBorderColorDisabled,
                IsCheckMarkBold,
                CheckMarkColorMouseOver,
                CheckMarkColorChecked,
                CheckMarkColorDisabled
            }
        }
    }
}
