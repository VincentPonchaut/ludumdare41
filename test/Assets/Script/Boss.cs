﻿using System.Collections;
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

        Character c = gameObject.GetComponent<Character>();
        c.Life = c.Life * 20;
        c.Strength = c.Strength * factor;
        c.MovementSpeed = c.MovementSpeed * 0.5f;

        EnemyController ennemy = gameObject.GetComponent<EnemyController>();
        ennemy.FiringRange = 8;
        ennemy.FireTimeMilliseconds = 400;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}