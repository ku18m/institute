using Institute.Models;

namespace Institute.Repository
{
    public class DepartmentRepository(InstituteContext _context) : IDepartmentRepo
    {
        private readonly InstituteContext context = _context;
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Department> GetAll()
        {
            return context.Departments.ToList();
        }

        public Department GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<Department> GetPage(int page)
        {
            throw new NotImplementedException();
        }

        public int GetTotalPages(int pageSize)
        {
            throw new NotImplementedException();
        }

        public void Insert(Department entity)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public List<Department> Search(string searchString, string searchProperty)
        {
            throw new NotImplementedException();
        }

        public void Update(Department entity)
        {
            throw new NotImplementedException();
        }
    }
}
