using RegionWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RegionWebAPI.Data
{
    public class DataLayer
    {
        public SqlConnection SqlConnection { get; set; }

        public void InitializeConnection()
        {
            //Get connection string from web.config file  
            string strcon = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            //create new sqlconnection and connection to database by using connection string from web.config file  
            SqlConnection = new SqlConnection(strcon);
            SqlConnection.Open();
        }
        public void EndConnection()
        {
            try
            {
                SqlConnection.Close();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Employee> EmployeeRepo()
        {
            InitializeConnection();
            List<Employee> employees = new List<Employee>();
            try
            {
                using (SqlConnection)
                {
                    SqlCommand command = new SqlCommand(
                      "SELECT * FROM dbo.Employee",
                      SqlConnection);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        employees = MapEntityToEmployeeModel(reader);
                    }
                }
                return employees;
            }
            catch (Exception e)
            {
                return null;//write logs or smth
            }
            finally
            {
                EndConnection();
            }

        }
        public List<Region> RegionRepo()
        {
            List<Region> regions = new List<Region>();
            InitializeConnection();
            try
            {
                using (SqlConnection)
                {
                    SqlCommand command = new SqlCommand(
                      "SELECT * FROM dbo.Region",
                      SqlConnection);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        regions = MapEntityToRegionModel(reader);
                    }
                }
                return regions;
            }
            catch (Exception)
            {
                //log error
                return regions;
            }
            finally
            {
                EndConnection();
            }

        }

        public (bool, string) SaveRegion(Region region)
        {
            InitializeConnection();
            try
            {
                using (SqlConnection)
                {
                    var parentId = (region.ParentRegionId != null) ? ", " + region.ParentRegionId.ToString() : "";
                    SqlCommand command = new SqlCommand(
                      $"Insert into dbo.Region values ( '{region.RegionName}' {(parentId)} )",
                      SqlConnection);

                    SqlDataReader reader = command.ExecuteReader();

                }
                return (true, "");
            }
            catch (Exception e)
            {
                //log error
                return (false, e.Message);

            }
            finally
            {
                EndConnection();
            }

        }


        public (bool, string) SaveEmployee(Employee employee)
        {
            InitializeConnection();
            try
            {
                using (SqlConnection)
                {
                    SqlCommand command = new SqlCommand(
                      $"Insert into dbo.Employee values ('{employee.FirstName}' ,'{employee.LastName}' ,{employee.RegionId} )",
                      SqlConnection);

                    SqlDataReader reader = command.ExecuteReader();

                }
                return (true, "");
            }
            catch (Exception e)
            {
                //log error
                return (false, e.Message);

            }
            finally
            {
                EndConnection();
            }

        }
        private List<Employee> MapEntityToEmployeeModel(SqlDataReader reader)
        {
            var empList = new List<Employee>();

            while (reader.Read())
            {
                var emp = new Employee
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    FirstName = reader["FirstName"].ToString(),
                    LastName = reader["LastName"].ToString(),
                    RegionId = Convert.ToInt32(reader["RegionId"])
                };
                empList.Add(emp);
            }
            reader.Close();
            return empList;
        }

        private List<Region> MapEntityToRegionModel(SqlDataReader reader)
        {
            var regList = new List<Region>();

            while (reader.Read())
            {
                var emp = new Region
                {
                    RegionId = Convert.ToInt32(reader["RegionId"]),
                    RegionName = reader["RegionName"].ToString(),
                    ParentRegionId = Convert.ToInt32(reader["ParentRegionId"])
                };
                regList.Add(emp);
            }
            return regList;
        }
    }
}