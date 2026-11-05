namespace DeadlockLab;
internal class Program
{
    static async Task Main()
    {
        Console.WriteLine("DEADLOCK И БЕЗОПАСНОЕ ВЗАИМОДЕЙСТВИЕ\n");
        Console.WriteLine("1. Воспроизведение и исправление deadlock (2 потока)");
        Console.WriteLine("2. Deadlock с тремя потоками (A->B, B->C, C->A)");
        Console.WriteLine("3. Безопасные банковские переводы (10 000 операций)");
        Console.WriteLine("0. Выход\n");

        Console.Write("Выберите эксперимент: ");
        string? choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                DeadlockDemo.Run();
                break;
            case "2":
                DeadlockDemo.RunThreeWayDeadlock();
                break;
            case "3":
                await TransferDemo.RunSafeAsync();
                break;
            case "0":
                return;
            default:
                Console.WriteLine("Неизвестная команда.");
                break;
        }
    }
}