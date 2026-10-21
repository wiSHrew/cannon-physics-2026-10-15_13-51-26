using System.Collections;
using UnityEngine;

public class enemyspawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform spawnPoint;

    private void Start()
    {
        if (ScoreManager.Instance == null)
        {
            Debug.LogError("Enemy spawner needs a ScoreManager in the scene.", this);
            enabled = false;
            return;
        }

        if (enemyPrefab == null || spawnPoint == null)
        {
            SpawnEnemy();
            enabled = false;
            return;
        }

        SpawnEnemy();
        StartCoroutine(SpawnEnemiesRoutine());
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        while (true)
        {
            float interval = Mathf.Max(
                0.1f,
                5f * (100f - ScoreManager.Instance.CurrentScore) / 100f
            );
            yield return new WaitForSeconds(interval);
            SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Assign an enemy prefab in the Inspector.", this);
            return;
        }

        if (spawnPoint == null)
        {
            Debug.LogError("Assign a spawn point in the Inspector.", this);
            return;
        }

        Vector3 spawnPosition = spawnPoint.position;
        spawnPosition.x = Random.Range(-50f, 50f);

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, spawnPoint.rotation);

        float scaleMultiplier = Random.Range(0.5f, 2f);
        enemy.transform.localScale = enemyPrefab.transform.localScale * scaleMultiplier;
    }
}
