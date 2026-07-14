namespace FileReaderWriterApp.Services;

internal class FileWriterService
{
    private readonly string _filePath;

    public FileWriterService(string filePath)
    {
        _filePath = filePath;
    }

    public void Run()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("====================================");
        Console.WriteLine("     WRITER MODE INITIALIZATION     ");
        Console.WriteLine("====================================");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(" Choose flush mode (manual / auto): ");
        Console.ResetColor();

        string flush = Console.ReadLine()?.Trim().ToLower() ?? "";

        if (flush != "auto" && flush != "manual")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\nError: '{flush}' is not a valid mode!");
            Console.WriteLine("Exiting Writer Mode. Please restart and type 'auto' or 'manual'.");
            Console.ResetColor();
            return;
        }

        bool isAutoFlush = (flush == "auto");

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("=================================================");
        Console.WriteLine($"    WRITER MODE ACTIVE  |  FLUSH: {flush.ToUpper()}");
        Console.WriteLine("=================================================");

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("Write your message and press [Enter]");

        if (!isAutoFlush)
        {
            Console.WriteLine("Type '/flush' to send your pending messages to the Reader");
        }

        Console.WriteLine("Type 'exit' to quit the application.\n");
        Console.ResetColor();

        try
        {
            using (FileStream fs = new(_filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            using (StreamWriter writer = new(fs, System.Text.Encoding.UTF8))
            {
                writer.AutoFlush = isAutoFlush;

                while (true)
                {
                    string? input = Console.ReadLine();

                    if (string.IsNullOrEmpty(input))
                        continue;

                    if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                        break;

                    if (!isAutoFlush && input.Equals("/flush", StringComparison.OrdinalIgnoreCase))
                    {
                        writer.Flush();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("[Flushed to file]");
                        Console.ResetColor();
                        continue;
                    }

                    writer.WriteLine(input);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Writer Error: {ex.Message}");
        }

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\nExiting Writer Mode");
        Console.ResetColor();
    }
}