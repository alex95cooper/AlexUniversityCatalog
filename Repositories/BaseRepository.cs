using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexUniversityCatalog
{
    internal abstract class BaseRepository<T>
    {
        public DataTable GetTable(List<T> entities, int offsetCount, int fetchRowsCount)
        {
            DataTable dataTable = new();
            dataTable.Columns.AddRange(GetColumns());
            fetchRowsCount = (offsetCount + fetchRowsCount > entities.Count) ? entities.Count - offsetCount : fetchRowsCount;
            for (int i = offsetCount; i < offsetCount + fetchRowsCount; i++)
            {
                AddRows(dataTable, entities, i);
            }

            return dataTable;
        }

        internal virtual DataColumn[] GetColumns()
        {
            return new DataColumn[] { new DataColumn() };
        }

        protected virtual void AddRows(DataTable datatable, List<T> entities, int i) { }
    }
}
