using Microsoft.AspNetCore.Mvc;
using TekTestApi.Interface;
using TekTestApi.Model;

namespace TekTestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        private readonly IJobsRepo _jobsRepository;
        private readonly IJobsRepoUsingDapper _jobsRepositoryUsingDapper;
        public JobsController(IJobsRepo JobsRepository, IJobsRepoUsingDapper jobsRepositoryUsingDapper)
        {
            _jobsRepository = JobsRepository;
            _jobsRepositoryUsingDapper = jobsRepositoryUsingDapper;
        }

        [HttpPost]
        [Route("InsertJob")]

        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> InsertJobDetails(JobsModel lstJobDetails)
        {

            var data =await _jobsRepository.InsertJobDetails(lstJobDetails);
            if(data.Result==true)
            {
                return Ok(data.Details);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, data.Details);
            }
            
        }

        [HttpPut]
        [Route("UpdateJob/{id}")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateJobDetails(JobsModel jobDtlsUpd, long id)
        {
            var data = await _jobsRepository.UpdateJobsDetails(jobDtlsUpd, id);
            if (data.Result == true)
            {
                return Ok(data);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, data.Details);
            }
        }

        [HttpPost]
        [Route("GetJobsList")]
        public async Task<SearchResponseModel> GetAllJobDtls( SearchModel jobDtls)
        {
            var data=await _jobsRepository.GetAllJobDtls(jobDtls);
            return data;
        }

        [HttpGet]
        [Route("GetJob/{id}")]
        public async Task<JobsByIdModel> GetAllJobDtls(long id)
        {
            var data = await _jobsRepository.GetJobDtlsById(id);
            return data;
        }

        [HttpPost]
        [Route("InsertLocation")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> InsertLocation(LocationInsertModel location)
        {

            var data = _jobsRepository.InsertLocation(location);
            return Ok(data);
        }

        [HttpPut]
        [Route("UpdateLocation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateLocation(int Id, LocationInsertModel location)
        {

            var data = _jobsRepository.UpdateLocation(Id, location);
            return Ok(data);
        }


        [HttpGet]
        [Route("GetLocation")]
        /*[ProducesResponseType(StatusCodes.Status200OK)]*/
        public async Task<IActionResult> GetLocation()
        {

            var data = await _jobsRepository.GetLocation();
            return Ok(data);
        }


        [HttpPost]
        [Route("InsertDepartment")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> InsertDepartment(DepartmentInsertModel department)
        {

            var data = _jobsRepository.InsertDepartment(department);
            return Ok(data);
        }


        [HttpPut]
        [Route("UpdateDepartment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateDepartment(int Id, DepartmentInsertModel department)
        {

            var data = _jobsRepository.UpdateDepartment(Id, department);
            return Ok(data);
        }



        [HttpGet]
        [Route("GetDepartment")]
        /*[ProducesResponseType(StatusCodes.Status200OK)]*/
        public async Task<IActionResult> GetDepartment()
        {

            var data = await _jobsRepository.GetDepartment();
            return Ok(data);
        }



    }
}
