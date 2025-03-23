using System.ComponentModel.DataAnnotations;

namespace blazor.Models
{
    public class UpdateTaskDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(100, ErrorMessage = "Task title cannot be over 100 characters.")]
        public string Title { get; set; } = string.Empty;
        [Required(ErrorMessage = "Description is required.")]
        [MaxLength(300, ErrorMessage = "Task description cannot be over 300 characters")]
        public string Desc { get; set; } = string.Empty;
        [Required(ErrorMessage = "Priority level is required.")]
        public string Priority { get; set; } = string.Empty;
        [Required(ErrorMessage = "Task status is required.")]
        public string Status { get; set; } = string.Empty;
        [Required(ErrorMessage = "Start date is required.")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "End date is required.")]
        [DataType(DataType.Date)]
        [DateGreaterThan(nameof(StartDate), ErrorMessage = "End date must be later than start date.")]
        public DateTime EndDate { get; set; }
    }
}
