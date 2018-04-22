﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#region Utility Classes
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

[Serializable]
public class QuestionList
{
    public Question[] questions;

    public static QuestionList CreateFromJson(string jsonString)
    {
        return JsonUtility.FromJson<QuestionList>(jsonString);
    }
};

[Serializable]
public class Question
{
    public string question;
    public string[] content;
    public int correct;
};
#endregion

public class LevelManager : MonoBehaviour
{
    #region Attributes
    private static LevelManager instance;
    public static LevelManager Instance
    {
        get
        {
            return instance;
        }

        set
        {
            instance = value;
        }
    }

    // General game info/params
    private int levelCount; // number of level achieved 

    public int NbEnemyPerLevel;
    public int RandomLevelIndexMin = 0;
    public int RandomLevelIndexMax = -1;

    public GameObject TemplatePlayerCharacter;
    public GameObject TemplateEnemyCharacter;

    // Current level info
    private string currentLevelName;
    private int currentLevelEnemyNumber;
    private Grid currentGrid;
    private Character currentPlayer;
    private SpawnManager currentSpawnManager;
    private CharacterData characterData = new CharacterData();

    // Damage mechanics
    int damageBalance;

    // User Interface
    public GameObject MainUI;

    #endregion

    #region Methods

    private QuestionList questionList;
    private Question activeQuestion;
    public GameObject QuestionOverlay;
    public GameObject QuestionText;
    public GameObject Ans1Text;
    public GameObject Ans2Text;
    public GameObject Ans3Text;
    public GameObject Ans4Text;
    public Text AnswerValueText;

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
        NextLevel();
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
            currentGrid = FindObjectOfType<Grid>();
            Character.CurrentLevel = currentGrid;

            currentSpawnManager = FindObjectOfType<SpawnManager>();

            // Spawn player
            SpawnPlayer();

