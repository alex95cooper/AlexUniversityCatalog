using System.Collections.Generic;
using System.Data.SqlClient;

namespace AlexUniversityCatalog
{ 
    internal class Inserter
    {
        private readonly SqlConnection _connection;

        public Inserter(SqlConnection connection)
        {
            _connection = connection;
        }

        public void InsertFaculty(string name, string description)
        {
            string query = "INSERT INTO Faculties (Name, Description) VALUES (@name, @description)";
            SqlCommand command = new(query, _connection);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@description", description);
            command.ExecuteNonQuery();
        }

        public void InsertSubject(string name, string facultyName, string description)
        {
            string query = @"INSERT INTO Subjects (FacID, Name, Description) 
            VALUES ((SELECT ID FROM Faculties WHERE Faculties.Name = @facultyName), @name, @description)";
            SqlCommand command = new(query, _connection);
            command.Parameters.AddWithValue("@facultyName", facultyName);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@description", description);
            command.ExecuteNonQuery();
        }

        public void InsertTeacher(string firstName, string lastName, string age, string experience, string subjectName)
        {
            string query = @"INSERT INTO Teachers (SubjID, FirstName, LastName, Age, Experience) 
            VALUES ((SELECT ID FROM Subjects WHERE Subjects.Name = @subjectName), @firstName, @lastName, @age, @experience)";
            SqlCommand command = new(query, _connection);
            command.Parameters.AddWithValue("@subjectName", subjectName);
            command.Parameters.AddWithValue("@firstName", firstName);
            command.Parameters.AddWithValue("@lastName", lastName);
            command.Parameters.AddWithValue("@age", age);
            command.Parameters.AddWithValue("@experience", experience);
            command.ExecuteNonQuery();
        }

        public void InsertStudent(string firstName, string lastName, string age, string year, string facultyName)
        {
            string query = @"INSERT INTO Students (FirstName, LastName, Age, Year, FacID) 
            VALUES (@firstName, @lastName, @age, @year, (SELECT ID FROM Faculties WHERE Faculties.Name = @facultyName))";
            SqlCommand command = new(query, _connection);
            command.Parameters.AddWithValue("@firstName", firstName);
            command.Parameters.AddWithValue("@lastName", lastName);
            command.Parameters.AddWithValue("@age", age);
            command.Parameters.AddWithValue("@year", year);
            command.Parameters.AddWithValue("@facultyName", facultyName);
            command.ExecuteNonQuery();
        }

        public void InsertStudentsWithSubjects(int studentID, string subjectNames)
        {
            List<string> subjectNamesCollection = new(subjectNames.Split(", "));
            foreach (string subjectName in subjectNamesCollection)
            {
                string query = "INSERT INTO Students_Subjects (StudID, SubjID) VALUES (@studentID, (SELECT ID FROM Subjects WHERE Subjects.Name = @subjectName))";
                SqlCommand command = new(query, _connection);
                command.Parameters.AddWithValue("@studentID", studentID);
                command.Parameters.AddWithValue("@subjectName", subjectName);
                command.ExecuteNonQuery();
            }
        }
    }
}
