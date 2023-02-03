

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TUF.Database.TUFDB;

[Table("TblBoardInfo")]
public partial class BoardInfo : AudiTableEntity
{

    [Required]
    [MaxLength(10)]
    public string GroupCode { get; set; }
    [Required]
    [MaxLength(10)]
    public string Bkey { get; set; }

    [MaxLength(100)]
    public string BoardName { get; set; }

    [MaxLength(200)]
    public string BoardDesc { get; set; }
    public bool? EditorYn { get; set; }

    public bool? CommentYn { get; set; }
    public bool? ImageYn { get; set; }
    public DateTime? Expiredate { get; set; }
    public bool? UseYn { get; set; }
    public int sort { get; set; }
}
