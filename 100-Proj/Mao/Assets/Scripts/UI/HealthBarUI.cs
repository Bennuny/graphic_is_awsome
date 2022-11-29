using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public GameObject HealthBarPrefab;

    public Transform barPoint;

    public bool alwaysVisible;

    public float visibleTime = 3.0f;

    private float timeLeft;

    Image healthSlider;

    Transform UIbar;

    // Camera;
    Transform Cam;

    CharacterStat currentStats;

    private void Awake()
    {
        currentStats = GetComponent<CharacterStat>();

        currentStats.UpdateHealthBarOnAttack += UpdateHealthUI;
    }

    private void OnEnable()
    {
        Cam = Camera.main.transform;

        foreach (Canvas canva in FindObjectsOfType<Canvas>())
        {
            if (canva.renderMode == RenderMode.WorldSpace)
            {
                UIbar = Instantiate(HealthBarPrefab, canva.transform).transform;

                healthSlider = UIbar.GetChild(0).GetComponent<Image>();

                UIbar.gameObject.SetActive(alwaysVisible);
            }
        }
    }

    private void LateUpdate()
    {
        if (UIbar != null)
        {
            UIbar.position = barPoint.position;

            UIbar.forward = -Cam.forward;

            if (timeLeft <= 0 && !alwaysVisible)
            {
                UIbar.gameObject.SetActive(false);
            }
            else
            {
                timeLeft -= Time.deltaTime;
            }
        }
    }

    private void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        if (currentHealth <= 0)
        {
            Destroy(UIbar.gameObject);
        }

        UIbar.gameObject.SetActive(true);

        float sliderPercent = (float) currentHealth / maxHealth;

        healthSlider.fillAmount = sliderPercent;

        timeLeft = visibleTime;
    }
}
