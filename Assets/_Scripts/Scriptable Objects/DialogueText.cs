using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Dialogue/New Dialogue Container")]
public class DialogueText : ScriptableObject
{
    //TO DO
    //Capsulate public fields

    public string speakerName;

    public Image speakerAvatar;

    [TextArea(5, 10)]
    public string[] paragraphs;
}
