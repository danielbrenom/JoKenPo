using System.ComponentModel;

namespace JoKenPo.Auxiliar
{
    public enum TableTypes
    {
        [Description("table-dark")]
        TableDark,
        [Description("table-stripped")]
        TableStriped,
        [Description("table-bordered")]
        TableBordered,
        [Description("table-borderless")]
        TableBorderless,
        [Description("table-hover")]
        TableHover,
        [Description("table-small")]
        TableSmall,
        [Description("table-responsive-md")]
        TableResponsive
    }
}