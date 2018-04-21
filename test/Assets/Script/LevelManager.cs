using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

class CharacterData
{
    // Data
    public int Life;
    public int Strength;
    public int MovementSpeed;
    public float ThrowSpeed;
    public Character.ThrowDirection Direction;
    public GameObject Item;

    public bool isNull = true;

    public void CopyData(Character c)
    {
        this.Life = c.Life;
        this.Strength = c.Strength;
        this.MovementSpeed = c.MovementSpeed;
        this.ThrowSpeed = c.ThrowSpeed;
        this.Direction = c.Direction;
        this.Item = c.Item;

        isNull = false;
    }

    public void WriteData(Character c)
    {
        c.Life = this.Life;
        c.Strength = this.Strength;
        c.MovementSpeed = this.MovementSpeed;
        c.ThrowSpeed = this.ThrowSpeed;
        c.Direction = this.Direction;
        c.Item = this.Item;
    }
};

public class LevelManager : MonoBehaviour
{
    private LevelManager instance;

    private int LevelCount; // number of level achieved 

    private string CurrentLevel;
    private int CurrentLevelMaxEnemies;
    private int CurrentLevelEnemyNumber;
    private Grid CurrentGrid;
    private bool WasLastAnswerCorrect = true;

    public int RandomLevelIndexMin = 0;
    public int RandomLevelIndexMax = -1;

    public GameObject templatePlayerCharacter;
    public GameObject templateEnemyCharacter;

    private CharacterData characterData = new CharacterData();

    #region Methods

    public void ShowStartMenu()
    {
        // TODO
        // Set template character
        //characterData.CopyData(templatePlayerCharacter.GetComponent<Character>());

        // temporary
        NextLevel();
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
        // TODO 
        // Find next level name by using random
        int randIndex = UnityEngine.Random.Range(RandomLevelIndexMin, RandomLevelIndexMax);
        string randLevelName = "level" + randIndex;

        LoadLevel("Scenes/Levels/" + randLevelName);
    }

    private void UnloadLevel(string levelName)
    {
        // Store player data first
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player.GetComponent<Character>())
            this.characterData.CopyData(player.GetComponent<Character>());

#pragma warning disable CS0618 // Type or member is obsolete
        SceneManager.UnloadScene(levelName);
#pragma warning restore CS0618 // Type or member is obsolete
    }

    private void LoadLevel(string levelName)
    {
        // Load level
        //SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName(levelName));
        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name.StartsWith("level"))
        {
            CurrentGrid = FindObjectOfType<Grid>();
            Character.CurrentLevel = CurrentGrid;

            // Spawn player
            SpawnPlayer();

            // Spawn enemies
            SpawnRandomEnemies();
        }
    }


    private void SpawnPlayer()
    {
        Debug.Log("Spawn player start");
        GameObject o = Instantiate(templatePlayerCharacter) as GameObject;
        Character c = o.GetComponent<Character>();
        if (c != null)
        {
            if (!characterData.isNull)
                characterData.WriteData(c);
        }
        c.GetComponent<SpriteRenderer>().sortingOrder = 50;

        Debug.Log("Spawn player end");
    }

    private void SpawnRandomEnemies()
    {
        int nEnemiesToSpawn = 10; // TODO
        CurrentLevelEnemyNumber = nEnemiesToSpawn;

        if (!WasLastAnswerCorrect)
            nEnemiesToSpawn *= 2;

        for (int i = 0; i < nEnemiesToSpawn; ++i)
        {
            // TODO: determine random position
            GameObject o = Instantiate(templateEnemyCharacter) as GameObject;
            o.GetComponent<SpriteRenderer>().sortingOrder = 50;

            o.GetComponent<Character>().destroyedEvent += HandleEnemyDestroyed;
        }
    }

    private void HandleEnemyDestroyed()
    {
        CurrentLevelEnemyNumber--;
        if (CurrentLevelEnemyNumber <= 0)
        {
            //UpdateCurrentLevelMap();
        }
    }

    #endregion

    #region UnityBehavior

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        // Connections
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Show start menu
        ShowStartMenu();
    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion
}
