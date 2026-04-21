using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolDbCoreWbAPI.Models
{
    public class Grade
    {
        public int GradeId { get; set; }

        [StringLength(25)]
        public string Section { get; set; } = null!;

        public string? Description { get; set; }

        public virtual ICollection<Student>? Students { get; set; }
    }
}