using System;

public class Health
{
    public event Action Dead;

    private int _max = 100;
    private int _min = 0;

    public Health()
    {
        CurrentValue = _max;
    }

    public Health(int maxVolume)
    {
        CurrentValue = maxVolume;
    }

    public int CurrentValue { get; private set; }

    public bool TryRemove(int value)
    {
        bool isDone = true;

        if (CurrentValue >= value)
        {
            CurrentValue -= value;

            if (CurrentValue == _min)
            {
                Dead?.Invoke();
            }
        }
        else
        {
            isDone = false;
        }

        return isDone;
    }
}
