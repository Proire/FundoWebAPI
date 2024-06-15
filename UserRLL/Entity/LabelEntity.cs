using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UserRLL.Entity
{
    public class LabelEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Label name is required.")]
        [StringLength(100, ErrorMessage = "Label name length can't be more than 100 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-]+$", ErrorMessage = "Label name can only contain letters, numbers, spaces, and hyphens.")]
        public string LableName { get; set; }   = string.Empty;

        [JsonIgnore]
        public ICollection<NoteLabelEntity> NoteLabels { get; set; } // Navigation property
    }
}
