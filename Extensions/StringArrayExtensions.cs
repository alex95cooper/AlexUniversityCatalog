namespace AlexUniversityCatalog
{
    public static class StringArrayExtensions
    {
        public static string ToQueryString(this string[] query)
        {
            string queryString = string.Empty;
            for (int i = 0; i < query.Length; i++)
            {
                queryString += query[i];
            }

            return queryString;
        }
    }
}
