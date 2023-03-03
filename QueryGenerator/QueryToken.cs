using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexUniversityCatalog
{
    internal enum QueryToken
    {
        ColumnsName = 1,
        NameOrderBy = 3,
        SortingOrder = 4,
        OffsetCount = 6,
        FetchRowsCount = 8
    }
}
