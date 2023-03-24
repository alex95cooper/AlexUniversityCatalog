using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexUniversityCatalog
{
    internal class FacultyRepository
    {
        private const string SelectFacultiesQuery = "SELECT * FROM Faculties ORDER BY {0}.{1} {2}";

        public static List<Faculty> GetFaculties(string connectionString, string tableName, string nameOrderBy, string sortingOrder)
        {
            List<Faculty> faculties = new();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                faculties = db.Query<Faculty>(string.Format(SelectFacultiesQuery, tableName, nameOrderBy, sortingOrder)).ToList();
            }

            return faculties;
        }

        public static DataTable GetTable(List<Faculty> entities, int offsetCount, int fetchRowsCount)
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

        private static void AddRows(DataTable dataTable, List<Faculty> entities, int i)
        {
            dataTable.Rows.Add(new object[] { entities[i].ID, entities[i].Name, entities[i].Description });
        }

        private static DataColumn[] GetColumns()
        {
            return new DataColumn[] { new("ID", typeof(int)), new("Name", typeof(string)), new("Description", typeof(string)) };
        }

        
    }
}
