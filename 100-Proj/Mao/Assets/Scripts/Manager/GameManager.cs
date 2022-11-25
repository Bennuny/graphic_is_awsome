using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public CharacterStat playerStats;

    List<IEndGameObserver> _endGameObservers = new List<IEndGameObserver>();

    public void RigisterPlayer(CharacterStat player)
    {
        playerStats = player;
    }

    public void AddObserver(IEndGameObserver observer)
    {
        _endGameObservers.Add(observer);
    }

    public void RemoveObserver(IEndGameObserver observer)
    {
        _endGameObservers.Remove(observer);
    }

    public void NotifiyObserver()
    {
        foreach (var obj in _endGameObservers)
        {
            obj.EndNotifiy();
        }
    }
}
