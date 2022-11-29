using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    Text Level;

    Image HealthSlider;

    Image ExpSlider;

    private void Awake()
    {
        Level = transform.GetChild(2).GetComponent<Text>();

        HealthSlider = transform.GetChild(0).GetComponent<Image>();

        ExpSlider = transform.GetChild(1).GetComponent<Image>();
    }

    private void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        var playerStat = GameManager.Instance.playerStats;

        float healthPercent = (float)playerStat.CurrentHealth / playerStat.MaxHealth;
        HealthSlider.fillAmount = healthPercent;

        float expPercent = (float)playerStat.CurrentExp / playerStat.BaseExp;
        ExpSlider.fillAmount = expPercent;

        Level.text = "Level   " + playerStat.CurrentLevel.ToString("00");
    }
}
