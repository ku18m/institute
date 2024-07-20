using Microsoft.EntityFrameworkCore;

namespace Institute.Models
{
    public class InstituteContext: DbContext
    {
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Instructor> Instructors { get; set; }
        public virtual DbSet<Trainee> Trainees { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseResult> CourseResults { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-C2CGBOT\\SQLEXPRESS;Database=InstituteDB;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Instructor>().HasData(new Instructor() { Id = 1, Name = "Ali", ImageUrl = "Ins1.jpg", Salary = 5000, Address = "Cairo" });
            modelBuilder.Entity<Instructor>().HasData(new Instructor() { Id = 2, Name = "Amr", ImageUrl = "Ins2.jpg", Salary = 15488, Address = "Alex" });
            modelBuilder.Entity<Instructor>().HasData(new Instructor() { Id = 3, Name = "Ola", ImageUrl = "Ins3.jpg", Salary = 8000, Address = "Cairo" });
            base.OnModelCreating(modelBuilder);
        }
    }
}
