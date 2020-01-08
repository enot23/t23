using System.ComponentModel.DataAnnotations;

namespace ToDo.Models
{
    public class CustomList
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Field cannot be empty", AllowEmptyStrings = true)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        public bool HideCompleted { get; set; } = true;
    }
}