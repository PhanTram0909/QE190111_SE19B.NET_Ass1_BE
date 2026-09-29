namespace TaskTrack.Repo.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        System.Threading.Tasks.Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
