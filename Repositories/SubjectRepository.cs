using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace AlexUniversityCatalog
{
    internal class SubjectRepository
    {
        private const string SelectSubjectsQuery = @"SELECT Subjects.*, Faculties.* FROM Subjects
                LEFT JOIN Faculties ON Faculties.ID = Subjects.FacID ORDER BY {0}.{1} {2}";

        public static List<Subject> GetSubjects(string connectionString, string tableName, string nameOrderBy, string sortingOrder)
        {
            List<Subject> subjects = new();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                subjects = db.Query<Subject, Faculty, Subject>(string.Format(SelectSubjectsQuery, tableName, nameOrderBy, sortingOrder), (subject, faculty) => {
                    subject.Faculty = faculty; return subject;
                }).ToList();
            }

            return subjects;
        }

        public static DataTable GetTable(List<Subject> entities, int offsetCount, int fetchRowsCount)
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

        private static void AddRows(DataTable dataTable, List<Subject> entities, int i)
        {
            dataTable.Rows.Add(new object[] { entities[i].ID, entities[i].Name, entities[i].Faculty.Name, entities[i].Description });
        }

        private static DataColumn[] GetColumns()
        {
            return new DataColumn[] { new("ID", typeof(int)), new("Name", typeof(string)),
                new("Faculty", typeof(string)), new("Description", typeof(string)) };
        }
    }
}
