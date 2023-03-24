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
    internal class StudentRepository 
    {
        private const string SelectStudentsQuery = @"SELECT Students.*, Faculties.*, Subjects.*
                FROM Students_Subjects
                RIGHT JOIN Students ON Students.ID = Students_Subjects.StudID
                INNER JOIN Faculties ON Students.FacID = Faculties.ID
                LEFT JOIN Subjects ON Students_Subjects.SubjID = Subjects.ID 
                ORDER BY {0}.{1} {2}";

        public static List<Student> GetStudents(string connectionString, string tableName, string nameOrderBy, string sortingOrder)
        {
            List<Student> students = new();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                students = db.Query<Student, Faculty, Subject, Student>(string.Format(SelectStudentsQuery, tableName, nameOrderBy, sortingOrder), (student, faculty, subject) =>
                {
                    student.Faculty = faculty;
                    if (student.Subjects == null)
                    {
                        student.Subjects = new();
                        student.Subjects.Add(subject);
                    }
                    return student;
                }).ToList();
                students = MergeRepeatingStudents(students);
            }

            return students;
        }

        public static DataTable GetTable(List<Student> entities, int offsetCount, int fetchRowsCount)
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

        private static List<Student> MergeRepeatingStudents(List<Student> students)
        {
            for (int i = 0; i < students.Count; i++)
            {
                for (int j = 0; j < students.Count; j++)
                {
                    if (students[i].ID == students[j].ID)
                    {
                        students[i].Subjects.AddRange(students[j].Subjects);
                        students.RemoveAt(j);
                    }
                }
            }

            return students;
        }

        private static void AddRows(DataTable dataTable, List<Student> entities, int i)
        {
            dataTable.Rows.Add(new object[] { entities[i].ID, entities[i].FirstName,
                    entities[i].LastName, entities[i].Age, entities[i].Year, entities[i].Faculty.Name, entities[i].Subjects.ToSubjectsString() });
        }

        private static DataColumn[] GetColumns()
        {
            return new DataColumn[] { new("ID", typeof(int)), new("FirstName", typeof(string)), new("LastName", typeof(string)),
               new("Age", typeof(int)),  new("Year", typeof(int)), new("Faculty", typeof(string)) , new("Subjects", typeof(string))};
        }
    }
}
