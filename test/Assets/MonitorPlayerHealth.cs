using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonitorPlayerHealth : MonoBehaviour
{
    public Character player;

    public Image HealthBar;
    public Text HealthText;

    // Update is called once per frame
    void Update()
    {
        if (player == null)
            player = FindObjectOfType<PlayerController>().gameObject.GetComponent<Character>();

        float hRatio = player.Life / 100.0f;
        HealthBar.rectTransform.localScale = new Vector3(hRatio, 1, 1);

        int hRatioInt = (int)(hRatio * 100.0f);
        HealthText.text = hRatioInt + "%";
    }
}
