namespace DeadlockLab;

public class DeadlockDemo
{
    public static void Run()
    {
        Console.WriteLine("=== DEADLOCK DEMO (Исправленный порядок) ===");

        object lockA = new();
        object lockB = new();

        Thread threadA = new(() =>
        {
            lock (lockA)
            {
                Console.WriteLine("Поток A: захватил ресурс A");
                Thread.Sleep(50);
                lock (lockB)
                {
                    Console.WriteLine("Поток A: получил оба ресурса");
                }
            }
        });

        Thread threadB = new(() =>
        {
            lock (lockA)
            {
                Console.WriteLine("Поток B: захватил ресурс A");
                Thread.Sleep(50);
                lock (lockB)
                {
                    Console.WriteLine("Поток B: получил оба ресурса");
                }
            }
        });

        threadA.IsBackground = true;
        threadB.IsBackground = true;

        threadA.Start();
        threadB.Start();
        bool completedA = threadA.Join(5000);
        bool completedB = threadB.Join(5000);

        Console.WriteLine();
        if (!completedA || !completedB)
        {
            Console.WriteLine("DEADLOCK ОБНАРУЖЕН!");
        }
        else
        {
            Console.WriteLine("Оба потока успешно завершились (deadlock предотвращён).");
        }
    }
    public static void RunThreeWayDeadlock()
    {
        Console.WriteLine("\n=== THREE-WAY DEADLOCK DEMO ===");
        object lockA = new(), lockB = new(), lockC = new();

        Thread t1 = new(() => { lock (lockA) { Thread.Sleep(50); lock (lockB) { } } });
        Thread t2 = new(() => { lock (lockB) { Thread.Sleep(50); lock (lockC) { } } });
        Thread t3 = new(() => { lock (lockC) { Thread.Sleep(50); lock (lockA) { } } });

        t1.IsBackground = t2.IsBackground = t3.IsBackground = true;
        t1.Start(); t2.Start(); t3.Start();

        bool done = t1.Join(3000) && t2.Join(3000) && t3.Join(3000);
        Console.WriteLine(done ? "Завершено." : "DEADLOCK ОБНАРУЖЕН (циклическое ожидание 3 потоков)!");
    }
}