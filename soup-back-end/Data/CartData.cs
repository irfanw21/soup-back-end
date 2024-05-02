using soup_back_end.Models;
using MySql.Data.MySqlClient;
using MimeKit.Encodings;
using Mysqlx.Crud;

namespace soup_back_end.Data
{
    public class CartData
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public CartData(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        //getAll
        public List<Cart> GetAll()
        {
            List<Cart> cart = new List<Cart>();
            string query = $"SELECT\r\n\tcr.cartId,\r\n\tcr.userId,\r\n\tcr.courseId,\r\n\tcr.categoryId,\r\n\tcr.scheduleId,\r\n\tco.course_Name,\r\n\tcs.scheduleDate\r\nFROM cart cr\r\nJOIN course co ON co.Id = cr.courseId\r\nJOIN course_schedule cs ON cs.scheduleId = cr.scheduleId\r\nORDER BY cr.cartId"; 
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read()) {
                                {
                                    cart.Add(new Cart
                                    {
                                        cartId = Guid.Parse(reader["cartId"].ToString() ?? string.Empty),
                                        courseId = reader["courseId"].ToString() ?? string.Empty,
                                        categoryId = reader["categoryId"].ToString() ?? string.Empty,
                                        userId = Guid.Parse(reader["userId"].ToString() ?? string.Empty),
                                        scheduleId = reader["scheduleId"].ToString() ?? string.Empty,
                                    });
                                } 
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
                return cart;
            }
        }

        //GetbyId
        public Cart? GetById(Guid cartid)
        {
            Cart? cart = null;

            string query = $"SELECT * FROM users WHERE cartId = @cartid";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@cartid", cartid);

                    connection.Open();


                    using (MySqlDataReader reader = command.ExecuteReader())

                    {
                        while (reader.Read())
                        {
                            cart = new Cart
                            {
                                cartId = Guid.Parse(reader["cartId"].ToString() ?? string.Empty),
                                courseId = reader["courseId"].ToString() ?? string.Empty,
                                categoryId = reader["categoryId"].ToString() ?? string.Empty,
                                userId = Guid.Parse(reader["userId"].ToString() ?? string.Empty),
                                scheduleId = reader["scheduleId"].ToString() ?? string.Empty,

                            };
                        }
                    }
                }

                connection.Close();
            }

            return cart;
        }

        //getByUserId
        public Cart? GetByUserId(Guid userid)
        {
            Cart? cart = null;

            string query = $"SELECT * FROM users WHERE userId = @userid";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@userid", userid);

                    connection.Open();


                    using (MySqlDataReader reader = command.ExecuteReader())

                    {
                        while (reader.Read())
                        {
                            cart = new Cart
                            {
                                cartId = Guid.Parse(reader["cartId"].ToString() ?? string.Empty),
                                userId = Guid.Parse(reader["userId"].ToString() ?? string.Empty),
                                courseId = reader["courseId"].ToString() ?? string.Empty,
                                categoryId = reader["categoryId"].ToString() ?? string.Empty,
                                scheduleId = reader["scheduleId"].ToString() ?? string.Empty,

                            };
                        }
                    }
                }

                connection.Close();
            }
            return cart;
        }

        public bool Insert(Cart cart)
        {
            bool result = false;

            string query = $"INSERT INTO cart(cartId, courseId, categoryId, userId, scheduleId)" + $"VALUES(@cartId, @courseId, @categoryId, @userId, @scheduleId)";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@cartId", cart.cartId);
                    command.Parameters.AddWithValue("@coursetId", cart.courseId);
                    command.Parameters.AddWithValue("@categoryId", cart.categoryId);
                    command.Parameters.AddWithValue("@usertId", cart.userId);
                    command.Parameters.AddWithValue("@scheduleId", cart.scheduleId);

                    connection.Open();

                    result = command.ExecuteNonQuery() > 0 ? true : false;

                    connection.Close();
                }
            }

            return result;
        }

        public bool Delete(string cartId)
        {
            bool result = false;

            string query = $"DELETE FROM cart WHERE cartId = '{cartId}'";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
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
