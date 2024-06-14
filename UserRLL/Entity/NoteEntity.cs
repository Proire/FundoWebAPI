using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UserRLL.Entity
{
    public class NoteEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title length can't be more than 100 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Title can only contain letters, numbers, and spaces.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description length can't be more than 500 characters.")]
        public string Description { get; set; } = string.Empty;

        public bool IsDeleted { get; set; } = false;

        public bool IsTrashed { get; set; } = false;

        public bool IsArchived { get; set; } = false;

        [ForeignKey("UserEntity")]
        public int? UserEntityId { get; set; }

        public override string ToString()
        {
            return $"{Title} : {Description}";
        }

        public UserEntity UserEntity { get; set; }   // Inverse Navigation Property 

        [JsonIgnore]
        public ICollection<NoteLabelEntity> NoteLabels { get; set; } // Navigation property

    }
}
