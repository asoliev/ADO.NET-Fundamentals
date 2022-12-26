using DB.Operations.Models;
using System.Data;
using System.Data.SqlClient;

namespace DB.Operations.Repositories
{
    public class OrderRepository : IExtendedOrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Create(Order entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "INSERT INTO [dbo].[Order]" +
                            " (Status, CreatedDate, UpdatedDate, ProductId)" +
                            " VALUES (@Status, @CreatedDate, @UpdatedDate, @ProductId);";

                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Status", entity.Status);
                    command.Parameters.AddWithValue("@CreatedDate", entity.CreatedDate);
                    command.Parameters.AddWithValue("@UpdatedDate", entity.UpdatedDate);
                    command.Parameters.AddWithValue("@ProductId", entity.ProductId);

                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public Order Read(int id)
        {
            SqlDataReader reader;
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT Id, Status, CreatedDate, UpdatedDate, ProductId" +
                            " FROM [dbo].[Order]" +
                            " WHERE Id = @Id;";

                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    reader = command.ExecuteReader();
                }
                connection.Close();
            }
            if (!reader.HasRows) return null;

            Order order = null;
            while (reader.Read())
            {
                order = new Order
                {
                    Id = reader.GetInt32(0),
                    Status = (OrderStatus)reader.GetInt32(1),
                    CreatedDate = reader.GetDateTime(2),
                    UpdatedDate = reader.GetDateTime(3),
                    ProductId = reader.GetInt32(4)
                };
            }
            return order;
        }

        public void Update(Order entity, int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "UPDATE [dbo].[Order]" +
                            " SET Status = @Status," +
                            " CreatedDate = @CreatedDate," +
                            " UpdatedDate = @UpdatedDate" +
                            " WHERE Id = @Id;";

                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Status", entity.Status);
                    command.Parameters.AddWithValue("@CreatedDate", entity.CreatedDate);
                    command.Parameters.AddWithValue("@UpdatedDate", entity.UpdatedDate);

                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "DELETE FROM [dbo].[Order] WHERE Id = @Id;";

                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public void Delete(int? month = null, OrderStatus? status = null, int? year = null, int? productId = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction sqlTransaction = connection.BeginTransaction())
                {
                    const string deleteOrders = "DeleteOrders";
                    using (SqlCommand command = new SqlCommand(deleteOrders, connection))
                    {
                        command.Transaction = sqlTransaction;
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Month", month);
                        command.Parameters.AddWithValue("@Status", status);
                        command.Parameters.AddWithValue("@Year", year);
                        command.Parameters.AddWithValue("@ProductId", productId);

                        try
                        {
                            command.ExecuteScalar();
                            sqlTransaction.Commit();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            sqlTransaction.Rollback();
                            throw;
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
            }
        }

        public IEnumerable<Order> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> GetAll(int? month = null, OrderStatus? status = null, int? year = null, int? productId = null)
        {
            SqlDataReader reader;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                const string fetchOrders = "FetchOrders";
                using (var command = new SqlCommand(fetchOrders, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Month", month);
                    command.Parameters.AddWithValue("@Status", status);
                    command.Parameters.AddWithValue("@Year", year);
                    command.Parameters.AddWithValue("@ProductId", productId);

                    reader = command.ExecuteReader();
                    connection.Close();
                }
            }

            var orders = new List<Order>();
            if (!reader.HasRows) return null;

            while (reader.Read())
            {
                var order = new Order
                {
                    Id = reader.GetInt32(0),
                    Status = (OrderStatus)reader.GetInt32(1),
                    CreatedDate = reader.GetDateTime(2),
                    UpdatedDate = reader.GetDateTime(3),
                    ProductId = reader.GetInt32(4)
                };
                orders.Add(order);
            }

            return orders;
        }
    }
}