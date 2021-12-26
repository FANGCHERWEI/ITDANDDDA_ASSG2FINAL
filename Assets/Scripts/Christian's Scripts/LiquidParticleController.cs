using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidParticleController : MonoBehaviour
{
    public Transform spawnLocTransform;
    public GameObject invisCollider;

    public RaycastHit raycastHit;
    public LayerMask containerOnly;

    [SerializeField]
    private Vector3 spawnLocActual;

    public float delay;

    public bool allowSpawning;
    public bool spawnSystemRunning;

    private void Awake()
    {
        allowSpawning = true;

        if (delay == 0)
        {
            delay = 0.1f;
        }
    }

    private void FixedUpdate()
    {
        if (gameObject.layer == 8)
        {
            SpawnSystem();
        }
    }

    private void SpawnSystem()
    {
        if (Physics.Raycast(origin: spawnLocTransform.position, direction: Vector3.down, hitInfo: out raycastHit, maxDistance: 100, layerMask: containerOnly))
        {
            if (raycastHit.collider.gameObject.layer == 9)
            {
                if (allowSpawning)
                {
                    StartCoroutine(Delay());

                    allowSpawning = false;
                }
            }

            else
            {
                allowSpawning = true;
            }
        }

        else
        {
            allowSpawning = true;
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(delay);

        spawnLocActual = spawnLocTransform.position;

        Instantiate(invisCollider, spawnLocActual, Quaternion.identity);
    }
}
