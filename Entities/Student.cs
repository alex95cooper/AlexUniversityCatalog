using System.Collections.Generic;

namespace AlexUniversityCatalog
{
    internal class Student
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public int Year { get; set; }
        public Faculty Faculty { get; set; }
        public List<Subject> Subjects { get; set; }
    }
}
