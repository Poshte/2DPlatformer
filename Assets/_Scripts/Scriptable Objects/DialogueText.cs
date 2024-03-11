using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/New Dialogue Container")]
public class DialogueText : ScriptableObject
{
	//TO DO
	//Capsulate public fields

	public string[] speakerNames;

	[TextArea(5, 10)]
	public string[] paragraphs;
}
