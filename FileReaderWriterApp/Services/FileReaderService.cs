namespace FileReaderWriterApp.Services;

internal class FileReaderService
{
    private readonly string _filePath;
    private long _lastPosition = 0;

    public FileReaderService(string filePath)
    {
        _filePath = filePath;
    }

    public void Run()
    {
        Console.Clear();

        if (!File.Exists(_filePath))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("===============");
            Console.WriteLine("     ERROR     ");
            Console.WriteLine("===============");
            Console.WriteLine($"\nThe shared file was not found at: {_filePath}\n");
            Console.ResetColor();
            return;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("=====================");
        Console.WriteLine("     READER MODE     ");
        Console.WriteLine("=====================");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($" Watching file ");
        Console.WriteLine(" Press 'q' at any time to exit Reader Mode.\n");
        Console.ResetColor();

        _lastPosition = new FileInfo(_filePath).Length;

        try
        {
            using (FileStream fs = new(_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader reader = new(fs, System.Text.Encoding.UTF8))
            {
                fs.Seek(_lastPosition, SeekOrigin.Begin);

                while (true)
                {
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo key = Console.ReadKey(intercept: true);

                        if (key.Key == ConsoleKey.Q)
                            break;
                    }

                    if (fs.Length < _lastPosition)
                    {
                        _lastPosition = 0;
                        fs.Seek(0, SeekOrigin.Begin);

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("=====================");
                        Console.WriteLine("     READER MODE     ");
                        Console.WriteLine("=====================");
                        Console.WriteLine("File was cleared. Waiting for new text...\n");
                        Console.WriteLine("Or Press 'q' at any time to exit Reader Mode.\n");
                        Console.ResetColor();
                    }

                    if (fs.Length > _lastPosition)
                    {
                        string? line;
                        Console.ForegroundColor = ConsoleColor.Green;

                        while ((line = reader.ReadLine()) != null)
                        {
                            Console.WriteLine(line);
                        }

                        _lastPosition = fs.Length;
                        Console.ResetColor();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Reader Error: {ex.Message}");
            Console.ResetColor();
        }
    }
}