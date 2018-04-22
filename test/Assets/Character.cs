﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static Grid CurrentLevel;

    public enum ThrowDirection
    {
        Up, // future updates
        Down,
        Left,
        Right
    };

    public delegate void DestroyDelegate();
    public DestroyDelegate destroyedEvent;

    public delegate void DamageDelegate(Character damagedCharacter, int damageAmount);
    public DamageDelegate damagedEvent;

    #region Attributes
    // Stats
    public int Life = 100;
    public int Strength = 10;
    public int MovementSpeed = 1;
    public float ThrowSpeed = 10.0f;

    // Item Management
    public ThrowDirection Direction;
    public GameObject Item;

    // Position
    public Vector3Int CellPosition;
    #endregion

    #region Character Methods

    public void Throw()
    {
        /**/
        GameObject item = Instantiate(this.Item,
                                      this.transform.position,
                                      this.transform.rotation);

        ThrowableItem tItem = item.GetComponent<ThrowableItem>();
        if (tItem == null)
        {
            Debug.Log("Error while creating throwable item");
            DestroyImmediate(item);
            return;
        }
        tItem.Thrower = this;

        // Avoid colliding thrower and throwee
        Physics2D.IgnoreCollision(item.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());

        Vector3Int cellPos = CurrentLevel.LocalToCell(this.transform.position);

        int dX = Direction == ThrowDirection.Left  ? -1:
                 Direction == ThrowDirection.Right ? +1:
                                                      0;
        int dY = Direction == ThrowDirection.Down ? -1:
                 Direction == ThrowDirection.Up   ? +1:
                                                     0;
        int scaleFactor = 5 ;
        dX *= scaleFactor;
        dY *= scaleFactor;

        Vector2 delta = new Vector2(dX, dY);

        item.GetComponent<Rigidbody2D>().velocity = delta * this.ThrowSpeed;
        /*/
        GameObject item = Instantiate(this.Item, this.transform.localPosition, Quaternion.identity);
        item.GetComponent<ThrowableItem>().Thrower = this;

        Debug.Log(this.GetComponent<Rigidbody2D>());
        item.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x,
                                                                this.GetComponent<Rigidbody2D>().velocity.y);
        /**/
    }

    public void ApplyDamageFrom(Character enemy, ThrowableItem item)
    {
        Debug.Log("Applying damage from " + enemy + " to " + this);

        int dmg = enemy.Strength;
        // TODO: factor in item later

        this.Life -= dmg;
        Debug.Log(this + " [Life: " + Life + "]");
        if (damagedEvent != null)
            damagedEvent(this, dmg); // call all slots

        if (this.Life <= 0)
            Destroy(this.gameObject);
    }

    public Sprite[] Sprites;
    public float AnimationBufferSeconds = 0.3f;
    private int spriteIndex = 0;
    private float lastAnimationTime;

    public void Animate()
    {
        if (Time.time - this.lastAnimationTime > this.AnimationBufferSeconds)
        {
            // Toggle sprite
            GetComponent<SpriteRenderer>().sprite = Sprites[spriteIndex];
            spriteIndex++;
            if (spriteIndex >= Sprites.Length)
                spriteIndex = 0;
            this.lastAnimationTime = Time.time;
        }
    }
    
    public void MoveBy(float moveH, float moveV)
    {
        // TODO: update direction
        // Translate
        // Rotate
        transform.Translate(moveH, moveV, 0);
        transform.rotation = Quaternion.Euler(0, 0, 0);

        // Cancel forces
        this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

        // Flip if necessary
        GetComponent<SpriteRenderer>().flipX = (moveH < 0);

        // Determine actual direction
        if (Math.Abs(moveV) > Math.Abs(moveH))
        {
            // Can be either Up or Down
            Direction = moveV > 0 ? Character.ThrowDirection.Up :
                                    Character.ThrowDirection.Down;
        }
        else
        {
            // Can be either Left or Right
            Direction = moveH < 0 ? Character.ThrowDirection.Left :
                                    Character.ThrowDirection.Right;
        }
    }
    
    #endregion

    #region Unity Behavior

    // Use this for initialization
    void Start()
    {
        this.GetComponent<Animator>().enabled = false;
        this.lastAnimationTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentLevel != null)
            this.CellPosition = CurrentLevel.LocalToCell(transform.localPosition);
    }

    private void OnDestroy()
    {
        if (destroyedEvent != null)
            destroyedEvent();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        ThrowableItem thrownItem = collider.gameObject.GetComponent<ThrowableItem>();
        if (thrownItem == null ||
            thrownItem.Thrower == this)
            return;

        ApplyDamageFrom(thrownItem.Thrower, thrownItem);
        Destroy(thrownItem.gameObject);
        /*
        if (this.Thrower != null && collider.gameObject.GetComponent<Character>())
        {
            Character hitCharacter = collider.gameObject.GetComponent<Character>();
            if (hitCharacter == this.Thrower)
                return;

            if (collider.GetType() == typeof(CircleCollider2D))
                hitCharacter.ApplyDamageFrom(this.Thrower, this);
        }

        Destroy(this.gameObject);
        */
    }

    #endregion
}
