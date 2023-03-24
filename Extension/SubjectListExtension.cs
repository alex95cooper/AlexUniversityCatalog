using System;
using System.Collections.Generic;

namespace AlexUniversityCatalog
{
    internal static class SubjectListExtension
    {
        public static string ToSubjectsString(this List<Subject> subjects)
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
