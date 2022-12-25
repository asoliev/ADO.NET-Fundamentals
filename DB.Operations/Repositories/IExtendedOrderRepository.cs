using DB.Operations.Models;

namespace DB.Operations.Repositories
{
    public interface IExtendedOrderRepository : IRepository<Order>
    {
        void Delete(
            int? month = null,
            OrderStatus? status = null,
            int? year = null,
            int? productId = null);

        IEnumerable<Order> GetAll(
            int? month = null,
            OrderStatus? status = null,
            int? year = null,
            int? productId = null);
    }
}