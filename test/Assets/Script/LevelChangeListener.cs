using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelChangeListener : MonoBehaviour
{
    public void Refresh()
    {
        if (LevelManager.Instance == null)
            return;

        Text t = GetComponent<Text>();
        t.text = "Level " + LevelManager.Instance.levelCount + "/" + LevelManager.Instance.MaxLevelCount;
    }
}
