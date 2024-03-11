using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : NPC, ITalkable, IWalkable
{
	[SerializeField] private DialogueText dialogueText;
	[SerializeField] private DialogueController dialogueController;

	public override void Interact()
	{
		Talk(dialogueText);
	}

	public void Talk(DialogueText dialogueText)
	{
		dialogueController.DisplayNextParagraph(dialogueText);
	}

	public void Walk()
	{
		throw new System.NotImplementedException();
	}
}
