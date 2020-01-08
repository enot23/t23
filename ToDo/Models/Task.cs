using System;
using System.ComponentModel.DataAnnotations;

namespace ToDo.Models
{
    public class Task
    {
        public int Id { get; set; }

        [Display(Name = "Complete")]
        public bool Complete { get; set; } = false;

        [Required(ErrorMessage = "Field cannot be empty", AllowEmptyStrings = true)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Importance")]
        public Importance_Levels Importance_Level { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Due Date(optional)")]
        public DateTime? Due_Date { get; set; }

        [Display(Name = "List Id")]
        public int List_Id { get; set; }


    }
    public enum Importance_Levels
    {
        Normal=0,
        Low=-1,
        High=1
    }
}
