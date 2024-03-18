using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
	//dialogue box
	private TextMeshProUGUI NPCName;
	private TextMeshProUGUI NPCDialogue;
	private Image boxImage;
	private readonly float typeSpeed = 0.01f;

	//paragraphs
	private readonly Queue<string> names = new();
	private readonly Queue<string> paragraphs = new();
	private string currentParagraph;
	private string currentName;
	private Coroutine typeDialogueCoroutine;

	//punctuations
	private readonly Dictionary<HashSet<char>, float> punctuations = new()
	{
		{ new HashSet<char>(){'.', '?', '!'}, 0.3f },
		{ new HashSet<char>(){',', ';', ':'}, 0.05f}
	};

	//dependencies
	private Player playerScript;

	//controllers
	private bool endOfconversation;
	private bool isTyping;
	private bool isDelayed = false;

	private void Awake()
	{
		playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

		var temp = GetComponentsInChildren<TextMeshProUGUI>();
		NPCName = temp[0];
		NPCDialogue = temp[1];

		boxImage = gameObject.GetComponent<Image>();
	}

	public void DisplayNextParagraph(DialogueText dialogueText)
	{
		//handle empty paragraphs
		if (paragraphs.Count == 0)
		{
			if (!endOfconversation)
				StartConversation(dialogueText);

			else if (endOfconversation && !isTyping)
			{
				EndConversation();
				return;
			}
		}

		//handle writting paragrpahs
		if (!isTyping && !isDelayed)
		{
			currentParagraph = paragraphs.Dequeue();
			currentName = names.Dequeue();

			//delay if text is empty
			if (string.IsNullOrWhiteSpace(currentParagraph))
			{
				StartCoroutine(Delay(currentName));
			}
			else
			{
				boxImage.enabled = true;
				typeDialogueCoroutine = StartCoroutine(TypeDialogue(currentName, currentParagraph));
			}
		}
		else
		{
			FinishParagraph(currentParagraph);
		}

		if (paragraphs.Count == 0)
			endOfconversation = true;
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
		endOfconversation = false;

		playerScript.enabled = true;
		playerScript.GetMovementInput(Vector2.zero);

		gameObject.SetActive(false);
	}

	private IEnumerator TypeDialogue(string name, string text)
	{
		isTyping = true;
		var visibleCharactersCount = 0;

		boxImage.material.color = Color.black;
		var speakerColor = NPCsColors.GetColor(name);
		NPCDialogue.color = speakerColor;
		NPCName.color = speakerColor;

		NPCName.text = name;
		NPCDialogue.text = text;
		NPCDialogue.maxVisibleCharacters = visibleCharactersCount;

		var lastCharacter = '.';
		foreach (var c in text)
		{
			visibleCharactersCount++;
			NPCDialogue.maxVisibleCharacters = visibleCharactersCount;

			//pause after punctuations
			foreach (KeyValuePair<HashSet<char>, float> category in punctuations)
			{
				if (c != lastCharacter && category.Key.Contains(c))
					yield return new WaitForSeconds(category.Value);
				else
					yield return new WaitForSeconds(typeSpeed);

				lastCharacter = c;
			}
		}

		isTyping = false;
	}

	private void FinishParagraph(string p)
	{
		StopCoroutine(typeDialogueCoroutine);
		NPCDialogue.maxVisibleCharacters = p.Length;
		isTyping = false;
	}

	private IEnumerator Delay(string delay)
	{
		boxImage.enabled = false;
		NPCName.text = "";
		NPCDialogue.text = "";
		isDelayed = true;

		yield return new WaitForSeconds(float.Parse(delay));

		isDelayed = false;
	}
}
