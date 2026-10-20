
using UnityEngine;

public class BulletRotation : MonoBehaviour
{
    private Rigidbody rb;
    private bool hasHit = false;

    [SerializeField] private Vector3 rotationOffset = new Vector3(0f, 90f, 0f);
    [SerializeField] private float minSpeed = 1f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (rb == null || hasHit || rb.isKinematic)
            return;

        Vector3 velocity = rb.linearVelocity;

        if (velocity.magnitude < minSpeed)
            return;

        Quaternion flightRotation =
            Quaternion.LookRotation(velocity.normalized, Vector3.up)
            * Quaternion.Euler(rotationOffset);

        rb.MoveRotation(flightRotation);
    }

    void OnCollisionEnter(Collision collision)
    {
        hasHit = true;
    }
}
