namespace Mock1Trainning.Logging
{
    public class Logging : ILogging
    {
        public void Log(string message, string type)
        {
            if (type == "error")
            {
                Console.WriteLine("error - " + message);
            }
            else
            {
                Console.WriteLine(message);
            }
        }
    }
}
