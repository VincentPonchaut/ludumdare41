using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

public class LevelManager : MonoBehaviour
{
    private LevelManager instance;

    private int LevelCount; // number of level achieved 
    public int NbEnemyPerLevel;

    private string CurrentLevel;
    private int CurrentLevelMaxEnemies;
    private int CurrentLevelEnemyNumber;
    private Grid CurrentGrid;

    public int RandomLevelIndexMin = 0;
    public int RandomLevelIndexMax = -1;

    public GameObject TemplatePlayerCharacter;
    public GameObject TemplateEnemyCharacter;

    private CharacterData characterData = new CharacterData();

    // User Interface
    public GameObject MainUI;

    private QuestionList questionList;
    private Question activeQuestion;
    public GameObject QuestionOverlay;
    public GameObject QuestionText;
    public GameObject Ans1Text;
    public GameObject Ans2Text;
    public GameObject Ans3Text;
    public GameObject Ans4Text;

    #region Methods ----------------------------------------------------------------------------------

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
        GameObject o = Instantiate(TemplatePlayerCharacter) as GameObject;
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
        int nEnemiesToSpawn = NbEnemyPerLevel; // TODO
        CurrentLevelEnemyNumber = nEnemiesToSpawn;

        for (int i = 0; i < nEnemiesToSpawn; ++i)
        {
            // TODO: determine random position
            GameObject o = Instantiate(TemplateEnemyCharacter) as GameObject;
            o.GetComponent<SpriteRenderer>().sortingOrder = 50;

            o.GetComponent<Character>().destroyedEvent += HandleEnemyDestroyed;
        }
    }

    private void HandleEnemyDestroyed()
    {
        CurrentLevelEnemyNumber--;
        if (CurrentLevelEnemyNumber <= 0)
        {
            //CurrentGrid//UpdateCurrentLevelMap();
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
        ResumeGame();
    }

    private void HandleCorrectAnswer()
    {

    }

    private void HandleWrongAnswer()
    {

    }

    #endregion

    #region UnityBehavior ----------------------------------------------------------------------------------

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
