using soup_back_end.Models;
using MySql.Data.MySqlClient;
using MimeKit.Encodings;
using Mysqlx.Crud;


namespace soup_back_end.Data
{
    public class PaymentMethodsData
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public PaymentMethodsData(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public List<PaymentMethods> GetAll()
        {
            List<PaymentMethods> paymentMethods = new List<PaymentMethods>();
            string query = $"SELECT * FROM payment_methods";
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
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
                                {
                                    paymentMethods.Add(new PaymentMethods
                                    {
                                        paymentId = reader["paymentId"].ToString() ?? string.Empty,
                                        paymentName = reader["paymentName"].ToString() ?? string.Empty,
                                        paymentImg = reader["paymentImg"].ToString() ?? string.Empty,
                                        
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
                return paymentMethods;
            }
        }

        //GetbyId
        public PaymentMethods? GetById(string paymentId)
        {
            PaymentMethods? paymentMethods = null;

            string query = $"SELECT * FROM payment_method WHERE paymentId = @paymentid";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@paymentId", paymentId);

                    connection.Open();


                    using (MySqlDataReader reader = command.ExecuteReader())

                    {
                        while (reader.Read())
                        {
                            paymentMethods = new PaymentMethods
                            {
                                paymentId = reader["paymentId"].ToString() ?? string.Empty,
                                paymentName = reader["paymentName"].ToString() ?? string.Empty,
                                paymentImg = reader["paymentImg"].ToString() ?? string.Empty,
                            };
                        }
                    }
                }

                connection.Close();
            }

            return paymentMethods;
        }

        public bool Insert(PaymentMethods paymentMethods)
        {
            bool result = false;

            string query = $"INSERT INTO payment_methods(paymentId, paymentName, paymentImg)" + $"VALUES(@paymentId, @paymentName, @paymentImg)";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@paymentId", paymentMethods.paymentId);
                    command.Parameters.AddWithValue("@paymentName", paymentMethods.paymentName);
                    command.Parameters.AddWithValue("@paymentImg", paymentMethods.paymentImg);
                    connection.Open();

                    result = command.ExecuteNonQuery() > 0 ? true : false;

                    connection.Close();
                }
            }

            return result;
        }

        public bool Delete(string paymentId)
        {
            bool result = false;

            string query = $"DELETE FROM payment_methods WHERE paymentId = '{paymentId}'";

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
