using Microsoft.AspNetCore.Mvc;
using SchoolDbCoreWbAPI.Models;
using SchoolDbCoreWbAPI.Services;

namespace SchoolDbCoreWbAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeController : ControllerBase
    {
        private readonly GradeDAL _gradeDAL;

        public GradeController(GradeDAL gradeDAL)
        {
            _gradeDAL = gradeDAL;
        }

        [HttpGet("GetAllGrades")]
        public IActionResult GetAllGrades()
        {
            return Ok(_gradeDAL.GetAllGrades());
        }

        [HttpGet("GetGradeById/{id}")]
        public IActionResult GetGradeById(int id)
        {
            var grade = _gradeDAL.GetGradeById(id);

            if (grade == null)
                return NotFound();

            return Ok(grade);
        }

        [HttpPost("AddGrade")]
        public IActionResult AddGrade([FromBody] Grade grade)
        {
            _gradeDAL.AddGrade(grade);
            return Ok("Grade added successfully");
        }

        [HttpPut("UpdateGrade")]
        public IActionResult UpdateGrade([FromBody] Grade grade)
        {
            _gradeDAL.UpdateGrade(grade);
            return Ok("Grade updated successfully");
        }

        [HttpDelete("DeleteGrade/{id}")]
        public IActionResult DeleteGrade(int id)
        {
            _gradeDAL.DeleteGrade(id);
            return Ok("Grade deleted successfully");
        }
    }
}
