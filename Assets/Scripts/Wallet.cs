using System;

public class Wallet
{
    public event Action AmountChanged;
    public int Count { get; private set; }

    public Wallet()
    {
        Count = 0;
    }

    public void Add()
    {
        Count++;
        AmountChanged?.Invoke();
    }
}
