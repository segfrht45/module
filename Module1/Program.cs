using System;
using System.IO;

delegate string TextOperation(string text);

class MessagePublisher
{
    public event Action<string> MessageSent;

    public void Send(string message)
    {
        MessageSent?.Invoke(message);
    }
}

class FileLogger
{
    private string fileName;

    public FileLogger(string fileName)
    {
        this.fileName = fileName;
    }

    public void Subscribe(MessagePublisher publisher)
    {
        publisher.MessageSent += LogMessage;
    }

    private void LogMessage(string message)
    {
        string log = $"[{DateTime.Now}] {message}\n";

        File.AppendAllText(fileName, log);
    }
}

class Program
{
    static string ToUpperCase(string text)
    {
        return text.ToUpper();
    }

    static string CountChars(string text)
    {
        return $"Кiлькiсть символiв: {text.Length}";
    }

    static string CountWords(string text)
    {
        int count = text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;

        return $"Кiлькiсть слiв: {count}";
    }

    static void ProcessFile(string inputFile,
                            string outputFile,
                            TextOperation operation)
    {
        string text = File.ReadAllText(inputFile);

        string result = operation(text);

        File.AppendAllText(outputFile, result + "\n\n");
    }

    static void Main(string[] args)
    {
        string input = "textPD24.txt";
        string output = "resultPD24.txt";

        ProcessFile(input, output, ToUpperCase);
        ProcessFile(input, output, CountChars);
        ProcessFile(input, output, CountWords);

        Console.WriteLine("Завдання 1 виконано");

        MessagePublisher publisher = new MessagePublisher();

        FileLogger logger = new FileLogger("logPD24.txt");

        logger.Subscribe(publisher);

        for (int i = 0; i < 4; i++)
        {
            Console.Write("Введiть повiдомлення: ");

            string msg = Console.ReadLine();

            publisher.Send(msg);
        }

        Console.WriteLine("Завдання 2 виконано");
    }
}