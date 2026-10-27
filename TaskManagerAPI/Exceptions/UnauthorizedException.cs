namespace TaskManagerAPI.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string massage):base(massage) { }
    }
}
