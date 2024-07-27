namespace Institute.Repository
{
    public interface IRepository<T>
    {
        public List<T> GetAll();
        public List<T> GetPage(int page);
        public int GetTotalPages(int pageSize);
        public List<T> Search(string searchString, string searchProperty);
        public T GetById(int id);
        public void Insert(T entity);
        public void Update(T entity);
        public void Delete(int id);
        public void Save();
    }
}
