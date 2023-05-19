using System.Data;
using System.Data.SqlClient;

namespace AlexUniversityCatalog
{
    internal class Updater
    {
        private readonly SqlConnection _connection;
        private readonly string _tableName;

        public Updater(string tableName, SqlConnection connection)
        {
            _tableName = tableName;
            _connection = connection;
        }

        public void UpdateTable(DataRowView rowData)
        {
            switch(_tableName)
            {
                case "Faculties":
                    UpdateFaculties(rowData);
                    break;
                case "Subjects":
                    UpdateSubjects(rowData);
                    break;
                case "Teachers":
                    UpdateTeachers(rowData);
                    break;
                case "Students":
                    UpdateStudents(rowData);
                    break;
            }
        }

        private void UpdateFaculties(DataRowView rowData)
        {
            string query = @"UPDATE Faculties SET Name = @name, Description = @description WHERE ID = @id";
            SqlCommand command = new(query, _connection);
            command.Parameters.AddWithValue("@name", rowData["Name"]);
            command.Parameters.AddWithValue("@description", rowData["Description"]);
            command.Parameters.AddWithValue("@id", rowData["ID"]);
            command.ExecuteNonQuery();
        }

        private void UpdateSubjects(DataRowView rowData)
        {
            string query = @"UPDATE Subjects SET Name = @name, Description = @description, 
            FacID = (SELECT Faculties.ID FROM Faculties WHERE Faculties.Name = @faculty) WHERE ID = @id";
            SqlCommand command = new(query, _connection);
            command.Parameters.AddWithValue("@name", rowData["Name"]);
            command.Parameters.AddWithValue("@description", rowData["Description"]);
            command.Parameters.AddWithValue("@faculty", rowData["Faculty"]);
            command.Parameters.AddWithValue("@id", rowData["ID"]);
            command.ExecuteNonQuery();
        }

        private void UpdateTeachers(DataRowView rowData)
        {
            string query = @"UPDATE Teachers SET FirstName = @firstName, LastName = @lastName', 
            Age = @age, Experience = @experience, SubjID = (SELECT Subjects.ID FROM Subjects 
            WHERE Subjects.Name = @subject) WHERE ID = @id";
            SqlCommand command = new(query, _connection);
            command.Parameters.AddWithValue("@firstName", rowData["FirstName"]);
            command.Parameters.AddWithValue("@lastName", rowData["LastName"]);
            command.Parameters.AddWithValue("@age", rowData["Age"]);
            command.Parameters.AddWithValue("@experience", rowData["Experience"]);
            command.Parameters.AddWithValue("@subject", rowData["Subject"]);
            command.Parameters.AddWithValue("@id", rowData["ID"]);
            command.ExecuteNonQuery();
        }

        private void UpdateStudents(DataRowView rowData)
        {
            string query = @$"UPDATE Students SET FirstName = @firstName, LastName = @lastName, 
            Age = @age, Year = @year, FacID = (SELECT Faculties.ID FROM Faculties 
            WHERE Faculties.Name = @faculty) WHERE ID = @id";
            SqlCommand command = new(query, _connection);
            command.Parameters.AddWithValue("@firstName", rowData["FirstName"]);
            command.Parameters.AddWithValue("@lastName", rowData["LastName"]);
            command.Parameters.AddWithValue("@age", rowData["Age"]);
            command.Parameters.AddWithValue("@year", rowData["Year"]);
            command.Parameters.AddWithValue("@faculty", rowData["Faculty"]);
            command.Parameters.AddWithValue("@id", rowData["ID"]);
            command.ExecuteNonQuery();
        }
    }
}
