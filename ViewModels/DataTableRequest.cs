namespace TestAPI.ViewModels
{
    public class DataTableRequest
    {
        public string draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public int skip { get; set; }
        public int pageSize { get; set; }
        public string sortColumn { get; set; }
        public string? sortColumnDirection { get; set; }
        public string? searchValue { get; set; }
    }
}
