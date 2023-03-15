﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexUniversityCatalog
{
    internal class SelectQueryGenerator
    {
        private const string SubjectsTableQueryString = "Subjects.ID, Subjects.Name, Faculties.Name AS Faculty, Subjects.Description FROM Subjects " +
            "INNER JOIN Faculties ON Faculties.ID = Subjects.FacID ";
        private const string TeachersTableColumnsJoin = "Teachers.ID, FirstName, LastName, Age, Experience, Name AS Subject " +
            "FROM Teachers INNER JOIN Subjects ON Teachers.SubjID = Subjects.ID";
        private const string StudentsTableColumnsJoin = "Students.ID, FirstName, LastName, Age, Year, Faculties.Name AS Faculty, STRING_AGG(Subjects.Name, ', ') AS Subjects " +
            "FROM Students_Subjects " +
            "INNER JOIN Students ON Students.ID = Students_Subjects.StudID " +
            "INNER JOIN Faculties ON Students.FacID = Faculties.ID " +
            "LEFT JOIN Subjects ON Students_Subjects.SubjID = Subjects.ID " +
            "GROUP BY Students.ID, FirstName, LastName, Age, Year, Faculties.Name ";


        private readonly string[] _selectQuery = new string[10];

        public SelectQueryGenerator(string tableName)
        {
            string tableColumns = GetTableColumns(tableName);
            FillSelectQuery(tableName, tableColumns);
        }

        public string GetSelectQuery(int offsetCount, int fetchRowsCount, string nameOrderBy, string sortingOrder)
        {
            _selectQuery[(int)SelectQueryToken.NameOrderBy] = nameOrderBy + " ";
            _selectQuery[(int)SelectQueryToken.SortingOrder] = sortingOrder;
            _selectQuery[(int)SelectQueryToken.OffsetCount] = offsetCount.ToString();
            _selectQuery[(int)SelectQueryToken.FetchRowsCount] = fetchRowsCount.ToString();
            return _selectQuery.ToQueryString();
        }

        private static string GetTableColumns(string tableName)
        {
            string tableColumns = tableName switch
            {
                "Faculties" => "ID, Name, Description FROM Faculties",
                "Subjects" => SubjectsTableQueryString,
                "Teachers" => TeachersTableColumnsJoin,
                "Students" => StudentsTableColumnsJoin,
                _ => string.Empty
            };

            return tableColumns;
        }

        private void FillSelectQuery(string tableName, string tableColumns)
        {
            _selectQuery[0] = "SELECT ";
            _selectQuery[1] = tableColumns;
            _selectQuery[2] = " ORDER BY ";
            _selectQuery[3] = tableName + ".ID ";
            _selectQuery[5] = " OFFSET ";
            _selectQuery[7] = " ROWS FETCH NEXT ";
            _selectQuery[9] = " ROWS ONLY";
        }
    }
}
