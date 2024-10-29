using Assets._Scripts.BaseInfos;
using System.Linq;
using UnityEngine;

public class LeafController : MonoBehaviour
{
	private Leaf[] leaves;

	private void Start()
	{
		leaves = GetComponentsInChildren<Leaf>(true);
		Reset();

		GameEvents.Instance.OnJumpButtonPressed += OnJumpButtonPressed;
	}

	private void OnJumpButtonPressed()
	{
		Reset();
	}

	public void EnableAdjacentLeaves(int[] indices)
	{
		foreach (var leaf in leaves)
		{
			if (indices.Contains(leaf.Index))
			{
				leaf.gameObject.SetActive(true);
			}
			else
			{
				leaf.gameObject.SetActive(false);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag(Constants.Tags.Player))
		{
			Reset();
		}
	}

	public void Reset()
	{
		foreach (var leaf in leaves)
		{
			leaf.gameObject.SetActive(false);
		}

		leaves[0].gameObject.SetActive(true);
	}
}
