using System.Data;

namespace AlexUniversityCatalog
{
    internal class UpdateQueryGenerator
    {
        private readonly string _tableName;

        public UpdateQueryGenerator(string tableName)
        {
            _tableName = tableName;
        }

        public string SelectUpdateQuery(DataRowView rowData)
        {
            return _tableName switch
            {
                "Faculties" => GetUpdateFacultiesQuery(rowData),
                "Subjects" => GetUpdateSubjectsQuery(rowData),
                "Teachers" => GetUpdateTeachersQuery(rowData),
                "Students" => GetUpdateStudentsQuery(rowData),
                _ => string.Empty
            };
        }

        private static string GetUpdateFacultiesQuery(DataRowView rowData)
        {
            return @$"UPDATE Faculties 
                    SET Name = '{rowData["Name"]}', Description = '{rowData["Description"]}'                   
                    WHERE ID = {rowData["ID"]}";
        }

        private static string GetUpdateSubjectsQuery(DataRowView rowData)
        {
            return @$"UPDATE Subjects 
                    SET Name = '{rowData["Name"]}', Description = '{rowData["Description"]}', 
                    FacID = (SELECT Faculties.ID FROM Faculties WHERE Faculties.Name = '{rowData["Faculty"]}') 
                    WHERE ID = {rowData["ID"]}";
        }

        private static string GetUpdateTeachersQuery(DataRowView rowData)
        {
            return @$"UPDATE Teachers 
                    SET FirstName = '{rowData["FirstName"]}', LastName = '{rowData["LastName"]}', 
                    Age = {rowData["Age"]}, Experience = {rowData["Experience"]}, 
                    SubjID = (SELECT Subjects.ID FROM Subjects WHERE Subjects.Name = '{rowData["Subject"]}') 
                    WHERE ID = {rowData["ID"]}";
        }

        private static string GetUpdateStudentsQuery(DataRowView rowData)
        {
            return @$"UPDATE Students 
                    SET FirstName = '{rowData["FirstName"]}', LastName = '{rowData["LastName"]}', 
                    Age = {rowData["Age"]}, Year = {rowData["Year"]}, 
                    FacID = (SELECT Faculties.ID FROM Faculties WHERE Faculties.Name = '{rowData["Faculty"]}') 
                    WHERE ID = {rowData["ID"]}";
        }
    }
}
