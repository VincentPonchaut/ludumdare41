using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonitorPlayerHealth : MonoBehaviour
{
    public Character player;

    public Image HealthBar;
    public Text HealthText;

    private void Update()
    {
        if (player != null)
            return;

        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController == null)
            return;

        player = playerController.gameObject.GetComponent<Character>();
        Refresh();
        player.damagedEvent += UpdateHealth;
    }

    private void UpdateHealth(Character damagedCharacter, int damageAmount)
    {
        float hRatio = player.Life / 100.0f;
        HealthBar.rectTransform.localScale = new Vector3(hRatio, 1, 1);

        int hRatioInt = (int)(hRatio * 100.0f);
        HealthText.text = hRatioInt + "%";
    }

    public void Refresh()
    {
        UpdateHealth(null, 0);
    }
}
