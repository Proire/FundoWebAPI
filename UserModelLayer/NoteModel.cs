using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserModelLayer
{
    public class NoteModel
    {
        [DefaultValue("Introduction to Cricket")]
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title length can't be more than 100 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Title can only contain letters, numbers, and spaces.")]
        public string Title { get; set; } = string.Empty;

        [DefaultValue("Cricket is a popular sport played with a bat and ball between two teams of eleven players each on a field. The game originated in England and is widely played across the world.")]
        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description length can't be more than 500 characters.")]
        public string Description { get; set; } = string.Empty;
    }
}
