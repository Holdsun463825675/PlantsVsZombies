using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<K, V> : ISerializationCallbackReceiver
{
    [SerializeField] private List<K> keys = new List<K>();
    [SerializeField] private List<V> values = new List<V>();

    private Dictionary<K, V> dictionary = new Dictionary<K, V>();

    public Dictionary<K, V> Dictionary => dictionary;

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        foreach (var pair in dictionary)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        dictionary.Clear();

        for (int i = 0; i < keys.Count && i < values.Count; i++)
        {
            if (keys[i] != null)
            {
                dictionary[keys[i]] = values[i];
            }
        }
    }

    // 添加字典操作方法
    public void Add(K key, V value) => dictionary.Add(key, value);
    public bool ContainsKey(K key) => dictionary.ContainsKey(key);
    public bool Remove(K key) => dictionary.Remove(key);
    public bool TryGetValue(K key, out V value) => dictionary.TryGetValue(key, out value);
    public V this[K key]
    {
        get => dictionary[key];
        set => dictionary[key] = value;
    }

    public int Count => dictionary.Count;
    public void Clear() => dictionary.Clear();
}
