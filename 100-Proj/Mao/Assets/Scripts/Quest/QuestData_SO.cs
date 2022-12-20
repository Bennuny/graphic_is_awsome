using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest/Quest Data")]
public class QuestData_SO : ScriptableObject
{
    public class QuestRequire
    {
        public string name;

        public int requireAmount;

        public int currentAmount;
    }

    public string questName;

    [TextArea]
    public string description;


    public bool isStarted;

    public bool isComplete;

    public bool isFinished;


    public List<QuestRequire> questRequires = new List<QuestRequire>();

    public List<InventoryItem> rewards = new List<InventoryItem>();



    public void GiveReward()
    {
        foreach (var reward in rewards)
        {
            if (reward.Amount < 0)
            {
                int requireCount = Mathf.Abs(reward.Amount);
            }
        }
    }
}
