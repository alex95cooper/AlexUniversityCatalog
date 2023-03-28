﻿using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace AlexUniversityCatalog
{
    internal class TeacherRepository
    {
        private const string SelectTeachersQuery = @"SELECT Teachers.*, Subjects.*
                FROM Teachers LEFT JOIN Subjects ON Teachers.SubjID = Subjects.ID ORDER BY {0} {1}";
        private const string InsertTeacherQuery = @"INSERT INTO Teachers (SubjID, FirstName, LastName, Age, Experience) 
                VALUES ((SELECT ID FROM Subjects WHERE Name = @SubjectName), @FirstName, @LastName, @Age, @Experience)";
        private const string UpdateSubjectQuery = @"UPDATE Teachers SET SubjID = (SELECT ID FROM Subjects WHERE Name = @SubjectName), 
                FirstName = @FirstName, LastName = @LastName, Age = @Age, Experience = @Experience WHERE ID = @ID";

        public static List<Teacher> GetTeachers(string connectionString, string nameOrderBy, string sortingOrder)
        {
            List<Teacher> subjects = new();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                subjects = db.Query<Teacher, Subject, Teacher>(string.Format(SelectTeachersQuery, nameOrderBy, sortingOrder), (teacher, subject) =>
                {
                    teacher.Subject = subject; return teacher;
                }).ToList();
            }

            return subjects;
        }

        public static DataTable GetTable(string connectionString, string nameOrderBy, string sortingOrder, int offsetCount, int fetchRowsCount)
        {
            DataTable dataTable = new();
            List<Teacher> subjects = GetTeachers(connectionString, nameOrderBy, sortingOrder);
            dataTable.Columns.AddRange(GetColumns());
            fetchRowsCount = (offsetCount + fetchRowsCount > subjects.Count) ? subjects.Count - offsetCount : fetchRowsCount;
            for (int i = offsetCount; i < offsetCount + fetchRowsCount; i++)
            {
                AddRows(dataTable, subjects, i);
            }

            return dataTable;
        }

        public static void Insert(Teacher teacher, string connectionString)
        {
            using IDbConnection db = new SqlConnection(connectionString);
            db.QueryFirstOrDefault<Subject>(InsertTeacherQuery,
            new { SubjectName = teacher.Subject.Name, teacher.FirstName, teacher.LastName, teacher.Age, teacher.Experience });
        }

        public static void Update(DataRowView rowData, string connectionString)
        {
            using IDbConnection db = new SqlConnection(connectionString);
            db.Execute(UpdateSubjectQuery, new
            {
                SubjectName = rowData["Subject"],
                FirstName = rowData["FirstName"],
                LastName = rowData["LastName"],
                Age = rowData["Age"],
                Experience = rowData["Experience"],
                ID = rowData["ID"]
            });
        }

        private static void AddRows(DataTable dataTable, List<Teacher> entities, int i)
        {
            dataTable.Rows.Add(new object[] { entities[i].ID, entities[i].FirstName,
                entities[i].LastName, entities[i].Age, entities[i].Experience, entities[i].Subject.Name });
        }

        private static DataColumn[] GetColumns()
        {
            return new DataColumn[] { new("ID", typeof(int)), new("FirstName", typeof(string)), new("LastName", typeof(string)),
               new("Age", typeof(int)),  new("Experience", typeof(int)), new("Subject", typeof(string))};
        }
    }
}
