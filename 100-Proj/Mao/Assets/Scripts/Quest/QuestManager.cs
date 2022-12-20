using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class QuestManager : Singleton<QuestManager>
{

    public class QuestTask
    {
        public QuestData_SO questData;

        public bool IsStarted { get { return questData.isStarted;  }  set { questData.isStarted = value; } }

        public bool IsComplete { get { return questData.isComplete; } set { questData.isComplete = value; } }

        public bool IsFinised { get { return questData.isFinished; } set { questData.isFinished = value; } }
    }


    public List<QuestTask> tasks = new List<QuestTask>();

    public void LoadQuestManager()
    {
        var questCount = PlayerPrefs.GetInt("QuestCount");

        for (int i = 0; i < questCount; i++)
        {
            var newQuest = ScriptableObject.CreateInstance<QuestData_SO>();

            SaveManager.Instance.Load(newQuest, "task" + i);
        }
    }

    public void SaveQuestManager()
    {
        PlayerPrefs.SetInt("QuestCount", tasks.Count);

        for (int i = 0; i < tasks.Count; i++)
        {
            SaveManager.Instance.Save(tasks[i].questData, "task" + i);
        }
    }


    public bool HaveQuest(QuestData_SO data)
    {
        if (data != null)
        {
            return tasks.Any(q => q.questData.questName == data.questName);
        }

        return false;
    }

    public QuestTask GetTask(QuestData_SO data)
    {
        return tasks.Find(q => q.questData.questName == data.questName);
    }

    public void CheckQuestProgress()
    {
        //var finish = tasks.Where(q => q.requ)
    }
}

