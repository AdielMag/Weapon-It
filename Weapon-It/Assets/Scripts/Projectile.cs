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

    Rigidbody rgb;

    private void Awake()
    {
        rgb = GetComponent<Rigidbody>();
    }

    public void OnObjectSpawn()
    {
        spawnTime = Time.time;

        rgb.velocity = Vector3.zero;
        rgb.angularVelocity = Vector3.zero;
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
            other.GetComponent<Enemy>().TakeDamage(projectileDamage);

            Explode();
        }
    }

    public void Explode()
    {
        gameObject.SetActive(false);
    }
}