            // Spawn enemies
            SpawnRandomEnemies();
        }
    }


    private void SpawnPlayer()
    {
        GameObject o = null;
        if (currentSpawnManager != null)
        {
            o = Instantiate(TemplatePlayerCharacter,
                            currentSpawnManager.PlayerSpawnPoint.transform.position,
                            Quaternion.identity) as GameObject;
        }
        else
        {
            o = Instantiate(TemplatePlayerCharacter) as GameObject;
        }
        currentPlayer = o.GetComponent<Character>();

        // Restore previous level data
        if (!characterData.isNull)
            characterData.WriteData(currentPlayer);

        // Ensure character will be visible
        currentPlayer.GetComponent<SpriteRenderer>().sortingOrder = 50;

        // Connect the player's damaged event to our handler
        currentPlayer.damagedEvent += OnPlayerDamaged;
    }

    private void OnPlayerDamaged(Character damagedCharacter, int damageAmount)
    {
        this.damageBalance = damageAmount;
        PauseGame();
    }

    private void SpawnRandomEnemies()
    {
        int nEnemiesToSpawn = NbEnemyPerLevel;
        currentLevelEnemyNumber = nEnemiesToSpawn;

        int nSpawnPoints = 0;
        int iSpawnPoint = 0; // index

        if (currentSpawnManager != null)
        {
            nSpawnPoints = currentSpawnManager.EnemySpawnPoints.Length;
        }

        for (int i = 0; i < nEnemiesToSpawn; ++i)
        {
            GameObject o = null;

            if (nSpawnPoints > 0)
            {
                Transform spawnTransform = currentSpawnManager.EnemySpawnPoints[iSpawnPoint].transform;

                // Prepare next iteration to use next valid spawn point
                iSpawnPoint++;
                if (iSpawnPoint == nSpawnPoints ||
                    currentSpawnManager.EnemySpawnPoints[iSpawnPoint] == null)
                    iSpawnPoint = 0;

                // Perform instantiation
                o = Instantiate(TemplateEnemyCharacter,
                                spawnTransform.position,
                                Quaternion.identity) as GameObject;
            }
            else
            {
                o = Instantiate(TemplateEnemyCharacter) as GameObject;
            }
            
            o.GetComponent<SpriteRenderer>().sortingOrder = 50;
            o.GetComponent<Character>().destroyedEvent += HandleEnemyDestroyed;
        }
    }

    private void HandleEnemyDestroyed()
    {
        currentLevelEnemyNumber--;
        if (currentLevelEnemyNumber <= 0)
        {
            GameObject ExitTilemap = GameObject.FindGameObjectWithTag("Exit");
            if (ExitTilemap != null)
                ExitTilemap.GetComponent<ExitScript>().ChangeExitSprite();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;

        // TODO: Random question

        SetupRandomQuestion();
        QuestionOverlay.SetActive(true);
        //pausePanel.SetActive(true);
        //Disable scripts that still work while timescale is set to 0
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        AnswerValueText.gameObject.SetActive(false);
        QuestionOverlay.SetActive(false);
        //pausePanel.SetActive(false);
        //enable the scripts again
    }

    #endregion

    #region QuestionManagement ----------------------------------------------------------------------------------

    public void InitializeQuestions()
    {
        TextAsset questionsFileJson = Resources.Load("questions") as TextAsset;
        this.questionList = QuestionList.CreateFromJson(questionsFileJson.text);

        Debug.Log("Loaded " + this.questionList.questions.Length + " questions from JSON file");
    }

    public void SetupRandomQuestion()
    {
        int questionIndex = UnityEngine.Random.Range(0, this.questionList.questions.Length - 1);

        Question theQuestion = this.questionList.questions[questionIndex];
        SetupQuestion(theQuestion);
    }

    public void SetupQuestion(Question theQuestion)
    {
        Text t = this.QuestionText.GetComponent<Text>();
        t.text = theQuestion.question;

        Text ans1Text = this.Ans1Text.GetComponent<Text>();
        Text ans2Text = this.Ans2Text.GetComponent<Text>();
        Text ans3Text = this.Ans3Text.GetComponent<Text>();
        Text ans4Text = this.Ans4Text.GetComponent<Text>();

        ans1Text.text = theQuestion.content[0];
        ans2Text.text = theQuestion.content[1];
        ans3Text.text = theQuestion.content[2];
        ans4Text.text = theQuestion.content[3];

        activeQuestion = theQuestion;
    }

    public void HandleQuestionAnswer(int answer)
    {
        // Handle answer
        if (activeQuestion.correct == answer)
        {
            Debug.Log("CORRECT");
            HandleCorrectAnswer();
        }
        else
        {
            HandleWrongAnswer();
            Debug.Log("OHNO");
        }

        // Then resume playing
        StartCoroutine(ResumeAfterSeconds());
    }

    IEnumerator ResumeAfterSeconds()
    {
        yield return new WaitForSecondsRealtime(1);
        LevelManager.Instance.ResumeGame();
    }

    private void HandleCorrectAnswer()
    {
        // Show "Correct" text
        AnswerValueText.text = "Correct!";
        AnswerValueText.color = Color.green;
        AnswerValueText.gameObject.SetActive(true);

        // Heal player on correct answer
        currentPlayer.Life += this.damageBalance;
    }

    private void HandleWrongAnswer()
    {
        // Show "Wrong" text
        AnswerValueText.text = "Wrong!";
        AnswerValueText.color = Color.red;
        AnswerValueText.gameObject.SetActive(true);

        // Hurt player even more on wrong answer
        currentPlayer.Life -= this.damageBalance;
        if (currentPlayer.Life <= 0)
            Destroy(currentPlayer.gameObject); // and game over please
    }

    #endregion

    #region UnityBehavior ----------------------------------------------------------------------------------

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        // Connections
        SceneManager.sceneLoaded += OnSceneLoaded;

        DontDestroyOnLoad(this.MainUI);
        InitializeQuestions();

        // Show start menu
        ShowStartMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p"))
            PauseGame();
        else if (Input.GetKeyDown("c"))
            ResumeGame();
    }
    #endregion ----------------------------------------------------------------------------------
}
