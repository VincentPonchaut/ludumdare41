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

        if (Input.GetAxisRaw("Horizontal") != 0 ||
            Input.GetAxisRaw("Vertical") != 0)
        {
            Move();
            // TODO: UpdateAnimation() -> in move ?
        }
    }

    private void Move()
    {
        character.Animate();

        float lockPos = 0;

        float moveH = Input.GetAxis("Horizontal");
        float moveV = Input.GetAxis("Vertical");
       
        transform.Translate(moveH * character.MovementSpeed * Time.deltaTime, moveV * character.MovementSpeed * Time.deltaTime, 0);
        transform.rotation = Quaternion.Euler(lockPos, lockPos, lockPos);

        // Cancel forces
        this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

        // Flip if necessary
        character.GetComponent<SpriteRenderer>().flipX = (moveH < 0);

        // Determine actual direction
        if (Math.Abs(moveV) > Math.Abs(moveH))
        {
            // Can be either Up or Down
            character.Direction = moveV > 0 ? Character.ThrowDirection.Up :
                                              Character.ThrowDirection.Down;
        }
        else
        {
            // Can be either Left or Right
            character.Direction = moveH < 0 ? Character.ThrowDirection.Left :
                                              Character.ThrowDirection.Right;
        }
    }
}
