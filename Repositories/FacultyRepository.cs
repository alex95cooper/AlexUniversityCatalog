using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace AlexUniversityCatalog
{
    internal static class FacultyRepository
    {
        private const string SelectFacultiesQuery = "SELECT * FROM Faculties ORDER BY {0} {1} ";
        private const string InsertFacultyQuery = @"INSERT INTO Faculties (Name, Description) VALUES (@Name, @Description)";
        private const string UpdateFacultyQuery = @"UPDATE Faculties SET Name = @Name, Description = @Description WHERE ID = @ID";

        public static DataTable GetTable(string connectionString, string nameOrderBy, string sortingOrder, int offsetCount, int fetchRowsCount)
        {
            if (CheckIfNameValid(GetColunmNames(), nameOrderBy))
            {
                List<Faculty> faculties = GetFaculties(connectionString, nameOrderBy, sortingOrder);
                return GetTableWithColumns(faculties, offsetCount, fetchRowsCount);
            }

            return null;
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

        public static bool CheckIfNameValid(List<string> columnNames, string nameOrderBy)
        {
            foreach (string name in columnNames)
            {
                if (nameOrderBy == name)
                {
                    return true;
                }
            }

            return false;
        }

        private static List<Faculty> GetFaculties(string connectionString, string nameOrderBy, string sortingOrder)
        {
            List<Faculty> faculties = new();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                faculties = db.Query<Faculty>(string.Format(SelectFacultiesQuery, nameOrderBy, sortingOrder)).ToList();
            }

            return faculties;
        }

        private static DataTable GetTableWithColumns(List<Faculty> faculties, int offsetCount, int fetchRowsCount)
        {
            DataTable dataTable = new();
            dataTable.Columns.AddRange(GetColumns());
            fetchRowsCount = (offsetCount + fetchRowsCount > faculties.Count) ? faculties.Count - offsetCount : fetchRowsCount;
            for (int i = offsetCount; i < offsetCount + fetchRowsCount; i++)
            {
                AddRows(dataTable, faculties, i);
            }

            return dataTable;
        }

        public static void AddRows(DataTable dataTable, List<Faculty> entities, int i)
        {
            dataTable.Rows.Add(new object[] { entities[i].ID, entities[i].Name, entities[i].Description });
        }

        public static DataColumn[] GetColumns()
        {
            return new DataColumn[] { new("ID", typeof(int)), new("Name", typeof(string)), new("Description", typeof(string)) };
        }

        private static List<string> GetColunmNames()
        {
            return new() { "Faulties.ID", "Name", "Description" };
        }
    }
}
