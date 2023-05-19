using System.Collections.Generic;

namespace AlexUniversityCatalog
{
    internal static class SubjectListExtensions
    {
        public static string ToSubjectsString(this List<Subject> subjects)
        {
            return (subjects[0] == null) ? string.Empty : FillSubjectString(subjects);
        }

        private static string FillSubjectString(List<Subject> subjects)
        {
            string subjectsString = string.Empty;
            foreach (Subject subject in subjects)
            {
                subjectsString += subject.Name + ", ";
            }

            subjectsString = subjectsString[..^2];
            return subjectsString;
        }
    }
}
