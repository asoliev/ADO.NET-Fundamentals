using DB.Operations.Models;
using DB.Operations.Repositories;

namespace Db.Operations.Tests
{
    public class OrderRepositoryTests
    {
        private readonly string _connectionString = App.ConectionString;

        private IExtendedOrderRepository _orderRepository;
        private Order _order;

        [OneTimeSetUp]
        public void Setup()
        {
            //IRepository<Product> _productRepository = new ProductRepository(_connectionString);
            //var _product = new Product
            //{
            //    Name = "ProductName1",
            //    Description = "ProductDescription1",
            //    Height = 101,
            //    Length = 102,
            //    Weight = 103,
            //    Width = 104
            //};
            //_productRepository.Create(_product);

            _orderRepository = new OrderRepository(_connectionString);
            _order = new Order
            {
                CreatedDate = new DateTime(2022, 12, 24),
                UpdatedDate = new DateTime(2022, 12, 25),
                Status = OrderStatus.Arrived,
                ProductId = 1
            };
        }

        [Test]
        [Order(1)]
        public void Create_Order_InsertsOrderIntoDb()
        {
            var expected = _order;
            _orderRepository.Create(_order);
            var actual = _orderRepository.GetAll().Last();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Order(2)]
        public void Read_ValidId_ReturnsOrder()
        {
            var expectedOrder = _order;
            expectedOrder.Id = _orderRepository.GetAll().Last().Id;
            var actual = _orderRepository.Read(expectedOrder.Id);

            Assert.AreEqual(expectedOrder, actual);
        }

        [Test]
        [Order(3)]
        public void Read_NotValidId_ReturnsNull()
        {
            var actual = _orderRepository.Read(3);

            Assert.IsNull(actual);
        }

        [Test]
        [Order(4)]
        public void Update_Order_UpdatesOrderIntoDb()
        {
            var expectedOrder = _order;
            expectedOrder.Id = _orderRepository.GetAll().Last().Id;
            expectedOrder.UpdatedDate = _order.UpdatedDate + TimeSpan.FromHours(4);

            _orderRepository.Update(expectedOrder, expectedOrder.Id);
            var actual = _orderRepository.Read(expectedOrder.Id);

            Assert.AreEqual(expectedOrder, actual);
        }

        [Test]
        [Order(5)]
        public void Delete_ValidId_DeletesOrderFromDb()
        {
            var id = _orderRepository.GetAll().Last().Id;

            _orderRepository.Delete(id: id);

            var actual = _orderRepository.Read(id);

            Assert.IsNull(actual);
        }
    }
}