namespace DB.Operations.Repositories
{
    public interface IRepository<T>
    {
        void Create(T entity);

        T Read(int id);

        void Update(T entity, int id);

        void Delete(int id);

        IEnumerable<T> GetAll();
    }
}