using System.Collections;

namespace Collections.HashList;
/// <summary>
/// This Class is Based on the GetHashcode Methode of The given Class T.
/// The Methode needs to be overwirdden if a uniquevalue Entrie can be found.
/// </summary>
/// <typeparam name="T"></typeparam>
public class HashList<T> : IList<T>
{
    private readonly Dictionary<int, T> _dictionary;

    public HashList()
    {
        _dictionary = [];
    }

    public HashList(IEnumerable<T> values)
    {
        _dictionary = [];
        foreach (T value in values)
        {
            Add(value);
        }
    }

    public T this[int index]
    {
        get
        {
            return _dictionary.TryGetValue(index, out var item) ? item : throw new KeyNotFoundException("Index not found.");
        }
        set
        {
            _dictionary[index] = value;
        }
    }

    public int Count => _dictionary.Count;

    public bool IsReadOnly => false;

    public void Add(T item)
    {
        int hash = item!.GetHashCode();
        if (!_dictionary.TryAdd(hash, item))
        {
            throw new ArgumentException("An element with the same hash already exists.");
        }
    }

    public void Clear()
    {
        _dictionary.Clear();
    }

    public bool Contains(T item)
    {
        return _dictionary.ContainsKey(item!.GetHashCode());
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        ArgumentNullException.ThrowIfNull(array);

        if (arrayIndex < 0 || arrayIndex > array.Length)
            throw new ArgumentOutOfRangeException(nameof(arrayIndex));

        if (array.Length - arrayIndex < _dictionary.Count)
            throw new ArgumentException("The target array is too small.");

        foreach (var item in _dictionary.Values)
        {
            array[arrayIndex++] = item;
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _dictionary.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int IndexOf(T item)
    {
        int hash = item!.GetHashCode();
        return _dictionary.ContainsKey(hash) ? hash : -1;
    }

    public void Insert(int index, T item)
    {
        throw new NotSupportedException("Inserting by index is not supported in HashList.");
    }

    public bool Remove(T item)
    {
        return _dictionary.Remove(item!.GetHashCode());
    }

    public void RemoveAt(int index)
    {
        if (!_dictionary.Remove(index))
        {
            throw new KeyNotFoundException("Index not found.");
        }
    }
}
