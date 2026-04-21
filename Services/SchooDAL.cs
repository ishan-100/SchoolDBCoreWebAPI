using Microsoft.EntityFrameworkCore;
using SchoolDbCoreWbAPI.Models;

namespace SchoolDbCodeWbAPI.Services
{
    public class SchoolDAL
    {
        private readonly SchoolDbContext Context;

        public SchoolDAL(SchoolDbContext context)
        {
            Context = context;
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

        public List<Grade> GetAllGradesWithStudents()
        {
            return Context.Grades
                .OrderBy(g => g.Section)
                .Include(g => g.Students)
                .ToList();
        }

        public List<Student> GetAllStudentsWithGrade()
        {
            return Context.Students
                .OrderBy(s => s.Name)
                .Include(s => s.Grade)   
                .ToList();
        }

        public int DeleteStudent(int stdId)
        {
            Student std = Context.Students.FirstOrDefault(s => s.StudentId == stdId);

            if (std == null)
                return 0;

            Context.Entry(std).State = EntityState.Deleted;
            return Context.SaveChanges();
        }

        public List<StdWithGradeDTO> GetAllStudentsDTO()
        {
            return Context.Students
                .OrderBy(s => s.Name)
                .Include(s => s.Grade)  
                .Select(s => new StdWithGradeDTO
                {
                    StudentId = s.StudentId,
                    StudentName = s.Name,
                    GrdDescription = s.Grade.Description,
                    GrdSection = s.Grade.Section
                })
                .ToList();
        }
    }
}