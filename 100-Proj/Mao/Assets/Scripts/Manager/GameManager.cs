using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : Singleton<GameManager>
{
    public CharacterStat playerStats;

    CinemachineFreeLook followCamera;

    List<IEndGameObserver> _endGameObservers = new List<IEndGameObserver>();


    // ! 如果是DontDestroyOnLoad的话，必须是Hierarchy窗口的物体，不能是子物体；
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void RigisterPlayer(CharacterStat player)
    {
        playerStats = player;

        followCamera = FindObjectOfType<CinemachineFreeLook>();

        if (followCamera != null)
        {
            followCamera.Follow = playerStats.transform.GetChild(2);
            followCamera.LookAt = playerStats.transform.GetChild(2);
        }
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

    //

    public Transform GetEntrance()
    {
        foreach (var item in FindObjectsOfType<TransitionDestination>())
        {
            if (item.transtionTag == TransitionDestination.DestinationTag.ENTER)
            {
                return item.transform;
            }
        }

        return null;
    }
}
