using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IPooledObject
{
    // After 'time' seconds the projectile will disable itself.
    public float timerSeconds = 5;

    [HideInInspector]
    public int projectileDamage = 1;

    float spawnTime;

    Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void OnObjectSpawn()
    {
        spawnTime = Time.time;

        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }

    private void Update()
    {
        // Check if spawn time + sceonds to add is less than acutal time.
        if (spawnTime + timerSeconds < Time.time)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Target")
        {
            other.GetComponent<Target>().TakeDamage(projectileDamage);

            Explode();
        }
    }

    public void Explode()
    {
        gameObject.SetActive(false);
    }
}
