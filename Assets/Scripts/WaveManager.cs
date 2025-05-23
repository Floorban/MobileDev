using UnityEngine;
using static UnityEditor.PlayerSettings;

public class WaveManager : MonoBehaviour
{
    private Transform player;
    [SerializeField] Vector3 spawnArea = new Vector3(2, 3, 0);
    [SerializeField] Vector3 spawnBounds = new Vector3(7, 7, 0);
    [SerializeField] GameObject enemyPrefabs;
    private void Awake()
    {
        player = FindFirstObjectByType<Player>().transform;    
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SpawnEnemy();
        }
    }
    private void SpawnEnemy()
    {
        Vector3 spawnPos = GetSpawnPos(player.position);
        GameObject newEnemy = Instantiate(enemyPrefabs, spawnPos, Quaternion.identity);
        newEnemy.transform.SetParent(transform);
    }

    private Vector3 GetSpawnPos(Vector3 playerPos)
    {
        Vector3 pos = new Vector3();
        int area = Random.Range(0, 4);

        switch (area)
        {
            case 0: // Top
                pos.y = Random.Range(spawnArea.y + playerPos.y, spawnBounds.y);
                pos.x = Random.Range(-spawnBounds.x, spawnBounds.x);
                break;
            case 1: // Bottom
                pos.y = Random.Range(-spawnBounds.y, -spawnArea.y + playerPos.y);
                pos.x = Random.Range(-spawnBounds.x, spawnBounds.x);
                break;
            case 2: // Left
                pos.x = Random.Range(-spawnBounds.x, -spawnArea.x + playerPos.x);
                pos.y = Random.Range(-spawnBounds.y, spawnBounds.y);
                break;
            case 3: // Right
                pos.x = Random.Range(spawnArea.x + playerPos.x, spawnBounds.x);
                pos.y = Random.Range(-spawnBounds.y, spawnBounds.y);
                break;
        }

        return pos;
    }
    private void OnDrawGizmosSelected()
    {
        Vector3 center = Application.isPlaying && player != null ? player.position : transform.position;

        Gizmos.color = Color.green;
        Vector3 areaSize = new Vector3(spawnArea.x * 2, spawnArea.y * 2, 0f);
        Gizmos.DrawWireCube(center, areaSize);

        Gizmos.color = Color.red;
        Vector3 boundsSize = new Vector3(spawnBounds.x * 2, spawnBounds.y * 2, 0f);
        Gizmos.DrawWireCube(transform.position, boundsSize);
    }
}
