using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingProjectile_RL : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float rotationSpeed;
    [SerializeField] float projectileSpeed;
    [SerializeField] Rigidbody rb;

    void Start()
    {
        target = gameManager.instance.player.transform;
    }

    void Update()
    {
        Vector3 direction = target.position - rb.position;
        Vector3 rotation = Vector3.Cross(transform.forward, direction);
        rb.angularVelocity = rotation * rotationSpeed;
        rb.velocity = transform.forward * projectileSpeed;
    }
}
