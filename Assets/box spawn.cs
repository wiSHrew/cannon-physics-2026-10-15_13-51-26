using UnityEngine;

public class boxspawn : MonoBehaviour
{
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private Transform spawnPoint;
    private GameObject spawnedBox;

    void Start()
    {
        SpawnBox();
    }

    public void SpawnBox()
    {
        if (boxPrefab == null)
        {
            Debug.LogError("Assign a box prefab in the Inspector.", this);
            return;
        }

        if (spawnPoint == null)
        {
            Debug.LogError("Assign a spawn point in the Inspector.", this);
            return;
        }

        if (spawnedBox != null)
            Destroy(spawnedBox);

        Vector3 randomOffset = new Vector3(
            Random.Range(-1f, 1f),
            0f,
            Random.Range(-1f, 1f)
        );
        spawnedBox = Instantiate(
            boxPrefab,
            spawnPoint.position + randomOffset,
            spawnPoint.rotation
        );
    }
}
