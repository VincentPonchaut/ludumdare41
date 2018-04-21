using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableItem : MonoBehaviour
{
    public GameObject hitPrefab;
    public Character Thrower;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<ThrowableItem>())
        {
            //ContactPoint2D contact = collision.contacts[0];
            //Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
               //Vector3 pos = contact.point;

            GameObject hitObject = Instantiate(hitPrefab, transform.localPosition, transform.localRotation) as GameObject;
            Destroy(collision.gameObject);
            Destroy(hitObject, 0.1f);
        }
        else if (this.Thrower != null && collision.gameObject.GetComponent<Character>())
        {
            Character hitCharacter = collision.gameObject.GetComponent<Character>();
            if (hitCharacter != this.Thrower)
            {
                hitCharacter.ApplyDamageFrom(this.Thrower, this);
            }
        }

        Destroy(this.gameObject);
    }
}
