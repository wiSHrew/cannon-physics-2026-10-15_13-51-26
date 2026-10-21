
using UnityEngine;

public class BulletRotation : MonoBehaviour
{
    private Rigidbody rb;
    private Collider bulletCollider;
    private bool hasHit = false;

    [SerializeField] private Vector3 rotationOffset = new Vector3(0f, 90f, 0f);
    [SerializeField] private float minSpeed = 1f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bulletCollider = GetComponent<Collider>();
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
        if (hasHit)
            return;

        hasHit = true;
        Invoke(nameof(DisableCollider), 1f);
        Destroy(gameObject, 2f);
    }

    private void DisableCollider()
    {
        if (bulletCollider != null)
        {
            bulletCollider.enabled = false;
        }
    }
}
