using System.Collections.Generic;

namespace AlexUniversityCatalog
{
    internal class Selector
    {
        private const string FacultiesTableQueryString = "ID, Name, Description FROM Faculties";
        private const string SubjectsTableQueryString = @"Subjects.ID, Subjects.Name, Faculties.Name AS Faculty, Subjects.Description FROM Subjects 
        INNER JOIN Faculties ON Faculties.ID = Subjects.FacID ";
        private const string TeachersTableColumnsJoin = @"Teachers.ID, FirstName, LastName, Age, Experience, Name AS Subject 
        FROM Teachers INNER JOIN Subjects ON Teachers.SubjID = Subjects.ID";
        private const string StudentsTableColumnsJoin = @"Students.ID, FirstName, LastName, Age, Year, Faculties.Name AS Faculty, STRING_AGG(Subjects.Name, ', ') AS Subjects 
        FROM Students_Subjects 
        INNER JOIN Students ON Students.ID = Students_Subjects.StudID 
        INNER JOIN Faculties ON Students.FacID = Faculties.ID 
        LEFT JOIN Subjects ON Students_Subjects.SubjID = Subjects.ID 
        GROUP BY Students.ID, FirstName, LastName, Age, Year, Faculties.Name ";

        private readonly string _tableName;
        private readonly string _tableColumns;
        private readonly List<string> _columnNames;

        public Selector(string tableName)
        {
            _tableName = tableName;
            _tableColumns = GetTableColumns(tableName);
            _columnNames = FillColumnNames();
        }

        public string GetSelectQuery(string nameOrderBy, string sortingOrder, int offsetCount, int fetchRowsCount)
        {
            return CheckIfOrderNameValid(nameOrderBy) ? @$"SELECT {_tableColumns} ORDER BY {nameOrderBy} {sortingOrder} 
            OFFSET {offsetCount} ROWS FETCH NEXT {fetchRowsCount} ROWS ONLY" : string.Empty;
        }

        private bool CheckIfOrderNameValid(string nameOrderBy)
        {
            foreach (string columnName in _columnNames)
            {
                if (columnName == nameOrderBy)
                {
                    return true;
                }
            }

            return false;
        }

        private static string GetTableColumns(string tableName)
        {
            return tableName switch
            {
                "Faculties" => FacultiesTableQueryString,
                "Subjects" => SubjectsTableQueryString,
                "Teachers" => TeachersTableColumnsJoin,
                "Students" => StudentsTableColumnsJoin,
                _ => string.Empty
            };
        }

        private List<string> FillColumnNames()
        {
            return _tableName switch
            {
                "Faculties" => new() { "Faculties.ID", "Name", "Description" },
                "Subjects" => new() { "Subjects.ID", "Name", "Faculty", "Description" },
                "Teachers" => new() { "Teachers.ID", "FirstName", "LastName", "Age", "Experience", "Subject" },
                "Students" => new() { "Students.ID", "FirstName", "LastName", "Age", "Year", "Faculty", "Subjects" },
                _ => new()
            };
        }
    }
}