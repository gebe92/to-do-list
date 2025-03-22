using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class UpdateTaskDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Task title cannot be over 100 characters")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MaxLength(300, ErrorMessage = "Task description cannot be over 300 characters")]
        public string Desc { get; set; } = string.Empty;
        [Required]
        public string Priority { get; set; } = string.Empty;
        [Required]
        public string Status { get; set; } = string.Empty;
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
    }
}