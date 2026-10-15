
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
        if (Input.GetKeyDown(KeyCode.Space) ||
            Input.GetMouseButtonDown(0))
        {
            if (projectilePrefab == null || spawnPoint == null)
                return;

            GameObject projectile = Instantiate(
                projectilePrefab,
                spawnPoint.position,
                spawnPoint.rotation
            );

            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            if (rb != null)
            {
                float velocity = 0.5f * acceleration * time
                                 * targetVelocity * i;

                rb.linearVelocity = spawnPoint.forward * velocity;
            }
        }
    }
}
