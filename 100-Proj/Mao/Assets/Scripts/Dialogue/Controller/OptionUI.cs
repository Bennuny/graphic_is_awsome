using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public Text optionText;

    private Button thisButton;

    private DialoguePiece currentPiece;

    private void Awake()
    {
        thisButton = GetComponent<Button>();
    }

    //public void UpdateOption(DialoguePiece piece, DialoguePiece)

    public void OnOptionClick()
    {
        var newTask = new QuestManager.QuestTask
        {
            questData = Instantiate(currentPiece.quest)
        };

        if (currentPiece.quest != null)
        {

        }
    }
}
