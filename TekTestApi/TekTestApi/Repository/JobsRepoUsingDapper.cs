using Dapper;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Threading.Tasks;
using TekTestApi.Context;
using TekTestApi.Interface;
using TekTestApi.Model;

namespace TekTestApi.Repository
{
    public class JobsRepoUsingDapper:IJobsRepoUsingDapper
    {
        private readonly DapperContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public JobsRepoUsingDapper(DapperContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ResultDto> InsertJobDetails(JobsModel jobDetails)
        {
            try
            {
                var parameters = new DynamicParameters();
                string Url=null;
                if (jobDetails != null)
                {
                    using (var connection = _context.CreateConnection())
                    {
                        parameters.Add("@Title", jobDetails.title, DbType.String);
                        parameters.Add("@Description", jobDetails.description, DbType.String);
                        parameters.Add("@LocationID", jobDetails.locationID, DbType.Int64);
                        parameters.Add("@DepartmentID", jobDetails.departmentID, DbType.Int64);
                        parameters.Add("@ClosingDate", jobDetails.closingDate, DbType.String);
                        var Id =  connection.ExecuteScalar("INSERT_JOBSDETAILS", parameters, commandType: CommandType.StoredProcedure);
                        Url = _httpContextAccessor.HttpContext.Request.GetDisplayUrl() + "/" + Id;
                    }

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
                var parameters = new DynamicParameters();
                if (jobDtlsUpd != null)
                {
                    using (var connection = _context.CreateConnection())
                    {
                        parameters.Add("@Id", jobId, DbType.Int64);
                        parameters.Add("@Title", jobDtlsUpd.title, DbType.String);
                        parameters.Add("@Description", jobDtlsUpd.description, DbType.String);
                        parameters.Add("@LocationID", jobDtlsUpd.locationID, DbType.Int64);
                        parameters.Add("@DepartmentID", jobDtlsUpd.departmentID, DbType.Int64);
                        parameters.Add("@ClosingDate", jobDtlsUpd.closingDate, DbType.String);
                         connection.Execute("INSERT_JOBSDETAILS", parameters, commandType: CommandType.StoredProcedure);
                    }

                }

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
                var parameters = new DynamicParameters();

                using (var connection = _context.CreateConnection())
                {
                    parameters.Add("@query", searchModel.q, DbType.String);
                    if (searchModel.locationId != 0)
                    {
                        parameters.Add("@LocationId", searchModel.locationId, DbType.Int64);
                    }
                    if (searchModel.departmentId != 0)
                    {
                        parameters.Add("@DepartmentId", searchModel.departmentId, DbType.Int64);
                    }
                   var data =await connection.QueryAsync<JobsSearchModel>("GET_JOBSDETAILS", parameters, commandType: CommandType.StoredProcedure);
                    var validFilter = new SearchModel(searchModel.pageNo, searchModel.pageSize);
                    var pagedData = data.ToList()
                    .Skip((validFilter.pageNo - 1) * validFilter.pageSize)
                    .Take(validFilter.pageSize).ToList();
                    obj.data = pagedData;
                    obj.total = data.Count();
                }

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
                var parameters = new DynamicParameters();

                using (var connection = _context.CreateConnection())
                {
                    parameters.Add("@Id", jobId, DbType.Int64);
                    var list = connection.ExecuteReader("GET_JOBSDETAILS", parameters, commandType: CommandType.StoredProcedure);
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
                var parameters = new DynamicParameters();
                using (var connection = _context.CreateConnection())
                {
                    parameters.Add("@Title", location.title, DbType.String);
                    parameters.Add("@City", location.city, DbType.String);
                    parameters.Add("@State", location.state, DbType.String);
                    parameters.Add("@Country", location.country, DbType.String);
                    parameters.Add("@Zip", location.zip, DbType.Int32);
                    connection.ExecuteScalar("Insert_Update_LocationDetails", parameters, commandType: CommandType.StoredProcedure);
                }


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
                var parameters = new DynamicParameters();
                using (var connection = _context.CreateConnection())
                {
                    parameters.Add("@ID", Id, DbType.Int32);
                    parameters.Add("@Title", location.title, DbType.String);
                    parameters.Add("@City", location.city, DbType.String);
                    parameters.Add("@State", location.state, DbType.String);
                    parameters.Add("@Country", location.country, DbType.String);
                    parameters.Add("@Zip", location.zip, DbType.Int32);
                    connection.ExecuteScalar("Insert_Update_LocationDetails", parameters, commandType: CommandType.StoredProcedure);
                }
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
                var query = "SELECT * FROM Job_Location_tbl";
                using (var connection = _context.CreateConnection())
                {

                    var parameters = await connection.QueryAsync<LocationModel>(query);
                    return (List<LocationModel>)parameters;

                }

            }
            catch (Exception exp)
            {
                throw new Exception(exp.Message, exp);
            }
        }


        public async Task<ResultDto> InsertDepartment(DepartmentInsertModel department)
        {
            try
            {
                var parameters = new DynamicParameters();
                using (var connection = _context.CreateConnection())
                {
                    parameters.Add("@Title", department.title, DbType.String);
                    connection.ExecuteScalar("Insert_Update_Department", parameters, commandType: CommandType.StoredProcedure);
                }
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
                var parameters = new DynamicParameters();
                using (var connection = _context.CreateConnection())
                {
                    parameters.Add("@ID", Id, DbType.Int32);
                    parameters.Add("@Title", department.title, DbType.String);
                    connection.ExecuteScalar("Insert_Update_Department", parameters, commandType: CommandType.StoredProcedure);
                }
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

                var query = "SELECT * FROM Department_tbl";
                using (var connection = _context.CreateConnection())
                {

                    var parameters = await connection.QueryAsync<DepartmentModel>(query);
                    return (List<DepartmentModel>)parameters;

                }


            }
            catch (Exception exp)
            {
                throw new Exception(exp.Message, exp);
            }
        }
    }
}
