namespace DeadlockLab;
public class BankTransferService
{
    public void TransferSafe(BankAccount from, BankAccount to, decimal amount)
    {
        if (ReferenceEquals(from, to))
            throw new ArgumentException("Нельзя перевести деньги на тот же счёт.");

        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Сумма должна быть больше нуля.");

        BankAccount first = from.Id < to.Id ? from : to;
        BankAccount second = from.Id < to.Id ? to : from;

        lock (first.SyncRoot)
        {
            lock (second.SyncRoot)
            {
                if (from.GetBalanceUnsafe() < amount)
                    throw new InvalidOperationException("Недостаточно средств.");

                from.SetBalanceUnsafe(from.GetBalanceUnsafe() - amount);
                to.SetBalanceUnsafe(to.GetBalanceUnsafe() + amount);
            }
        }
    }
}