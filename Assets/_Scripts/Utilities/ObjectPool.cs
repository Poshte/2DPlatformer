using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
	private readonly Queue<T> pool = new Queue<T>();
	private readonly T prefab;

	public ObjectPool(T prefab, int initialSize)
	{
		this.prefab = prefab;

		for (int i = 0; i < initialSize; i++)
		{
			var obj = GameObject.Instantiate(this.prefab);
			obj.gameObject.SetActive(false);
			pool.Enqueue(obj);
		}
	}

	public T GetObject()
	{
		if (pool.Count == 0)
		{
			return GameObject.Instantiate(prefab);
		}

		var obj = pool.Dequeue();
		obj.gameObject.SetActive(true);
		return obj;
	}

	public void ReturnObject(T obj)
	{
		obj.gameObject.SetActive(false);
		pool.Enqueue(obj);
	}
}
