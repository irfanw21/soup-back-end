using soup_back_end.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;

namespace soup_back_end.Data
{
    public class CategoryData
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;

        public CategoryData(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        //getAll
        public List<Category>GetAll()
        {
            List<Category> categories = new List<Category>();
            string query = "SELECT * FROM category";
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
                                categories.Add(new Category
                                {
                                    category_id = reader["category_id"].ToString() ?? string.Empty,
                                    category_name = reader["category_Name"].ToString() ?? string.Empty,
                                    Description = reader["Description"].ToString(),
                                    img = reader["img"].ToString() ?? string.Empty,
                                    Created = Convert.ToDateTime(reader["Created"]),
                                    Updated = Convert.ToDateTime(reader["Updated"]),
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
            return categories;
        }
        public Category? GetById(string category_id)
        {
            Category? category = null;

            string query = $"SELECT * FROM category WHERE category_id = @category_id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@category_id", category_id);

                    connection.Open();


                    using (MySqlDataReader reader = command.ExecuteReader())

                    {
                        while (reader.Read())
                        {
                            category = new Category
                            {
                                category_id = reader["category_id"].ToString() ?? string.Empty,
                                category_name = reader["category_name"].ToString() ?? string.Empty,
                                Description = reader["Description"].ToString(),
                                img = reader["img"].ToString() ?? string.Empty,
                                Created = Convert.ToDateTime(reader["Created"]),
                                Updated = Convert.ToDateTime(reader["Updated"]),
                            };
                        }
                    }
                }

                connection.Close();
            }

            return category;
        }


        public Category? GetByName(string category_name)
        {
            Category? category = null;

            string query = $"SELECT * FROM category WHERE category_name = @category_name";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@category_name", category_name);

                    connection.Open();


                    using (MySqlDataReader reader = command.ExecuteReader())

                    {
                        while (reader.Read())
                        {
                            category = new Category
                            {
                                category_id = reader["category_id"].ToString() ?? string.Empty,
                                category_name = reader["category_name"].ToString() ?? string.Empty,
                                Description = reader["Description"].ToString(),
                                img = reader["img"].ToString() ?? string.Empty,
                                Created = Convert.ToDateTime(reader["Created"]),
                                Updated = Convert.ToDateTime(reader["Updated"]),
                            };
                        }
                    }
                }

                connection.Close();
            }

            return category;
        }

        public Course? GetCourseByCategoryId(string categoryId)
        {
            Course? course = null;

            string query = $"SELECT * FROM course WHERE category_name = @category_name";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@categoryId", categoryId);

                    connection.Open();


                    using (MySqlDataReader reader = command.ExecuteReader())

                    {
                        while (reader.Read())
                        {
                            course = new Course
                            {
                                Id = reader["Id"].ToString() ?? string.Empty,
                                categoryId = reader["categoryId"].ToString() ?? string.Empty,
                                course_Name = reader["course_Name"].ToString() ?? string.Empty,
                                Description = reader["Description"].ToString(),
                                img = reader["img"].ToString() ?? string.Empty,
                                course_price = Convert.ToInt32(reader["course_price"]),
                            };
                        }
                    }
                }

                connection.Close();
            }

            return course;
        }

        public bool Insert(Category category)
        {
            bool result = false;

            string created = category.Created.Date.ToString("yyyy-MM-dd HH:mm:ss");
            string updated = category.Updated.Date.ToString("yyyy-MM-dd HH:mm:ss");

            string query = $"INSERT INTO category(category_id, category_name, Description, img, Created, Updated) " + $"VALUES(@category_id, @category_name, @Description, @img, @Created, @Updated)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@category_id", category.category_id);
                    command.Parameters.AddWithValue("@category_name", category.category_name);
                    command.Parameters.AddWithValue("@Description", category.Description);
                    command.Parameters.AddWithValue("@img", category.img);
                    command.Parameters.AddWithValue("@Created", category.Created);
                    command.Parameters.AddWithValue("@Updated", category.Updated);

                    connection.Open();

                    result = command.ExecuteNonQuery() > 0 ? true : false;

                    connection.Close();
                }
            }

            return result;

        }

        public bool Update(string category_id, Category category)
        {
            bool result = false;

            string updated = category.Updated.Date.ToString("yyyy-MM-dd HH:mm:ss");

            string query = $"UPDATE category SET course_Name = @category_name, Description = @Description, img = @img,  Updated = @Updated " + $"WHERE category_id = @category_id";


            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@category_id", category.category_id);
                    command.Parameters.AddWithValue("@category_name", category.category_name);
                    command.Parameters.AddWithValue("@Description", category.Description);
                    command.Parameters.AddWithValue("@img", category.img);
                    command.Parameters.AddWithValue("@Updated", updated);

                    connection.Open();

                    result = command.ExecuteNonQuery() > 0 ? true : false;

                    connection.Close();
                }
            }

            return result;
        }

        public bool Delete(string category_id)
        {
            bool result = false;

            string query = $"DELETE FROM category WHERE category_id = '{category_id}'";

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
