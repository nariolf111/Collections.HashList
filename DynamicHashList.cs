using System.Collections;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text;

namespace Collections.HashList;
internal class DynamicHashList<T> :IList<T>
{
    private readonly Dictionary<int, T> _dictionary;

    public DynamicHashList()
    {
        _dictionary = [];
    }
    public DynamicHashList(IEnumerable<T> values)
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
        int hash = GetHashCode(item);
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
        return _dictionary.ContainsKey(GetHashCode(item));
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
        int hash = GetHashCode(item);
        return _dictionary.ContainsKey(hash) ? hash : -1;
    }

    public void Insert(int index, T item)
    {
        throw new NotSupportedException("Inserting by index is not supported in HashList.");
    }

    public bool Remove(T item)
    {
        return _dictionary.Remove(GetHashCode(item));
    }

    public void RemoveAt(int index)
    {
        if (!_dictionary.Remove(index))
        {
            throw new KeyNotFoundException("Index not found.");
        }
    }

    private static int GetHashCode(T element)
    {
        string json = JsonSerializer.Serialize(element);
        byte[] hashBytes = SHA512.HashData(Encoding.UTF8.GetBytes(json));
        return BitConverter.ToInt32(hashBytes, 0);
    }


}
