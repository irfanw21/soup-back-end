using soup_back_end.Models;
using MySql.Data.MySqlClient;
using MimeKit.Encodings;
using Mysqlx.Crud;


namespace soup_back_end.Data
{
    public class InvoiceData
    {

        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private CartData _cartData;

        public InvoiceData(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            _cartData = new CartData(_configuration);
        }

        //getAll
        public List<Invoice> GetAll()
        {
            List<Invoice> invoice = new List<Invoice>();
            string query = $"SELECT * FROM invoice";
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
                                    invoice.Add(new Invoice
                                    {
                                        invoiceId = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                        paymentId = reader["courseId"].ToString() ?? string.Empty,
                                        userId = Guid.Parse(reader["categoryId"].ToString() ?? string.Empty),
                                        invoiceDate = Convert.ToDateTime(reader["invoiceDate"]),
                                        totalPaid = Convert.ToInt32(reader["course_price"]),
                                        itemCount = Convert.ToInt16(reader["itemCount"]),
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
                return invoice;
            }
        }

        //getByUserId
        public Invoice? GetByUserId(Guid userid)
        {
            Invoice? invoice = null;

            string query = $"SELECT * FROM invoice WHERE userId = @userid";

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
                            invoice = new Invoice
                            {
                                invoiceId = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                paymentId = reader["courseId"].ToString() ?? string.Empty,
                                userId = Guid.Parse(reader["categoryId"].ToString() ?? string.Empty),
                                invoiceDate = Convert.ToDateTime(reader["invoiceDate"]),
                                totalPaid = Convert.ToInt32(reader["course_price"]),
                                itemCount = Convert.ToInt16(reader["itemCount"]),

                            };
                        }
                    }
                }

                connection.Close();
            }
            return invoice;
        }

        public bool Insert(Invoice invoice)
        {
            bool result = false;

            int itemCount = _cartData.GetItemAmount(invoice.userId);
            int totalPaid = _cartData.GetTotalPrice(invoice.userId);
            
            string query = $"INSERT INTO invoice(paymentId, userId, invoiceDate, totalPaid, itemCount)" + $"VALUES(@paymentId, @userId, @invoiceDate, @totalPaid, @itemCount)"
                 + $"WHERE invoiceId FROM cart = NULL AND isSelected = TRUE" + $"SELECT LAST_INSERT_ID();";

            string invoiceDate = invoice.invoiceDate.Date.ToString("yyyy - MM - dd HH: mm:ss");

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@paymentId", invoice.paymentId);
                    command.Parameters.AddWithValue("@userId", invoice.userId);
                    command.Parameters.AddWithValue("@invoiceDate", invoice.invoiceDate);
                    command.Parameters.AddWithValue("@totalPaid", totalPaid);
                    command.Parameters.AddWithValue("@itemCount", itemCount);

                    connection.Open();

                    object updateInvoiceId = command.ExecuteScalar();

                    if (updateInvoiceId != null && updateInvoiceId != DBNull.Value)
                    {
                        Guid newInvoiceId = Guid.Parse(updateInvoiceId.ToString());

                        string updateCartQuery = @"UPDATE cart SET invoiceId = @newInvoiceId WHERE userId = @userId AND invoiceId IS NULL";

                        using (MySqlCommand updateCommand = new MySqlCommand(updateCartQuery, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@newInvoiceId", newInvoiceId);
                            updateCommand.Parameters.AddWithValue("@userId", invoice.userId);

                            result = command.ExecuteNonQuery() > 0 ? true : false; 
                        }
                    }

                    connection.Close();
                }
            }

            return result;
        }

        public bool Delete(string invoiceId)
        {
            bool result = false;

            string query = $"DELETE FROM invoice WHERE invoiceId = '{invoiceId}'";

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
