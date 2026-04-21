using SchoolDbCoreWbAPI.Models;
namespace SchoolDbCoreWbAPI.Services
{
    public class GradeDAL
    {
        private readonly SchoolDbContext _context;
        public GradeDAL(SchoolDbContext context)
        {
            _context = context;
        }

        public List<Grade> GetAllGrades()
        {
            return _context.Grades.ToList();
        }

        public Grade? GetGradeById(int grdId)
        {
            return _context.Grades
                .FirstOrDefault(g => g.GradeId == grdId);
        }

        public void AddGrade(Grade grade)
        {
            _context.Grades.Add(grade);
            _context.SaveChanges();
        }

       public void UpdateGrade(Grade grade)
        {
            _context.Grades.Update(grade);
            _context.SaveChanges();
        }

        public void DeleteGrade(int id)
        {
            var grade = _context.Grades.FirstOrDefault(g => g.GradeId == id);
            if(grade != null)
            {
                _context.Grades.Remove(grade);
                _context.SaveChanges();
            }

        }
    }
}
