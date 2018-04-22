using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ExitScript : MonoBehaviour {

    private bool IsOver = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsOver == false)
            return;

        LevelManager.Instance.AttemptExit();
    }

    public void InformEnd ()
    {
        //GetComponent<TilemapCollider2D>().enabled = false;
        IsOver = true;
    }

	// Use this for initialization
	void Start () {

    }

    // Update is called once per frame
    void Update () {
		
	}
}
