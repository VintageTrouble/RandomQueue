namespace RandomQueue;

public interface IRandomQueue<T>
{
    int Count { get; }
    T Current { get; }
    bool CanGetPrev { get; }

    T Next();
    T Prev();
    void Add(T item);
    void AddRange(IEnumerable<T> items);
    void Refresh();
    T Delete();
    void Clear();
}
