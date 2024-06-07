namespace TechECommerceServer.Infrastructure.Operations.File
{
    public static class NameOperation
    {
        // note: it's necessary to guarantee that the names of the file(s) received by the client contain letters of the private alphabet.
        public static string CharacterRegulatory(string fileName)
            => fileName.Replace("!", "")
                .Replace("'", "")
                .Replace("^", "")
                .Replace("+", "")
                .Replace("%", "")
                .Replace("&", "")
                .Replace("#", "")
                .Replace("$", "")
                .Replace("*", "")
                .Replace("æ", "")
                .Replace("ß", "")
                .Replace("(", "")
                .Replace(")", "")
                .Replace("½", "")
                .Replace("{", "")
                .Replace("[", "")
                .Replace("]", "")
                .Replace("}", "")
                .Replace("=", "")
                .Replace("?", "")
                .Replace("_", "")
                .Replace(" ", "-")
                .Replace("@", "")
                .Replace("€", "")
                .Replace("¨", "")
                .Replace("~", "")
                .Replace(",", "")
                .Replace(";", "")
                .Replace(":", "")
                .Replace(".", "")
                .Replace("<", "")
                .Replace(">", "")
                .Replace("|", "");
    }
}
