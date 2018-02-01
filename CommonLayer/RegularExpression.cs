namespace CommonLayer
{
    public static class RegularExpression
    {
        public const string RegexEmail = @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}";

        public const string RegexNumber = @"^[0-9]*$";
    }
}
