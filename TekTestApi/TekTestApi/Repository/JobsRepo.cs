using System.Data.SqlClient;
using System.Data;
using TekTestApi.Interface;
using TekTestApi.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http.Extensions;
using System.Collections.Generic;
using System;

namespace TekTestApi.Repository
{
    public class JobsRepo:IJobsRepo
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public JobsRepo(IConfiguration configuration,IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SqlConnection");
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResultDto> InsertJobDetails(JobsModel jobDetails)
        {
            try
            {
                string Url = null;
                SqlConnection sqlconn = new SqlConnection(_connectionString);
                SqlCommand sqlcomm = new SqlCommand("INSERT_JOBSDETAILS");
                sqlconn.Open();
                sqlcomm.Connection = sqlconn;
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@Title", jobDetails.title);
                sqlcomm.Parameters.AddWithValue("@Description", jobDetails.description);
                sqlcomm.Parameters.AddWithValue("@LocationID", jobDetails.locationID);
                sqlcomm.Parameters.AddWithValue("@DepartmentID", jobDetails.departmentID);
                sqlcomm.Parameters.AddWithValue("@ClosingDate", jobDetails.closingDate);
                SqlDataReader sdr = sqlcomm.ExecuteReader();
                while (sdr.Read())
                {
                    Url = _httpContextAccessor.HttpContext.Request.GetDisplayUrl() + "/" + Convert.ToInt64(sdr["Job_ID"]);
                }
                    

                return new ResultDto
                {
                    Result = true,
                    Details = Url,
                    Status = System.Net.HttpStatusCode.Created,
                };

            }
            catch (Exception exp)
            {
                return new ResultDto
                {
                    Result = false,
                    Details = exp.Message,
                    Status = System.Net.HttpStatusCode.InternalServerError,
                };
            }
        }

        public async Task<ResultDto> UpdateJobsDetails(JobsModel jobDtlsUpd, long jobId)
        {
            try
            {
                string Url = null;
                SqlConnection sqlconn = new SqlConnection(_connectionString);
                SqlCommand sqlcomm = new SqlCommand("INSERT_JOBSDETAILS");
                sqlconn.Open();
                sqlcomm.Connection = sqlconn;
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@ID", jobId);
                sqlcomm.Parameters.AddWithValue("@Title", jobDtlsUpd.title);
                sqlcomm.Parameters.AddWithValue("@Description", jobDtlsUpd.description);
                sqlcomm.Parameters.AddWithValue("@LocationID", jobDtlsUpd.locationID);
                sqlcomm.Parameters.AddWithValue("@DepartmentID", jobDtlsUpd.departmentID);
                sqlcomm.Parameters.AddWithValue("@ClosingDate", jobDtlsUpd.closingDate);
                SqlDataReader sdr = sqlcomm.ExecuteReader();

                return new ResultDto
                {
                    Result = true,
                    Details = "Successfull",
                    Status = System.Net.HttpStatusCode.OK,
                };
            }
            catch (Exception exp)
            {
                return new ResultDto
                {
                    Result = false,
                    Details = exp.Message,
                    Status = System.Net.HttpStatusCode.InternalServerError,
                };
            }
        }

        public async Task<SearchResponseModel> GetAllJobDtls(SearchModel searchModel)
        {
            try
            {
                SearchResponseModel obj = new SearchResponseModel();
                SqlConnection sqlconn = new SqlConnection(_connectionString);
                SqlCommand sqlcomm = new SqlCommand("GET_JOBSDETAILS");
                sqlconn.Open();
                sqlcomm.Connection = sqlconn;
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@query", searchModel.q);
                if (searchModel.locationId != 0)
                {
                    sqlcomm.Parameters.AddWithValue("@LocationId", searchModel.locationId);
                }
                if (searchModel.departmentId != 0)
                {
                    sqlcomm.Parameters.AddWithValue("@DepartmentId", searchModel.departmentId);
                }

                SqlDataReader sdr = sqlcomm.ExecuteReader();

                List<JobsSearchModel> jobs = new List<JobsSearchModel>();
                while (sdr.Read())
                {
                    JobsSearchModel JobDtls = new JobsSearchModel();
                    JobDtls.id = Convert.ToInt64( sdr["id"]);
                    JobDtls.code = sdr["code"].ToString();
                    JobDtls.title = sdr["title"].ToString();
                    JobDtls.location = sdr["location"].ToString();
                    JobDtls.department = sdr["department"].ToString();
                    JobDtls.postedDate = sdr["postedDate"].ToString();
                    JobDtls.closingDate = sdr["closingDate"].ToString();
                    jobs.Add(JobDtls);
                }
                
                var validFilter = new SearchModel(searchModel.pageNo, searchModel.pageSize);
                var pagedData = jobs.ToList()
                .Skip((validFilter.pageNo - 1) * validFilter.pageSize)
                .Take(validFilter.pageSize).ToList();
                obj.data = pagedData;
                obj.total = jobs.Count();


                return obj;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        public async Task<JobsByIdModel> GetJobDtlsById(long jobId)
        {
            try
            {
                JobsByIdModel obj = new JobsByIdModel();
                LocationModel lm = new LocationModel();
                DepartmentModel dm = new DepartmentModel();
                SqlConnection sqlconn = new SqlConnection(_connectionString);
                SqlCommand sqlcomm = new SqlCommand("GET_JOBSDETAILS");
                sqlconn.Open();
                sqlcomm.Connection = sqlconn;
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@Id", jobId);

                SqlDataReader list = sqlcomm.ExecuteReader();

                while (list.Read())
                {
                    obj.id = list.GetInt64(0);
                    obj.code = list.GetString(1);
                    obj.title = list.GetString(2);
                    obj.description = list.GetString(3);
                    lm.id = list.GetInt64(4);
                    lm.title = list.GetString(5);
                    lm.city = list.GetString(6);
                    lm.state = list.GetString(7);
                    lm.country = list.GetString(8);
                    lm.zip = list.GetInt32(9);
                    dm.id = list.GetInt64(10);
                    dm.title = list.GetString(11);
                    obj.postedDate = list.GetString(12);
                    obj.closingDate = list.GetString(13);
                    obj.location = lm;
                    obj.department = dm;
                }
                return obj;

            }
            catch (Exception exp)
            {
                throw exp;
            }

        }

        public async Task<ResultDto> InsertLocation(LocationInsertModel location)
        {
            try
            {
                SqlConnection sqlconn = new SqlConnection(_connectionString);
                SqlCommand sqlcomm = new SqlCommand("Insert_Update_LocationDetails");
                sqlconn.Open();
                sqlcomm.Connection = sqlconn;
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@Title", location.title);
                sqlcomm.Parameters.AddWithValue("@City", location.city);
                sqlcomm.Parameters.AddWithValue("@State", location.state);
                sqlcomm.Parameters.AddWithValue("@Country", location.country);
                sqlcomm.Parameters.AddWithValue("@Zip", location.zip);
                SqlDataReader sdr = sqlcomm.ExecuteReader();

                return new ResultDto
                {
                    Result = true,
                    Details = "Success",
                    Status = System.Net.HttpStatusCode.Created,
                };

            }
            catch (Exception exp)
            {
                return new ResultDto
                {
                    Result = false,
                    Details = exp.Message,
                    Status = System.Net.HttpStatusCode.InternalServerError,
                };
            }
        }



        public async Task<ResultDto> UpdateLocation(int Id, LocationInsertModel location)
        {
            try
            {
                SqlConnection sqlconn = new SqlConnection(_connectionString);
                SqlCommand sqlcomm = new SqlCommand("Insert_Update_LocationDetails");
                sqlconn.Open();
                sqlcomm.Connection = sqlconn;
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@ID", Id);
                sqlcomm.Parameters.AddWithValue("@Title", location.title);
                sqlcomm.Parameters.AddWithValue("@City", location.city);
                sqlcomm.Parameters.AddWithValue("@State", location.state);
                sqlcomm.Parameters.AddWithValue("@Country", location.country);
                sqlcomm.Parameters.AddWithValue("@Zip", location.zip);
                SqlDataReader sdr = sqlcomm.ExecuteReader();

                return new ResultDto
                {
                    Result = true,
                    Details = "Success",
                    Status = System.Net.HttpStatusCode.Created,
                };

            }
            catch (Exception exp)
            {
                return new ResultDto
                {
                    Result = false,
                    Details = exp.Message,
                    Status = System.Net.HttpStatusCode.InternalServerError,
                };
            }
        }


        public async Task<List<LocationModel>> GetLocation()
        {
            try
            {
                SqlConnection sqlconn = new SqlConnection(_connectionString);
                SqlCommand sqlcomm = new SqlCommand("SELECT * FROM Job_Location_tbl");
                sqlconn.Open();
                sqlcomm.Connection = sqlconn;
                SqlDataReader sdr = sqlcomm.ExecuteReader();
                
                List<LocationModel> location = new List<LocationModel>();

                while (sdr.Read())
                {
                    LocationModel locationDtls = new LocationModel();
                    locationDtls.id = Convert.ToInt64(sdr["Location_Id"]);
                    locationDtls.title = sdr["Location_Title"].ToString();
                    locationDtls.city = sdr["Location_City"].ToString();
                    locationDtls.state = sdr["Location_State"].ToString();
                    locationDtls.country = sdr["Location_Country"].ToString();
                    locationDtls.zip = Convert.ToInt32(sdr["Location_Zipcode"]);
                    location.Add(locationDtls);
                }
                return location;

            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        public async Task<ResultDto> InsertDepartment(DepartmentInsertModel department)
        {
            try
            {
                SqlConnection sqlconn = new SqlConnection(_connectionString);
                SqlCommand sqlcomm = new SqlCommand("Insert_Update_Department");
                sqlconn.Open();
                sqlcomm.Connection = sqlconn;
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@Title", department.title);
                SqlDataReader sdr = sqlcomm.ExecuteReader();

                return new ResultDto
                {
                    Result = true,
                    Details = "Success",
                    Status = System.Net.HttpStatusCode.Created,
                };

            }
            catch (Exception exp)
            {
                return new ResultDto
                {
                    Result = false,
                    Details = exp.Message,
                    Status = System.Net.HttpStatusCode.InternalServerError,
                };
            }
        }



        public async Task<ResultDto> UpdateDepartment(int Id, DepartmentInsertModel department)
        {
            try
            {
                SqlConnection sqlconn = new SqlConnection(_connectionString);
                SqlCommand sqlcomm = new SqlCommand("Insert_Update_Department");
                sqlconn.Open();
                sqlcomm.Connection = sqlconn;
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@ID", Id);
                sqlcomm.Parameters.AddWithValue("@Title", department.title);
 
                SqlDataReader sdr = sqlcomm.ExecuteReader();

                return new ResultDto
                {
                    Result = true,
                    Details = "Success",
                    Status = System.Net.HttpStatusCode.Created,
                };

            }
            catch (Exception exp)
            {
                return new ResultDto
                {
                    Result = false,
                    Details = exp.Message,
                    Status = System.Net.HttpStatusCode.InternalServerError,
                };
            }
        }

        public async Task<List<DepartmentModel>> GetDepartment()
        {
            try
            {
                SqlConnection sqlconn = new SqlConnection(_connectionString);
                SqlCommand sqlcomm = new SqlCommand("SELECT * FROM Department_tbl");
                sqlconn.Open();
                sqlcomm.Connection = sqlconn;
                SqlDataReader sdr = sqlcomm.ExecuteReader();
                
                List<DepartmentModel> departments = new List<DepartmentModel>();

                while (sdr.Read())
                {
                    DepartmentModel departmentDtls = new DepartmentModel();
                    departmentDtls.id = Convert.ToInt64(sdr["Dept_Id"]);
                    departmentDtls.title = sdr["Dept_Name"].ToString();
                    departments.Add(departmentDtls);
                }
                return departments;

            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
    }
}
