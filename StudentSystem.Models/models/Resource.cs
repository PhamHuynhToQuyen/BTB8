using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToQuyenBTT8.Models
{
    public enum ResourceType
    {
        Video,
        Presentation,
        Document,
        Other
    }

    public class Resource
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public ResourceType Type { get; set; }

        [Required]
        [Url]
        public string Url { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
