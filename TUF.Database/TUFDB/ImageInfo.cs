using Daniel.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TUF.Database.TUFDB;

[Table("TblImageInfo")]
public partial class ImageInfo : AudiTableEntity
{
    [MaxLength(10)]
    public string Bkey { get; set; }
    public int BoardId { get; set; }
    [MaxLength(500)]
    public string ImagePath { get; set; }
    [MaxLength(100)]
    public string ImageName { get; set; }

    public string ImageTag { get; set; }

    public int? ImageSize { get; set; }
    public int? SortNum { get; set; }
    [MaxLength(30)]
    public string ContainerName { get; set; }
    public DateTime ExprireDate { get; set; }
    public bool UseYn { get; set; }
}
