using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreColliders : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Stage02SideCollider")
        {
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }
}
