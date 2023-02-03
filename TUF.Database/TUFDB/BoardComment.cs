

using System.ComponentModel.DataAnnotations;

namespace TUF.Database.TUFDB;

[Table("TblBoardComment")]
public class BoardComment : AudiTableEntity
{
    [Required]
    [MaxLength(10)]
    public string Bkey { get; set; }
    [Required]
    public int BoardId { get; set; }
    [MaxLength(500)]
    public string Comment { get; set; }
    public bool? UseYn { get; set; }

    public int GrpNo { get; set; }
    public int GrpOrd { get; set; }
    public int Depth { get; set; }

    [MaxLength(30)]
    public string UserIpAddr { get; set; }
}
