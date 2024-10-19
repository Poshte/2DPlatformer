using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
	[SerializeField]
	private List<TKey> keys = new();

	[SerializeField]
	private List<TValue> values = new();

	public void OnBeforeSerialize()
	{
		keys.Clear();
		values.Clear();

		foreach (var pair in this)
		{
			keys.Add(pair.Key);
			values.Add(pair.Value);
		}
	}

	public void OnAfterDeserialize()
	{
		this.Clear();

		for (int i = 0; i < keys.Count; i++)
		{
			this.Add(keys[i], values[i]);
		}
	}
}
