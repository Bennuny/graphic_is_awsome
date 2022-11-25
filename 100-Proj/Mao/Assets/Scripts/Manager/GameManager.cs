using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public CharacterStat playerStats;

    public void RigisterPlayer(CharacterStat player)
    {
        playerStats = player;
    }

}
