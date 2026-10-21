using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class enemybehaviour : MonoBehaviour
{
    [SerializeField] private float targetZ = -100f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float stoppingDistance = 1f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float minJumpInterval = 1f;
    [SerializeField] private float maxJumpInterval = 3f;
    [SerializeField] private float knockdownTorque = 3f;
    [SerializeField] private float destroyDelay = 2f;
    [SerializeField] private float scaleChangeInterval = 3f;
    [SerializeField] private float minScaleMultiplier = 0.5f;
    [SerializeField] private float maxScaleMultiplier = 2f;

    private Rigidbody rb;
    private CapsuleCollider bodyCollider;
    private Vector3 targetPosition;
    private float nextJumpTime;
    private bool hitByBullet;
    private Coroutine scaleRoutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bodyCollider = GetComponent<CapsuleCollider>();
        targetPosition = new Vector3(rb.position.x, rb.position.y, targetZ);
        nextJumpTime = Time.time + Random.Range(minJumpInterval, maxJumpInterval);
    }

    private void Start()
    {
        scaleRoutine = StartCoroutine(ChangeScaleRandomly());
    }

    private void FixedUpdate()
    {
        if (hitByBullet)
        {
            return;
        }

        Vector3 toTarget = targetPosition - rb.position;
        toTarget.y = 0f;

        Vector3 horizontalVelocity = Vector3.zero;
        if (toTarget.sqrMagnitude > stoppingDistance * stoppingDistance)
        {
            Vector3 direction = toTarget.normalized;
            horizontalVelocity = direction * moveSpeed;
            rb.MoveRotation(Quaternion.LookRotation(direction));
        }

        Vector3 velocity = rb.linearVelocity;
        velocity.x = horizontalVelocity.x;
        velocity.z = horizontalVelocity.z;
        rb.linearVelocity = velocity;

        if (IsGrounded() && Time.time >= nextJumpTime)
        {
            velocity = rb.linearVelocity;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
            rb.linearVelocity = velocity;
            nextJumpTime = Time.time + Random.Range(minJumpInterval, maxJumpInterval);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hitByBullet || !HasBulletTagInHierarchy(collision.collider.transform))
        {
            return;
        }

        hitByBullet = true;
        if (scaleRoutine != null)
        {
            StopCoroutine(scaleRoutine);
        }

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddPoint();
        }
        else
        {
            Debug.LogError("No ScoreManager found in the scene.", this);
        }
        rb.constraints = RigidbodyConstraints.None;

        ContactPoint contact = collision.GetContact(0);
        Vector3 hitDirection = contact.point - rb.worldCenterOfMass;
        hitDirection.y = 0f;

        if (hitDirection.sqrMagnitude < 0.001f)
        {
            hitDirection = -collision.relativeVelocity;
            hitDirection.y = 0f;
        }

        if (hitDirection.sqrMagnitude > 0.001f)
        {
            Vector3 fallTorque = Vector3.Cross(hitDirection.normalized, Vector3.up);
            rb.AddTorque(fallTorque * knockdownTorque, ForceMode.Impulse);
        }

        Invoke(nameof(DisableCollider), 1f);
        Destroy(gameObject, destroyDelay);
    }

    private IEnumerator ChangeScaleRandomly()
    {
        Vector3 baseScale = transform.localScale;
        float interval = Mathf.Max(0.01f, scaleChangeInterval);

        while (!hitByBullet)
        {
            Vector3 startScale = transform.localScale;
            Vector3 targetScale = baseScale * Random.Range(
                minScaleMultiplier,
                maxScaleMultiplier
            );

            for (float elapsed = 0f; elapsed < interval; elapsed += Time.deltaTime)
            {
                if (hitByBullet)
                {
                    yield break;
                }

                transform.localScale = Vector3.Lerp(startScale, targetScale, elapsed / interval);
                yield return null;
            }

            transform.localScale = targetScale;
        }
    }

    private void DisableCollider()
    {
        bodyCollider.enabled = false;
    }

    private static bool HasBulletTagInHierarchy(Transform candidate)
    {
        while (candidate != null)
        {
            if (candidate.CompareTag("bullet"))
            {
                return true;
            }

            candidate = candidate.parent;
        }

        return false;
    }

    private bool IsGrounded()
    {
        float groundCheckDistance = bodyCollider.bounds.extents.y + 0.1f;
        return Physics.Raycast(
            rb.position,
            Vector3.down,
            groundCheckDistance,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Ignore
        );
    }
}
