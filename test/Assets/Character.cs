using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    enum ThrowDirection {
        //Up, // future updates
        //Down,
        Left,
        Right
    };

    // Item Management
    public ThrowDirection Direction;
    public ThrowableItem Item;

    // Position
    public Vector3Int CellPosition;

    public void EquipThrowableItem(string itemType)
    {
        this.Item = new ThrowableItem(itemType);
    }

    public void Throw()
    {

    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
