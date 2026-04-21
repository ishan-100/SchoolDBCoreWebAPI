using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using SchoolDbCoreWbAPI.Models;
using SchoolDbCoreWbAPI.Services;

namespace SchoolDbCoreWbAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [EnableCors]
    public class StudentController : ControllerBase
    {
        public StudentDAl stdDAL;
        public SchoolDbContext _Context;

        public StudentController(SchoolDbContext context)
        {
            _Context = context;
        }

        public class StudentCreateDTO
        {
            public string Name { get; set; }
            public string RollNumber { get; set; }
            public int GradeId { get; set; }
        }

        [HttpGet]
        public ActionResult<List<Student>> GetAllStudents()
        {
            stdDAL = new StudentDAl(_Context);
            return stdDAL.GetAllStudents();
        }

        [HttpGet("{stdId}")]
        public ActionResult<Student> GetStudentById(int stdId)
        {
            stdDAL = new StudentDAl(_Context);
            var student = stdDAL.GetStudentById(stdId);

            if (student == null)
                return NotFound();

            return Ok(student);
        }

        [HttpPost]
        public IActionResult AddStudent([FromBody] StudentCreateDTO dto)
        {
            if (dto == null)
                return BadRequest();

            var student = new Student
            {
                Name = dto.Name,
                RollNumber = dto.RollNumber,
                GradeId = dto.GradeId
            };

            stdDAL = new StudentDAl(_Context);
            stdDAL.AddStudent(student);

            return Ok("Student added successfully");
        }

        [HttpPut("{stdId}")]
        public IActionResult UpdateStudent(int stdId, StudentCreateDTO dto)
        {
            stdDAL = new StudentDAl(_Context);
            var student = stdDAL.GetStudentById(stdId);

            if (student == null)
                return NotFound();

            student.Name = dto.Name;
            student.RollNumber = dto.RollNumber;
            student.GradeId = dto.GradeId;

            stdDAL.UpdateStudent(student);
            return Ok("Student updated successfully");
        }

        [HttpDelete("{stdId}")]
        public IActionResult DeleteStudent(int stdId)
        {
            stdDAL = new StudentDAl(_Context);
            int result = stdDAL.DeleteStudent(stdId);

            if (result == 0)
                return NotFound("Student not found");

            return Ok("Student deleted successfully");
        }
    }
}