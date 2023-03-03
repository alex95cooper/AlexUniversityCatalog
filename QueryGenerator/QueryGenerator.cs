using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexUniversityCatalog
{
    internal class QueryGenerator
    {
        private readonly string[] _selectQuery = new string[10];

        public QueryGenerator(string tableName)
        {
            string tableColumns = GetTableColumns(tableName);
            FillSelectQueryByEmptyStrings();
            FillSelectQuery(tableName, tableColumns);
        }

        public string GetSelectQuery(int offsetCount, int fetchRowsCount, string sortingOrder = "ASC ")
        {
            _selectQuery[(int)QueryToken.SortingOrder] = sortingOrder;
            _selectQuery[(int)QueryToken.OffsetCount] = offsetCount.ToString();
            _selectQuery[(int)QueryToken.FetchRowsCount] = fetchRowsCount.ToString();
            return _selectQuery.ToQueryString();
        }

        private string GetTableColumns(string tableName)
        {
            string tableColumns = tableName switch
            {
                "Faculties" => "Name, Description FROM Faculties",
                "Subjects" => "Name, Description FROM Subjects",
                "Teachers" => @"FirstName, LastName, Age, Experience, Name AS Subjects 
                               FROM Teachers 
                               INNER JOIN Subjects ON Teachers.ID = Subjects.ID",
                "Students" => @"FirstName, LastName, Age, Year, Faculties.Name AS Faculty, STRING_AGG(Subjects.Name, ', ') AS Subjects FROM Students_Subjects
                               INNER JOIN Students ON Students.ID = Students_Subjects.StudID
                               INNER JOIN Faculties ON Students.FacID = Faculties.ID 
                               LEFT JOIN Subjects ON Students_Subjects.SubjID = Subjects.ID 
                               GROUP BY Students.ID, FirstName, LastName, Age, Year, Faculties.Name ",
                _ => string.Empty,
            };

            return tableColumns;
        }

        private void FillSelectQueryByEmptyStrings()
        {
            for (int i = 0; i < _selectQuery.Length; i++)
            {
                _selectQuery[i] = string.Empty;
            }
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
