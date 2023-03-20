using System.ComponentModel.DataAnnotations;

namespace TekTestApi.Model
{
    public class JobsModel
    {
        [Required]
        public string title { get; set; }
        [Required]
        public string description { get; set; }
        [Required]
        public long locationID { get; set; }
        [Required]
        public long departmentID { get; set; }
        [Required]
        public string closingDate { get; set; }
    }

    public class JobsByIdModel
    {
        public long id { get; set; }
        public string code { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public LocationModel location { get; set; }
        public DepartmentModel department { get; set; }
        public string postedDate { get; set; }
        public string closingDate { get; set; }
    }

    public class JobsSearchModel
    {
        public long id { get; set; }
        public string code { get; set; }
        public string title { get; set; }
        public string location { get; set; }
        public string department { get; set; }
        public string postedDate { get; set; }
        public string closingDate { get; set; }
    }
}
