using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace AlexUniversityCatalog
{
    internal class FacultyRepository
    {
        private const string SelectFacultiesQuery = "SELECT * FROM Faculties ORDER BY {0} {1} ";
        private const string InsertFacultyQuery = @"INSERT INTO Faculties (Name, Description) VALUES (@Name, @Description)";
        private const string UpdateFacultyQuery = @"UPDATE Faculties SET Name = @Name, Description = @Description WHERE ID = @ID";

        public static DataTable GetTable(string connectionString, string nameOrderBy, string sortingOrder, int offsetCount, int fetchRowsCount)
        {
            DataTable dataTable = new();
            List<Faculty> faculties = GetFaculties(connectionString, nameOrderBy, sortingOrder);
            dataTable.Columns.AddRange(GetColumns());
            fetchRowsCount = (offsetCount + fetchRowsCount > faculties.Count) ? faculties.Count - offsetCount : fetchRowsCount;
            for (int i = offsetCount; i < offsetCount + fetchRowsCount; i++)
            {
                AddRows(dataTable, faculties, i);
            }

            return dataTable;
        }

        public static List<Faculty> GetFaculties(string connectionString, string nameOrderBy, string sortingOrder)
        {
            List<Faculty> faculties = new();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                faculties = db.Query<Faculty>(string.Format(SelectFacultiesQuery, nameOrderBy, sortingOrder)).ToList();
            }

            return faculties;
        }

        public static void Insert(Faculty faculty, string connectionString)
        {
            using IDbConnection db = new SqlConnection(connectionString);
            db.Execute(InsertFacultyQuery, new { faculty.Name, faculty.Description });
        }

        public static void Update(DataRowView rowData, string connectionString)
        {
            using IDbConnection db = new SqlConnection(connectionString);
            db.Execute(UpdateFacultyQuery, new { Name = rowData["Name"], Description = rowData["Description"], ID = rowData["ID"] });
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
