using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script controls the spawning and characteristics of liquid particles.

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
                // This if statement makes it so that fluid objects will only spawn when there is a container right below it.

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
        // This function waits for a while before spawning a fluid obj to ensure it does not collide with the exterior side or edge of containers and just bounce off.

        yield return new WaitForSeconds(delay);

        spawnLocActual = spawnLocTransform.position;

        Instantiate(invisCollider, spawnLocActual, Quaternion.identity);
    }
}
