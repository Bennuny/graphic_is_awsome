using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialoguePiece
{
    public string ID;

    public Sprite Image;

    public string Text;

    public List<DailogueOption> options = new List<DailogueOption>();
}
