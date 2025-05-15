using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToQuyenBTT8.Models
{
    public enum ContentType
    {
        Pdf,
        Zip
    }

    public class Homework
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public ContentType ContentType { get; set; }

        [Required]
        public DateTime SubmissionDate { get; set; }

        public int StudentId { get; set; }
        public int CourseId { get; set; }

        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}
