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
            List<Invoice> invoices = new List<Invoice>();
            string query = "SELECT * FROM invoice";

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
                                invoices.Add(new Invoice
                                {
                                    invoiceId = Guid.Parse(reader["invoiceId"].ToString()),
                                    paymentId = reader["paymentId"].ToString(),
                                    userId = Guid.Parse(reader["userId"].ToString()),
                                    invoiceDate = Convert.ToDateTime(reader["invoiceDate"]),
                                    totalPaid = Convert.ToDecimal(reader["totalPaid"]),
                                    itemCount = Convert.ToInt32(reader["itemCount"])
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                        throw; // Rethrow the exception to indicate failure
                    }
                }
            }

            return invoices;
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

            // Generate a new GUID for invoiceId
            Guid newInvoiceId = Guid.NewGuid();

            string insertInvoiceQuery = @"
        INSERT INTO invoice(invoiceId, paymentId, userId, invoiceDate, totalPaid, itemCount)
        SELECT @invoiceId, @paymentId, @userId, @invoiceDate, SUM(co.course_price), COUNT(*)
        FROM cart cr
        INNER JOIN course co ON cr.courseId = co.Id
        WHERE cr.userId = @userId AND cr.invoiceId IS NULL AND cr.isSelected = 1
        GROUP BY cr.userId";

            string updateCartQuery = @"
        UPDATE cart
        SET invoiceId = @newInvoiceId
        WHERE userId = @userId AND invoiceId IS NULL AND isSelected = 1";

            string invoiceDate = invoice.invoiceDate.Date.ToString("yyyy-MM-dd HH:mm:ss");

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Insert into invoice table
                        using (MySqlCommand insertCommand = new MySqlCommand(insertInvoiceQuery, connection, transaction))
                        {
                            insertCommand.Parameters.AddWithValue("@invoiceId", newInvoiceId);
                            insertCommand.Parameters.AddWithValue("@paymentId", invoice.paymentId);
                            insertCommand.Parameters.AddWithValue("@userId", invoice.userId);
                            insertCommand.Parameters.AddWithValue("@invoiceDate", invoiceDate);

                            int rowsAffected = insertCommand.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                // Update cart table with the new invoiceId
                                using (MySqlCommand updateCommand = new MySqlCommand(updateCartQuery, connection, transaction))
                                {
                                    updateCommand.Parameters.AddWithValue("@newInvoiceId", newInvoiceId);
                                    updateCommand.Parameters.AddWithValue("@userId", invoice.userId);

                                    int updateRows = updateCommand.ExecuteNonQuery();

                                    if (updateRows > 0)
                                    {
                                        transaction.Commit();
                                        result = true;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions and rollback transaction if necessary
                        Console.WriteLine($"An error occurred: {ex.Message}");
                        transaction.Rollback();
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
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
