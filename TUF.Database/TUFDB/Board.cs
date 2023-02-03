


using System.ComponentModel.DataAnnotations;

namespace TUF.Database.TUFDB
{
    [Table("TblBoard")]
    public class Board: AudiTableEntity
    {
        [MaxLength(200)]
        public string Subject { get; set; }

        public string Contents { get; set; }

        public string ContentsHtml { get; set; }
        [MaxLength(10)]
        public string Bkey { get; set; }
        public int? ReadCount { get; set; }
        [MaxLength(30)]
        public string UserIpAddr { get; set; }
        [MaxLength(100)]
        public string BoardPassword { get; set; }
        public bool UseYn { get; set; }
    }
}
