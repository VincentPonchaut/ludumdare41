using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private int LevelCount; // number of level achieved
    private string CurrentLevel;
    private int CurrentLevelMaxEnemies;

    private int RandomSceneIndexMin = 0;
    private int RandomSceneIndexMax = -1;


    #region Methods

    public void ShowStartMenu()
    {

    }

    public void AttemptExit(/*Vector2 exitPosition*/)
    {
        // TODO
        // Unload current level
        // Load Question level
    }

    private void RestartLevel()
    {

    }

    private void NextLevel()
    {

    }

    private void LoadLevel(string levelName)
    {
        // TODO
        // Load level
        // Spawn player
        // Spawn enemies
        SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
    }

    private void SpawnRandomEnemies()
    {

    }

    #endregion

    #region UnityBehavior
    // Use this for initialization
    void Start()
    {
        // Show start menu
    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion
}
