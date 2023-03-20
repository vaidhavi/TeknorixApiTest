namespace TekTestApi.Model
{
    public class LocationModel
    {
        public long id { get; set; }
        public string title { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public int zip { get; set; }
    }

    public class LocationInsertModel
    {
        public string title { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public int zip { get; set; }
    }
}
