using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UserRLL.Entity
{
    public class NoteLabelEntity
    {
        public int NoteId { get; set; }

        [JsonIgnore]
        public NoteEntity Note { get; set; } // Inverse Navigation Property 
        public int LabelId { get; set; }

        public LabelEntity Label { get; set; }
    }
}
