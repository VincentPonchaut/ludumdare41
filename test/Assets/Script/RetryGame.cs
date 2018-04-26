using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryGame : MonoBehaviour
{
    public void Retry()
    {
        //if (LevelManager.Instance != null)
        //{
        //    DestroyImmediate(LevelManager.Instance.MainUI.gameObject);
        //    DestroyImmediate(LevelManager.Instance.gameObject);
        //}

        SceneManager.LoadScene("Welcome", LoadSceneMode.Single);
    }
}
