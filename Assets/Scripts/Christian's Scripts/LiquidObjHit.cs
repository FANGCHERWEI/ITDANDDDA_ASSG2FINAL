using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidObjHit : MonoBehaviour
{
    public GameObject fluid;
    public float delay;

    private void Awake()
    {
        if (delay == 0)
        {
            delay = 1f;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "FluidPlane")
        {
            StartCoroutine(Delay());
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(delay);
        fluid.GetComponent<MeshRenderer>().enabled = true;
    }
}
