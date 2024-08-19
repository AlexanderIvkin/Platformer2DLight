using System;

public class Health
{
    public event Action Dead;
    private int _current;
    private int _max = 100;
    private int _min = 0;

    public Health()
    {
        _current = _max;
    }

    public Health(int maxVolume)
    {
        _current = maxVolume;
    }

    public bool TryRemove(int value)
    {
        bool isDone = true;

        if (_current >= value)
        {
            _current -= value;

            if (_current == _min)
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
