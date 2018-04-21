using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class EnemyController : MonoBehaviour
{
    Character character;

    // Use this for initialization
    void Start()
    {
        character = GetComponent<Character>();
    }
	
	// Update is called once per frame
	void Update ()
    {
#if UNITY_EDITOR
        // Debug only
        if (Input.GetKeyDown("u"))
            character.Throw();
#endif
    }
}
