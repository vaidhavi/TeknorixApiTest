namespace TekTestApi.Model
{
    public class DepartmentModel
    {
        public long id { get; set; }
        public string title { get; set; }
    }

    public class DepartmentInsertModel
    {
        public string title { get; set; }
    }
}
