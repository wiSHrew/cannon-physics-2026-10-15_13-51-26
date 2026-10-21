
using UnityEngine;

public class ballSpawner : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform spawnPoint;

    [Header("Velocity Formula")]
    [SerializeField] private float acceleration = 1f;
    [SerializeField] private float time = 1f;
    [SerializeField] private float targetVelocity = 5f;
    [SerializeField] private float i = 1f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (projectilePrefab == null || spawnPoint == null)
            {
                Debug.LogError("Projectile Prefab or Spawn Point is missing!");
                return;
            }

            GameObject projectile = Instantiate(
                projectilePrefab,
                spawnPoint.position,
                spawnPoint.rotation
            );

            Rigidbody rb = projectile.GetComponentInChildren<Rigidbody>();

            if (rb == null)
            {
                Debug.LogError("No Rigidbody found on projectile!");
                Destroy(projectile);
                return;
            }

            float velocity = 0.5f * acceleration * time
                             * targetVelocity * i;

            rb.isKinematic = false;
            rb.useGravity = true;
            rb.linearVelocity = spawnPoint.forward * velocity;

            BulletRotation bulletRotation =
                projectile.GetComponent<BulletRotation>();

            if (bulletRotation == null)
            {
                bulletRotation = projectile.AddComponent<BulletRotation>();
            }

            Debug.Log("Projectile launched. Velocity: " + rb.linearVelocity);
        }
    }
}
