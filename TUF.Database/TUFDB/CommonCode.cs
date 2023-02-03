

using System.ComponentModel.DataAnnotations;

namespace TUF.Database.TUFDB;

[Table("TblCommonCode")]
public partial class CommonCode : AudiTableEntity
{

    [MaxLength(20)]
    public string GroupCode { get; set; }
    [Required]
    [MaxLength(20)]
    public string Code { get; set; }

    [MaxLength(100)]
    public string Desc { get; set; }

}
