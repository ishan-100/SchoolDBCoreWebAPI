using System.Text.Json.Serialization;

namespace SchoolDbCoreWbAPI.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public string RollNumber { get; set; }

        public int GradeId { get; set; }
        [JsonIgnore]
        public Grade Grade { get; set; }
    }
}