using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    public Sprite WinScreenMeatarian;
    public Sprite WinScreenVegetarian;

    // Use this for initialization
    void Start()
    {
        if (LevelManager.RequestedGameMode == "Meatarian")
        {
            GetComponent<Image>().sprite = WinScreenMeatarian;
        }
        else
        {
            GetComponent<Image>().sprite = WinScreenVegetarian;
        }
    }
}
