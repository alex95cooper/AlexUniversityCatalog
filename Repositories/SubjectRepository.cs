using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;


namespace AlexUniversityCatalog
{
    internal class SubjectRepository
    {
        private const string SelectSubjectsQuery = @"SELECT Subjects.*, Faculties.* FROM Subjects
                LEFT JOIN Faculties ON Faculties.ID = Subjects.FacID ORDER BY {0} {1}";
        private const string InsertSubjectQuery = @"INSERT INTO Subjects (FacID, Name, Description) 
                VALUES ((SELECT ID FROM Faculties WHERE Name = @FacultyName), @Name, @Description)";
        private const string UpdateSubjectQuery = @"UPDATE Subjects SET Name = @Name, Description = @Description, 
                FacID = (SELECT Faculties.ID FROM Faculties WHERE Faculties.Name = @Faculty) WHERE ID = @ID";

        public static List<Subject> GetSubjects(string connectionString, string nameOrderBy, string sortingOrder)
        {
            if (FacultyRepository.CheckIfNameValid(GetColunmNames(), nameOrderBy))
            {
                List<Subject> subjects = new();
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    subjects = db.Query<Subject, Faculty, Subject>(string.Format(SelectSubjectsQuery, nameOrderBy, sortingOrder),
                        (subject, faculty) => { subject.Faculty = faculty; return subject; }).ToList();
                }

                return subjects;
            }
            return null;
        }

        public static DataTable GetTable(string connectionString, string nameOrderBy, string sortingOrder, int offsetCount, int fetchRowsCount)
        {
            DataTable dataTable = new();
            List<Subject> subjects = GetSubjects(connectionString, nameOrderBy, sortingOrder);
            dataTable.Columns.AddRange(GetColumns());
            fetchRowsCount = (offsetCount + fetchRowsCount > subjects.Count) ? subjects.Count - offsetCount : fetchRowsCount;
            for (int i = offsetCount; i < offsetCount + fetchRowsCount; i++)
            {
                AddRows(dataTable, subjects, i);
            }

            return dataTable;
        }

        public static void Insert(Subject subject, string connectionString)
        {
            using IDbConnection db = new SqlConnection(connectionString);
            db.QueryFirstOrDefault<Subject>(InsertSubjectQuery,
            new { FacultyName = subject.Faculty.Name, subject.Name, subject.Description });
        }

        public static void Update(DataRowView rowData, string connectionString)
        {
            using IDbConnection db = new SqlConnection(connectionString);
            db.Execute(UpdateSubjectQuery, new
            {
                Name = rowData["Name"],
                Description = rowData["Description"],
                Faculty = rowData["Faculty"],
                ID = rowData["ID"]
            });
        }

        private static void AddRows(DataTable dataTable, List<Subject> entities, int i)
        {
            dataTable.Rows.Add(new object[] { entities[i].ID, entities[i].Name, entities[i].Faculty.Name, entities[i].Description });
        }

        private static DataColumn[] GetColumns()
        {
            return new DataColumn[] { new("ID", typeof(int)), new("Name", typeof(string)),
                new("Faculty", typeof(string)), new("Description", typeof(string)) };
        }

        private static List<string> GetColunmNames()
        {
            return new() { "Subjects.ID", "Name", "Faculty", "Description" };
        }
    }
}
