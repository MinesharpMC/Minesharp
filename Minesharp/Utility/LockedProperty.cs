namespace Minesharp.Utility;

public class LockedProperty<T>
{
    private readonly object lockObject = new();
    private T value;

    public T Value
    {
        get
        {
            lock (lockObject)
            {
                return value;
            }
        }
        set
        {
            lock (lockObject)
            {
                this.value = value;
            }
        }
    }
}