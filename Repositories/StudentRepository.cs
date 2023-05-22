using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;

namespace AlexUniversityCatalog
{
    internal static class StudentRepository 
    {
        private const string SubjectsFieldMessage = "You entered subject name(s) or not fill subject field. The student will be added without a list of subjects.";
        private const string IncorrectDataErrorMessage = "You entered incorrect data";
        private const string SelectLastStudentsQuery = @"SELECT Students.* FROM Students WHERE ID = (SELECT MAX(ID) FROM Students)";
        private const string InsertStudentQuery = @"INSERT INTO Students (FacID, FirstName, LastName, Age, Year) 
                VALUES ((SELECT ID FROM Faculties WHERE Name = @FacultyName), @FirstName, @LastName, @Age, @Year)";
        private const string InsertSubjectsQuery = @"INSERT INTO Students_Subjects (StudID, SubjID) 
                VALUES  (@StudentID, (SELECT ID FROM Subjects WHERE Name = @SubjectName))";
        private const string UpdateStudentQuery = @"UPDATE Students SET FirstName = @FirstName, LastName = @LastName, Age = @Age,
                FacID = (SELECT Faculties.ID FROM Faculties WHERE Faculties.Name = @Faculty) WHERE ID = @ID";

        public static DataTable GetTable(string connectionString, string nameOrderBy, string sortingOrder, int offsetCount, int fetchRowsCount)
        {
            if (FacultyRepository.CheckIfNameValid(GetColunmNames(), nameOrderBy))
            {
                List<Student> students = GetStudents(connectionString, nameOrderBy, sortingOrder);
                return GetTableWithColumns(students, offsetCount, fetchRowsCount);
            }

            return null;
        }

        public static void Insert(Student student, string connectionString)
        {
            using IDbConnection db = new SqlConnection(connectionString);
            db.QueryFirstOrDefault<Student>(InsertStudentQuery, new
            {
                FacultyName = student.Faculty.Name,
                student.FirstName,
                student.LastName,
                student.Age,
                student.Year
            });
            Student lastStudent = db.QueryFirstOrDefault<Student>(SelectLastStudentsQuery);
            InsertSubjects(db, lastStudent, student.Subjects);
        }

        public static void Update(DataRowView rowData, string connectionString)
        {
            using IDbConnection db = new SqlConnection(connectionString);
            db.Execute(UpdateStudentQuery, new
            {
                FirstName = rowData["FirstName"],
                LastName = rowData["LastName"],
                Age = rowData["Age"],
                Faculty = rowData["Faculty"],
                ID = rowData["ID"]
            });
            UpdateSubjects(db, rowData);
        }
        private static List<Student> GetStudents(string connectionString, string nameOrderBy, string sortingOrder)
        {
            using IDbConnection db = new SqlConnection(connectionString);
            List<Student> students = db.Query<Student, Faculty, Student>($@"SELECT Students.*, Faculties.* FROM Students 
            INNER JOIN Faculties ON Students.FacID = Faculties.ID ORDER BY {nameOrderBy} {sortingOrder}", (student, faculty) =>
            {
                student.Faculty = faculty;
                using IDbConnection db2 = new SqlConnection(connectionString);
                student.Subjects = db2.Query<Subject>($@"SELECT Subjects.* FROM Subjects 
                        RIGHT JOIN Students_Subjects ON Subjects.ID = Students_Subjects.SubjID WHERE StudID = {student.ID}").ToList();
                return student;
            }).ToList();

            return students;
        }

        private static DataTable GetTableWithColumns(List<Student> students, int offsetCount, int fetchRowsCount)
        {
            DataTable dataTable = new();
            dataTable.Columns.AddRange(GetColumns());
            fetchRowsCount = (offsetCount + fetchRowsCount > students.Count) ? students.Count - offsetCount : fetchRowsCount;
            for (int i = offsetCount; i < offsetCount + fetchRowsCount; i++)
            {
                AddRows(dataTable, students, i);
            }

            return dataTable;
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

        private static void InsertSubjects(IDbConnection db, Student student, List<Subject> subjects)
        {
            try
            {
                foreach (Subject subject in subjects)
                {
                    db.Execute(InsertSubjectsQuery, new { StudentID = student.ID, SubjectName = subject.Name });
                }
            }
            catch
            {
                MessageBox.Show(SubjectsFieldMessage);
            }
        }

        private static void UpdateSubjects(IDbConnection db, DataRowView rowData)
        {
            List<string> subjectNamesUiCollection = new(rowData["Subjects"].ToString().Split(", "));
            if (CheckIfSubjectsExists(subjectNamesUiCollection, db))
            {
                db.Execute($"DELETE FROM Students_Subjects WHERE StudID = {rowData["ID"]}");
                foreach (string subjectName in subjectNamesUiCollection)
                {
                    db.Execute(@$"INSERT INTO Students_Subjects (StudID, SubjID) 
                    VALUES ({rowData["ID"]}, (SELECT ID FROM Subjects WHERE Name = '{subjectName}'))");
                }
            }
            else
            {
                MessageBox.Show(IncorrectDataErrorMessage);
            }
        }

        private static bool CheckIfSubjectsExists(List<string> subjectNamesUiCollection, IDbConnection db)
        {
            foreach (string subjectName in subjectNamesUiCollection)
            {
                if (db.ExecuteScalar($"SELECT ID FROM Subjects WHERE Name = '{subjectName}'") == null)
                {
                    return false;
                }
            }

            return true;
        }

        private static List<string> GetColunmNames()
        {
            return new() { "Students.ID", "FirstName", "LastName", "Age", "Year", "Faculty", "Subjects" };
        }
    }
}
