using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
	//dialogue box
	private TextMeshProUGUI NPCName;
	private TextMeshProUGUI NPCDialogue;
	private Material boxMaterial;
	[SerializeField] private float typeSpeed = 5f;

	//paragraphs
	private readonly Queue<string> names = new();
	private readonly Queue<string> paragraphs = new();
	private string currentParagraph;
	private string currentName;
	private Coroutine typeDialogueCoroutine;

	//dependencies
	private Player playerScript;

	//controllers
	private bool conversationEnded;
	private bool isTyping;

	//constants
	private const float maxTypeTime = 0.1f;

	private void Awake()
	{
		playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

		var temp = GetComponentsInChildren<TextMeshProUGUI>();
		NPCName = temp[0];
		NPCDialogue = temp[1];

		boxMaterial = gameObject.GetComponent<Image>().material;
	}

	public void DisplayNextParagraph(DialogueText dialogueText)
	{
		if (paragraphs.Count == 0)
		{
			if (!conversationEnded)
				StartConversation(dialogueText);

			else if (conversationEnded && !isTyping)
			{
				EndConversation();
				return;
			}
		}

		if (!isTyping)
		{
			currentParagraph = paragraphs.Dequeue();
			currentName = names.Dequeue();

			typeDialogueCoroutine = StartCoroutine(TypeDialogue(currentName, currentParagraph));
		}
		else
		{
			FinishParagraph(currentParagraph);
		}

		if (paragraphs.Count == 0)
			conversationEnded = true;
	}

	private void StartConversation(DialogueText dialogueText)
	{
		gameObject.SetActive(true);
		playerScript.enabled = false;

		foreach (var n in dialogueText.speakerNames)
			names.Enqueue(n);

		foreach (var p in dialogueText.paragraphs)
			paragraphs.Enqueue(p);
	}

	private void EndConversation()
	{
		paragraphs.Clear();
		names.Clear();
		gameObject.SetActive(false);
		playerScript.enabled = true;
		conversationEnded = false;
	}

	private IEnumerator TypeDialogue(string name, string text)
	{
		isTyping = true;
		var visibleCharactersCount = 0;

		boxMaterial.color = NPCsColors.GetColor(name);
		NPCName.text = name;
		NPCDialogue.text = text;
		NPCDialogue.maxVisibleCharacters = visibleCharactersCount;

		foreach (var c in text)
		{
			visibleCharactersCount++;
			NPCDialogue.maxVisibleCharacters = visibleCharactersCount;

			yield return new WaitForSeconds(maxTypeTime / typeSpeed);
		}

		isTyping = false;
	}

	private void FinishParagraph(string p)
	{
		StopCoroutine(typeDialogueCoroutine);
		NPCDialogue.maxVisibleCharacters = p.Length;
		isTyping = false;
	}
}
