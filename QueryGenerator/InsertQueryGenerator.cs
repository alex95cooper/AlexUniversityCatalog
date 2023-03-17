using System.Collections.Generic;

namespace AlexUniversityCatalog
{ 
    internal class InsertQueryGenerator
    {
        private readonly string[] _insertQuery = new string[5];

        public InsertQueryGenerator(string tableName)
        {
            string tableColumns = GetTableColumns(tableName);
            FillINsertQuery(tableName, tableColumns);
        }

        public string GetInsertFacultyQuery(string name, string description)
        {
            string values = $"('{name}', '{description}')";
            _insertQuery[4] = values;
            return _insertQuery.ToQueryString(); 
        }

        public string GetInsertSubjectQuery(string name, string facultyName, string description)
        {
            string values = $"((SELECT ID FROM Faculties WHERE Faculties.Name = '{facultyName}'), '{name}', '{description}')";
            _insertQuery[4] = values;
            return _insertQuery.ToQueryString();
        }

        public string GetInsertTeacherQuery(string firstName, string lastName, string age, string experience, string subjectName)
        {
            string values = $"((SELECT ID FROM Subjects WHERE Subjects.Name = '{subjectName}'), '{firstName}', '{lastName}', {age}, {experience})";
            _insertQuery[4] = values;
            return _insertQuery.ToQueryString();
        }

        public string GetInsertStudentsQuery(string firstName, string lastName, string age, string year, string facultyName)
        {          
            string values = $"('{firstName}', '{lastName}', {age}, {year}, (SELECT ID FROM Faculties WHERE Faculties.Name = '{facultyName}'))";
            _insertQuery[4] = values;
            return _insertQuery.ToQueryString();
        }

        public static List<string> GetInsertStudentsSubjectsQueries(int studentID, string subjectNames)
        {
            List<string> queries = new();
            List<string> subjectNamesCollection = new(subjectNames.Split(", "));
            foreach (string subjectName in subjectNamesCollection)
            {
                queries.Add($"INSERT INTO Students_Subjects (StudID, SubjID) VALUES ({studentID}, (SELECT ID FROM Subjects WHERE Subjects.Name = '{subjectName}'))");
            }

            return queries;
        }

        private static string GetTableColumns(string tableName)
        {
            string tableColumns = tableName switch
            {
                "Faculties" => "(Name, Description)",
                "Subjects" => "(FacID, Name, Description)",
                "Teachers" => "(SubjID, FirstName, LastName, Age, Experience)",
                "Students" => "(FirstName, LastName, Age, Year, FacID)",
                _ => string.Empty,
            };

            return tableColumns;
        }

        private void FillINsertQuery(string tableName, string tableColumns)
        {
            _insertQuery[0] = "INSERT INTO ";
            _insertQuery[1] = tableName + " ";
            _insertQuery[2] = tableColumns;
            _insertQuery[3] = " VALUES ";
        }
    }
}
