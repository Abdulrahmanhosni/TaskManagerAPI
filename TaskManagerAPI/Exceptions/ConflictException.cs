namespace TaskManagerAPI.Exceptions
{
    public class ConflictException : Exception
    {
        public ConflictException(string massage) : base(massage) 
        {

        }
    }
}