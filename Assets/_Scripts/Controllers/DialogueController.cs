using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
	//dialogues
	private TextMeshProUGUI NPCName;
	private TextMeshProUGUI NPCDialogue;
	[SerializeField] private float typeSpeed = 20f;

	//paragraphs
	private readonly Queue<string> paragraphs = new();
	private string currentParagraph;
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
			typeDialogueCoroutine = StartCoroutine(TypeDialogue(currentParagraph));
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

		NPCName.text = dialogueText.speakerName;

		foreach (var p in dialogueText.paragraphs)
			paragraphs.Enqueue(p);
	}

	private void EndConversation()
	{
		paragraphs.Clear();
		gameObject.SetActive(false);
		playerScript.enabled = true;
		conversationEnded = false;
	}

	private IEnumerator TypeDialogue(string p)
	{
		isTyping = true;
		var visibleCharactersCount = 0;

		NPCDialogue.text = p;
		NPCDialogue.maxVisibleCharacters = visibleCharactersCount;

		foreach (var c in p)
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
