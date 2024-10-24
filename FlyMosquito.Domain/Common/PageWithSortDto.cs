#region using
#endregion

namespace FlyMosquito.Domain
{
    public class PageWithSortDto
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string Sort { get; set; }

        public OrederType OrederType { get; set; } = OrederType.Asc;
    }

    public enum OrederType
    {
        Asc,
        Desc,
    }
}
