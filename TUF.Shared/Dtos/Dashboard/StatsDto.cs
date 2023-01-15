using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUF.Shared.Dtos.Dashboard
{
    public class StatsDto
    {
        public int ProductCount { get; set; }
        public int BrandCount { get; set; }
        public int UserCount { get; set; }
        public int RoleCount { get; set; }
        //public List<ChartSeries> DataEnterBarChart { get; set; } = new();
        public Dictionary<string, double>? ProductByBrandTypePieChart { get; set; }
    }
}
