using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryGame : MonoBehaviour
{
    public void Retry()
    {
        SceneManager.LoadScene("Welcome", LoadSceneMode.Single);
    }
}
