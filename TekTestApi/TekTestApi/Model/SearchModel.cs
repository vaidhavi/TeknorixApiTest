using System.ComponentModel;

namespace TekTestApi.Model
{
    public class SearchModel
    {
        [DefaultValue(null)]
        public string? q { get; set; }
        public int pageNo { get; set; } 
        public int pageSize { get; set; }
        public long locationId { get; set; }
        public long departmentId { get; set; }
        public SearchModel()
        {
            this.pageNo = 1;
            this.pageSize = 10;
        }
        public SearchModel(int pageNumber, int pageSize)
        {
            this.pageNo = pageNumber < 1 ? 1 : pageNumber;
            this.pageSize = pageSize > 10 ? 10 : pageSize;
        }
    }
    public class SearchResponseModel
    {
        public int total { get; set; }
        public IEnumerable<JobsSearchModel> data { get; set; }
 
    }

}
