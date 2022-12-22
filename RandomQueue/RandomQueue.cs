namespace RandomQueue;

public class RandomQueue<T> : IRandomQueue<T>
{
    private const ushort CapacityIncreaseMultiplier = 2;

    private readonly Random _random = new();

    private T?[] _array;
    private int _currentIndex = -1;
    private int _prevIndex = -1;
    private int _nextIndex = -1;

    public int Count { get; private set; } = 0;
    public T Current
    {
        get
        {
            if (Count == 0)
                throw new EmptyQueueException();

            if (_currentIndex == -1)
                Refresh();

            return _array[_currentIndex]!;
        }
    }
    public bool CanGetPrev => _prevIndex != -1;

    public RandomQueue(int size = 1)
    {
        if (size < 1)
            throw new ArgumentOutOfRangeException(nameof(size));

        _array = new T[size];
    }

    public RandomQueue(IEnumerable<T> items)
        : this(items.Count())
    {
        foreach (var item in items)
            Add(item);
    }

    public void Add(T item)
    {
        if (Count == _array.Length)
            IncreaseArraySize();

        _array[Count] = item;

        Count++;
    }

    public void AddRange(IEnumerable<T> items)
    {
        foreach (var item in items)
            Add(item);
    }

    public void Refresh()
    {
        _prevIndex = -1;
        _currentIndex = GetRandomIndex(0, Count, _currentIndex, _prevIndex);
        _nextIndex = GetRandomIndex(0, Count, _currentIndex, _prevIndex);
    }

    public void Clear()
    {
        _array = new T?[1];
        _currentIndex = -1;
        _prevIndex = -1;
        _nextIndex = -1;
    }

    public T Delete()
    {
        _array[_currentIndex] = default;
        Count--;

        if (_currentIndex != Count - 1)
            BalanceArray();

        Refresh();

        return Current;
    }

    public T Next()
    {
        _prevIndex = _currentIndex;
        _currentIndex = _nextIndex;
        _nextIndex = GetRandomIndex(0, Count, _currentIndex, _prevIndex);

        return Current;
    }

    public T Prev()
    {
        if (Count == 0)
            throw new EmptyQueueException();

        if (_prevIndex == -1)
            throw new IndexOutOfRangeException("Previous item does not exists.");

        _nextIndex = GetRandomIndex(0, Count, _currentIndex, _prevIndex);
        _currentIndex = _prevIndex;
        _prevIndex = -1;

        return Current;
    }

    private void IncreaseArraySize()
    {
        var array = new T[_array.Length * CapacityIncreaseMultiplier];

        for (int i = 0; i < Count; i++)
        {
            array[i] = _array[i]!;
        }

        _array = array;
    }

    private void BalanceArray()
    {
        for (int i = _currentIndex; i < Count - 1; i++)
        {
            _array[i] = _array[i + 1];
        }
    }

    private int GetRandomIndex(int min, int max, params int[] excepts)
    {
        var result = _random.Next(min, max);

        if (!excepts.Contains(result) || max - min <= 2)
            return result;

        var mid = (max - min) / 2 + min;

        if (result >= mid)
        {
            excepts = excepts.Where(x => x < mid).ToArray();
            return GetRandomIndex(min, mid, excepts);
        }

        excepts = excepts.Where(x => x >= mid).ToArray();
        return GetRandomIndex(mid + 1, max, excepts);
    }
}