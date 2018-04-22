using System;
using System.Collections;
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

        // Handle in priority Fire commands ...
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            character.ThrowTowards(Character.ThrowDirection.Up);
            return;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            character.ThrowTowards(Character.ThrowDirection.Down);
            return;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            character.ThrowTowards(Character.ThrowDirection.Left);
            return;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            character.ThrowTowards(Character.ThrowDirection.Right);
            return;
        }

        // ... Otherwise just read movement
        if (Input.GetAxisRaw("Horizontal") != 0 ||
            Input.GetAxisRaw("Vertical") != 0)
        {
            Move();
        }
    }

    private void Move()
    {
        character.Animate();

        float moveH = Input.GetAxis("Horizontal");
        float moveV = Input.GetAxis("Vertical");

        character.MoveBy(moveH * character.MovementSpeed * Time.deltaTime,
                         moveV * character.MovementSpeed * Time.deltaTime);
    }
}
