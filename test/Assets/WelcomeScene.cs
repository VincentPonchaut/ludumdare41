﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WelcomeScene : MonoBehaviour
{
    public void StartGame(bool asVegetarian)
    {
        LevelManager.RequestedGameMode = asVegetarian ? "Vegetarian" :
                                                        "Meatarian" ;
        SceneManager.LoadScene("Init", LoadSceneMode.Single);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
