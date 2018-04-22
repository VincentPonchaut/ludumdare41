using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class EnemyController : MonoBehaviour
{
    Character character;

    Character target;

    public int ReactionTimeMilliseconds = 60;
    public int FireTimeMilliseconds = 800;
    public int FiringRange = 3;

    private float lastActionTime;
    private float lastFireTime;

    // Use this for initialization
    void Start()
    {
        character = GetComponent<Character>();
        AcquireTarget();
        lastActionTime = Time.time;
    }

    void AcquireTarget()
    {
        target = FindObjectOfType<PlayerController>().GetComponent<Character>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if ((Time.time - lastActionTime) < ((float) ReactionTimeMilliseconds / 1000.0f))
            return;

        bool firingAuthorized = Time.time - lastFireTime > ((float)FireTimeMilliseconds / 1000.0f);

#if UNITY_EDITOR
        // Debug only
        if (Input.GetKeyDown("u"))
            character.Throw();
#endif
        if (target == null)
            AcquireTarget();

        if (target != null)
        {
            // Go towards target
            Vector3 targetPos = target.transform.localPosition;
            Vector3 targetDir = targetPos - this.transform.localPosition;

            float targetDistance = targetDir.magnitude;
            if (targetDistance <= FiringRange && firingAuthorized)
            {
                character.Throw();
                lastFireTime = Time.time;
            }
            else
            {
                Vector3 movement = targetDir.normalized * character.MovementSpeed * Time.deltaTime;

                float moveH = movement.x;
                float moveV = movement.y;

                character.Animate();
                character.MoveBy(moveH, moveV);
            }
        }
        else
        {
            // Random movement
        }

        // Update last action
        lastActionTime = Time.time;
    }
}
