using TekTestApi.Model;

namespace TekTestApi.Interface
{
    public interface IJobsRepo
    {
        Task<ResultDto> InsertJobDetails(JobsModel jobDetails);

        Task<ResultDto> UpdateJobsDetails(JobsModel jobDtlsUpd, long jobId);
        Task<SearchResponseModel> GetAllJobDtls(SearchModel searchModel);
        Task<JobsByIdModel> GetJobDtlsById(long jobId);
        Task<ResultDto> InsertLocation(LocationInsertModel location);

        Task<ResultDto> UpdateLocation(int Id, LocationInsertModel location);

        Task<List<LocationModel>> GetLocation();

        Task<ResultDto> InsertDepartment(DepartmentInsertModel department);

        Task<ResultDto> UpdateDepartment(int Id, DepartmentInsertModel department);

        Task<List<DepartmentModel>> GetDepartment();
    }
}
