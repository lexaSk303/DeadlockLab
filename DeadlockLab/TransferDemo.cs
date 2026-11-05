namespace DeadlockLab;
public class TransferDemo
{
    public static async Task RunSafeAsync()
    {
        Console.WriteLine("=== SAFE TRANSFER DEMO ===");

        BankAccount accountA = new(10000); 
        BankAccount accountB = new(20000); 
        BankAccount accountC = new(30000); 

        BankTransferService service = new();

        const int operations = 10000;
        const decimal amount = 1;

        Console.WriteLine("Запуск параллельных переводов...");

        Task[] tasks = new Task[3];

        tasks[0] = Task.Run(() =>
        {
            for (int i = 0; i < operations; i++)
            {
                try
                {
                    service.TransferSafe(accountA, accountB, amount);
                }
                catch (InvalidOperationException)
                {

                }
            }
        });

        tasks[1] = Task.Run(() =>
        {
            for (int i = 0; i < operations; i++)
            {
                try
                {
                    service.TransferSafe(accountB, accountA, amount);
                }
                catch (InvalidOperationException)
                {

                }
            }
        });

        tasks[2] = Task.Run(() =>
        {
            for (int i = 0; i < operations; i++)
            {
                try
                {
                    service.TransferSafe(accountB, accountC, amount);
                }
                catch (InvalidOperationException)
                {
                
                }
            }
        });

        await Task.WhenAll(tasks);

        decimal totalBalance = accountA.Balance + accountB.Balance + accountC.Balance;
        Console.WriteLine($"Баланс A: {accountA.Balance}");
        Console.WriteLine($"Баланс B: {accountB.Balance}");
        Console.WriteLine($"Баланс C: {accountC.Balance}");
        Console.WriteLine($"Общий баланс: {totalBalance} (должен быть 60000)");
        Console.WriteLine("\n--- Проверка граничных сценариев ---");

        try
        {
            service.TransferSafe(accountA, accountB, 99999);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"[OK] Перевод отклонён: {ex.Message}");
        }

        try
        {
            service.TransferSafe(accountA, accountA, 10);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"[OK] Перевод отклонён: {ex.Message}");
        }
    }
}