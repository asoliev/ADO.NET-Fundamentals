using DB.Operations.Models;
using System.Data;
using System.Data.SqlClient;

namespace DB.Operations.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        private const string SelectQuery = "SELECT * FROM dbo.Product;";
        private readonly string _connectionString;

        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Create(Product product)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var adapter = new SqlDataAdapter(SelectQuery, connection))
                {
                    using (var ds = new DataSet())
                    {
                        adapter.Fill(ds);

                        var dt = ds.Tables[0];
                        var row = dt.NewRow();
                        row["Name"] = product.Name;
                        row["Description"] = product.Description;
                        row["Height"] = product.Height;
                        row["Width"] = product.Width;
                        row["Length"] = product.Length;
                        row["Weight"] = product.Weight;
                        dt.Rows.Add(row);

                        adapter.InsertCommand = new SqlCommandBuilder(adapter).GetInsertCommand();
                        adapter.Update(ds);
                    }
                }
                connection.Close();
            }
        }

        public Product Read(int id)
        {
            SqlDataReader reader;
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT Id, Name, Description, Weight, Height, Width, Length" +
                            " FROM dbo.Product" +
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

            Product product = null;
            while (reader.Read())
            {
                product = new Product
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.GetString(2),
                    Weight = reader.GetInt32(3),
                    Height = reader.GetInt32(4),
                    Width = reader.GetInt32(5),
                    Length = reader.GetInt32(6)
                };
            }
            return product;
        }

        public void Update(Product entity, int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var adapter = new SqlDataAdapter(SelectQuery, connection))
                {
                    using (var ds = new DataSet())
                    {
                        adapter.Fill(ds);

                        var dt = ds.Tables[0];
                        var row = dt.AsEnumerable().Single(x => x.Field<int>("Id") == id);
                        row["Name"] = entity.Name;
                        row["Description"] = entity.Description;
                        row["Weight"] = entity.Weight;
                        row["Height"] = entity.Height;
                        row["Width"] = entity.Width;
                        row["Length"] = entity.Length;

                        adapter.UpdateCommand = new SqlCommandBuilder(adapter).GetUpdateCommand();
                        adapter.Update(ds);
                    }
                }
                connection.Close();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var adapter = new SqlDataAdapter(SelectQuery, connection))
                {
                    using (var ds = new DataSet())
                    {
                        adapter.Fill(ds);

                        var dt = ds.Tables[0];
                        var row = dt.AsEnumerable().Single(x => x.Field<int>("Id") == id);
                        row.Delete();

                        adapter.DeleteCommand = new SqlCommandBuilder(adapter).GetDeleteCommand();
                        adapter.Update(ds);
                    }
                }
                connection.Close();
            }
        }

        public IEnumerable<Product> GetAll()
        {
            var products = new List<Product>();
            SqlDataReader reader;
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT Id, Name, Description, Weight, Height, Width, Length" +
                            " FROM dbo.Product;";

                connection.Open();
                var command = new SqlCommand(query, connection);
                reader = command.ExecuteReader();
                connection.Close();
            }
            if (!reader.HasRows) return products;

            while (reader.Read())
            {
                var product = new Product
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.GetString(2),
                    Weight = reader.GetInt32(3),
                    Height = reader.GetInt32(4),
                    Width = reader.GetInt32(5),
                    Length = reader.GetInt32(6)
                };

                products.Add(product);
            }

            return products;
        }
    }
}