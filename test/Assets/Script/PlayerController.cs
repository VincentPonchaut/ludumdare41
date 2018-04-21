﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class PlayerController : MonoBehaviour
{
    Character character;

    // Use this for initialization
    void Start()
    {
        character = GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
            character.Throw();

        Move();
    }

    private void Move()
    {
        float lockPos = 0;

        float moveH = Input.GetAxis("Horizontal");
        float moveV = Input.GetAxis("Vertical");

        transform.Translate(moveH * character.MovementSpeed * Time.deltaTime, moveV * character.MovementSpeed * Time.deltaTime, 0);
        transform.rotation = Quaternion.Euler(lockPos, lockPos, lockPos);
    }
}