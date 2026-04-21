using Microsoft.EntityFrameworkCore;
using SchoolDbCodeWbAPI.Services;
using SchoolDbCoreWbAPI.Models;

namespace SchoolDbCoreWbAPI.Services
{
    public class StudentDAl
    {
        public SchoolDbContext Context;

        public StudentDAl(SchoolDbContext context)
        {
            Context = context;
        }

        public StudentDAl()
        {
        }

        public int AddStudent(Student std)
        {
            Context.Students.Add(std);
            return Context.SaveChanges();
        }
        public int UpdateStudent(Student std)
        {
            Context.Entry(std).State = EntityState.Modified;
            return Context.SaveChanges();
        }

        public Student GetStudentById(int stdId)
        {
            return Context.Students.FirstOrDefault(s => s.StudentId == stdId);
        }
        public StdWithNameAndRoll GetStudentNameAndRollById(int stdId)
        {
            Student std = Context.Students.FirstOrDefault(s => s.StudentId == stdId);

            if (std == null)
                return null;

            return new StdWithNameAndRoll
            {
                StudentName = std.Name,
                RollNumber = std.RollNumber
            };
        }
        public int DeleteStudent(int stdId)
        {
            Student std = Context.Students.FirstOrDefault(s => s.StudentId == stdId);

            if (std == null)
                return 0;

            Context.Entry(std).State = EntityState.Deleted;
            return Context.SaveChanges();
        }

        public List<Student> GetAllStudents()
        {
            List<Student> allStudents;

            try
            {
                return Context.Students.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return allStudents;
        }
    }
}
