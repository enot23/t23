using System.ComponentModel.DataAnnotations;

namespace ToDo.Models
{
    public class SmartList
{
        public int Id { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "Field cannot be empty", AllowEmptyStrings = true)]
        public string Title { get; set; }

        public bool HideCompleted { get; set; } = true;
    }
}