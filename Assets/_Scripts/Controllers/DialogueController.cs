using Assets._Scripts.BaseInfos;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
	//dialogue box
	[SerializeField]
	private GameObject dialogueBox;
	private TextMeshProUGUI NPCName;
	private TextMeshProUGUI NPCDialogue;
	private Image boxImage;
	private readonly float defaultTypeSpeed = 0.02f;

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
		{ new HashSet<char>(){',', ';', ':'}, 0.1f}
	};

	//controllers
	private bool endOfconversation;
	private bool isTyping;
	private bool isDelayed = false;

	//dependencies
	private Player playerScript;
	private Controller2D controller2D;

	private void Awake()
	{
		playerScript = GameObject.FindGameObjectWithTag(Constants.Tags.Player).GetComponent<Player>();
		controller2D = GameObject.FindGameObjectWithTag(Constants.Tags.Player).GetComponent<Controller2D>();

		var temp = dialogueBox.GetComponentsInChildren<TextMeshProUGUI>();
		NPCName = temp[0];
		NPCDialogue = temp[1];

		boxImage = dialogueBox.GetComponent<Image>();
	}

	public void DisplayNextParagraph(DialogueText dialogueText)
	{
		//handle empty paragraphs
		if (paragraphs.Count == 0)
		{
			if (!endOfconversation && controller2D.info.bottomCollision)
				StartConversation(dialogueText);

			else if (endOfconversation && !isTyping)
			{
				EndConversation();
				return;
			}
			else
				return;
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
		dialogueBox.SetActive(true);
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

		dialogueBox.SetActive(false);
	}

	private IEnumerator TypeDialogue(string name, string text)
	{
		//assigning text and color values 
		isTyping = true;
		var visibleCharactersCount = 0;

		boxImage.material.color = Color.black;
		var speakerColor = NPCsColors.GetColor(name);
		NPCDialogue.color = speakerColor;
		NPCName.color = speakerColor;

		NPCName.text = name;
		NPCDialogue.text = text;
		NPCDialogue.maxVisibleCharacters = visibleCharactersCount;

		//displaying text letter by letter
		var lastCharacter = '.';

		foreach (var c in text)
		{
			var pauseDuration = defaultTypeSpeed;
			visibleCharactersCount++;
			NPCDialogue.maxVisibleCharacters = visibleCharactersCount;

			//pause after punctuations
			foreach (KeyValuePair<HashSet<char>, float> category in punctuations)
			{
				if (c != lastCharacter && category.Key.Contains(c))
					pauseDuration = category.Value;
			}

			lastCharacter = c;
			yield return new WaitForSeconds(pauseDuration);
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
