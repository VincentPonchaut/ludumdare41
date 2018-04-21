using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public enum ThrowDirection {
        //Up, // future updates
        //Down,
        Left,
        Right
    };

    // Stats
    public int Life = 100;
    public int MovementSpeed = 1;
    public float ThrowSpeed = 10.0f;

    // Item Management
    public ThrowDirection Direction;
    //public string ItemType;
    public GameObject Item;

    // Position
    public Grid CurrentLevel;
    public Vector3Int CellPosition;

    //public void EquipThrowableItem(string itemType)
    //{
    //    this.Item.ResourcePath = itemType;
    //}

    public void Throw()
    {
        // Determine Spawn position
        int itemSpawnPointX = 0;

        switch (this.Direction)
        {
            case ThrowDirection.Left:
                itemSpawnPointX = this.CellPosition.x - 1;
                break;
            case ThrowDirection.Right:
                itemSpawnPointX = this.CellPosition.x + 1;
                break;
            default:
                break;
        }

        // Spawn a new ThrowableItem
        //ThrowableItem item = new ThrowableItem(this.ItemType);
        Vector3Int targetPos = this.CellPosition;
        targetPos.x = itemSpawnPointX;

        Vector3Int directionVec = targetPos - this.CellPosition;
        
        GameObject item = Instantiate(this.Item, this.CurrentLevel.CellToLocal(targetPos), Quaternion.identity);
        item.GetComponent<Rigidbody2D>().velocity = new Vector2(directionVec.x, directionVec.y) * this.ThrowSpeed;
    }

    // Use this for initialization
    void Start()
    {
        //if (this.ItemType.Length > 0)
        //    this.EquipThrowableItem(this.ItemType);
    }

    // Update is called once per frame
    void Update()
    {
        this.CellPosition = this.CurrentLevel.LocalToCell(transform.localPosition);

        if (Input.GetKeyDown("space"))
            this.Throw();
    }
}
