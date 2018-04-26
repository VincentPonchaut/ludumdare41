using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

    public int factor = 2;
    public Character character;

    private void HandleBossDeath(Character c)
    {
        LevelManager.Instance.GameEnd();
    }

	// Use this for initialization
	void Start ()
    {
        character = GetComponent<Character>();
        character.deathEvent += HandleBossDeath;

        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * 3,
        gameObject.transform.localScale.y * 2,
        gameObject.transform.localScale.z);

        character.Life = character.Life * 25;
        character.Strength = character.Strength * factor;
        character.MovementSpeed = character.MovementSpeed * 0.5f;

        EnemyController ennemy = gameObject.GetComponent<EnemyController>();
        ennemy.FiringRange = 8;
        ennemy.FireTimeMilliseconds = 800;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
