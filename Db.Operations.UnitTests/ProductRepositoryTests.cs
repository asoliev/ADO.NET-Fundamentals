using DB.Operations.Models;
using DB.Operations.Repositories;

namespace Db.Operations.Tests
{
    public class ProductRepositoryTests
    {
        private readonly string _connectionString = App.ConectionString;

        private IRepository<Product> _productRepository;
        private Product _product;

        [OneTimeSetUp]
        public void Setup()
        {
            _productRepository = new ProductRepository(_connectionString);
            _product = new Product
            {
                Name = "ProductName1",
                Description = "ProductDescription1",
                Height = 101,
                Length = 102,
                Weight = 103,
                Width = 104
            };
        }

        [Test]
        [Order(1)]
        public void Create_Product_InsertsProductIntoDB()
        {
            var expected = _product;

            _productRepository.Create(_product);
            var actual = _productRepository.GetAll().Last();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Order(2)]
        public void Read_ValidId_ReturnsProduct()
        {
            var expectedProduct = _product;
            expectedProduct.Id = _productRepository.GetAll().Last().Id;

            var actual = _productRepository.Read(expectedProduct.Id);

            Assert.AreEqual(expectedProduct, actual);
        }

        [Test]
        [Order(3)]
        public void Read_NotValidId_ReturnsNull()
        {
            var actual = _productRepository.Read(10);

            Assert.IsNull(actual);
        }

        [Test]
        [Order(4)]
        public void Update_Product_UpdateProductInDB()
        {
            var expectedProduct = _product;
            expectedProduct.Id = _productRepository.GetAll().Last().Id;
            expectedProduct.Description = "new product description";

            _productRepository.Update(expectedProduct, expectedProduct.Id);

            var actual = _productRepository.Read(expectedProduct.Id);

            Assert.AreEqual(expectedProduct, actual);
        }

        [Test]
        [Order(5)]
        public void Delete_ValidId_DeleteProductInDB()
        {
            var id = _productRepository.GetAll().Last().Id;
            _productRepository.Delete(id);

            var actual = _productRepository.Read(id);

            Assert.IsNull(actual);
        }
    }
}