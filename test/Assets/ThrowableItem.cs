using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableItem : MonoBehaviour
{
    public GameObject hitPrefab;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Character>())
        {
            Debug.Log("Apply damage");
        }
        else if (collision.gameObject.GetComponent<ThrowableItem>())
        {
            //ContactPoint2D contact = collision.contacts[0];
            //Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            //Vector3 pos = contact.point;

            GameObject hitObject = Instantiate(hitPrefab, transform.localPosition, transform.localRotation);
            Destroy(collision.gameObject);
            Destroy(hitObject, 0.1f);
        }

        Destroy(this);
    }
}
