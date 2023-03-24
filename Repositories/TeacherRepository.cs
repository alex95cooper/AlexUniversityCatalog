
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
    internal class TeacherRepository 
    {
        private const string SelectTeachersQuery = @"SELECT Teachers.*, Subjects.*
                FROM Teachers LEFT JOIN Subjects ON Teachers.SubjID = Subjects.ID 
                ORDER BY {0}.{1} {2}";

        public static List<Teacher> GetTeachers(string connectionString, string tableName, string nameOrderBy, string sortingOrder)
        {
            List<Teacher> subjects = new();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                subjects = db.Query<Teacher, Subject, Teacher>(string.Format(SelectTeachersQuery, tableName, nameOrderBy, sortingOrder), (teacher, subject) => {
                    teacher.Subject = subject; return teacher;
                }).ToList();
            }

            return subjects;
        }

        public static DataTable GetTable(List<Teacher> entities, int offsetCount, int fetchRowsCount)
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

        private static void AddRows(DataTable dataTable, List<Teacher> entities, int i)
        {
            dataTable.Rows.Add(new object[] { entities[i].ID, entities[i].FirstName, 
                entities[i].LastName, entities[i].Age, entities[i].Experience, entities[i].Subject.Name });
        }

        private static DataColumn[] GetColumns()
        {
            return new DataColumn[] { new("ID", typeof(int)), new("FirstName", typeof(string)), new("LastName", typeof(string)),
               new("Age", typeof(int)),  new("Year", typeof(int)), new("Faculty", typeof(string)) , new("Subjects", typeof(string))};
        }
    }
}
