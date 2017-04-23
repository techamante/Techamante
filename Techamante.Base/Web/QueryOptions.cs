namespace Techamante.Web
{
    public class QueryOptions
    {
        public string FilterBy { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public ActionType? Action { get; set; }

        public ViewType? ViewType { get; set; }

    }


    public enum ViewType
    {
        Summary,
        Detail
    }

    public enum ActionType
    {

    }
}