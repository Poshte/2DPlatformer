using Assets._Scripts.BaseInfos;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour
{
	private LeafController leafController;
	private int[] adjacentLeavesIndex;
	public int Index;

	private void Awake()
	{
		leafController = GetComponentInParent<LeafController>();
	}

	void Start()
	{
		adjacentLeavesIndex = GetAdjacentLeavesIndex();
	}

	private int[] GetAdjacentLeavesIndex()
	{
		var result = new List<int>
		{
			Index,
			Index + 1,
			Index - 1
		};

		result.Remove(-1);
		return result.ToArray();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag(Constants.Tags.Player))
		{
			leafController.EnableAdjacentLeaves(adjacentLeavesIndex);
		}
	}
}