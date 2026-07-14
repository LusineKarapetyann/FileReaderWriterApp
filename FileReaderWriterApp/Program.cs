using FileReaderWriterApp.Services;

class Program
{
    static void Main()
    {
        Console.Clear();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=======================================");
        Console.WriteLine("     WELCOME TO FILE READER-WRITER     ");
        Console.WriteLine("=======================================");
        Console.ResetColor();

        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "shared.txt");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("\n Select mode (reader / writer): ");
        Console.ResetColor();

        string mode = Console.ReadLine()?.Trim().ToLower() ?? "";

        try
        {
            if (mode == "writer")
            {
                FileWriterService writerService = new(filePath);
                writerService.Run();
            }
            else if (mode == "reader")
            {
                FileReaderService readerService = new(filePath);
                readerService.Run();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n Invalid mode selected. Exiting Program");
                Console.ResetColor();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n An unexpected error occurred: {ex.Message}");
            Console.ResetColor();
        }
    }
}