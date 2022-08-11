namespace Minesharp.Utility;

public class ThreadSafeProperty<T>
{
    private readonly object propertyLock = new();

    private T value;

    public T Value
    {
        get
        {
            lock (propertyLock)
            {
                return value;
            }
        }
        set
        {
            lock (propertyLock)
            {
                this.value = value;
            }
        }
    }
}