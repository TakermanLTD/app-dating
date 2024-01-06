using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Takerman.Dating.Data
{
    public class UploadData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Description("Upload Data")]
        [DataType(DataType.Upload)]
        public byte[] Data { get; set; }

        [Description("Upload")]
        public Upload Upload { get; set; } = null;
    }
}