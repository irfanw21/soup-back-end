using soup_back_end.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using MySqlX.XDevAPI.Common;

namespace soup_back_end.Data
{
    public class CourseScheduleData
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;

        public CourseScheduleData(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        //getAll
        public List<CourseSchedule> GetAll()
        {
            List<CourseSchedule> courseSchedule = new List<CourseSchedule>();
            string query = "SELECT * FROM course_schedule";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                courseSchedule.Add(new CourseSchedule
                                {
                                    scheduleId = reader["scheduleId"].ToString() ?? string.Empty,
                                    scheduleDate = DateTime.Parse(reader["scheduleDate"].ToString()),
                                    courseId = reader["courseId"].ToString() ?? string.Empty,
                                });
                            }
                        }
                    }

                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return courseSchedule;
        }

        //create Schedule
        public bool CreateSchedule(CourseSchedule courseSchedule)
        {
            bool result = false;

            string query = $"INSERT INTO course_schedule (scheduleId, scheduleDate, courseId) " + $"VALUES (@scheduleId, @scheduleDate, @courseId)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;    
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@scheduleId", courseSchedule.scheduleId);
                    command.Parameters.AddWithValue("@scheduleDate", courseSchedule.scheduleDate);
                    command.Parameters.AddWithValue("@courseId", courseSchedule.courseId);

                    connection.Open();

                    result = command.ExecuteNonQuery() > 0 ? true : false;

                    connection.Close();
                }
            }

            return result;
        }

        //update schedule
        public bool Update(string scheduleId, CourseSchedule courseSchedule)
        {
            bool result = false;

            string query = $"UPDATE course_schedule SET scheduleDate = @scheduleDate, courseId = @courseId" + $"WHERE scheduleId = @scheduleId";

            using (MySqlConnection connection = new MySqlConnection(connectionString)) 
            {
                using (MySqlCommand command = new MySqlCommand()) 
                {
                    command.Connection = connection;
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@scheduleId", courseSchedule.scheduleId);
                    command.Parameters.AddWithValue("@schduleDate", courseSchedule.scheduleDate);
                    command.Parameters.AddWithValue("@courseId", courseSchedule.courseId);

                    connection.Open();

                    result = command.ExecuteNonQuery() > 0 ? true : false;

                    connection.Close();
                }
            }

            return result;
        }

        public bool Delete(string scheduleId) 
        {
            bool result = false;
            string query = $"DELETE FROM course_schedule WHERE scheduleId = '{scheduleId}'";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = query;
                    connection.Open();

                    result = command.ExecuteNonQuery() > 0 ? true : false;

                    connection.Close();
                }
            }

            return result;
        }
    }
}
