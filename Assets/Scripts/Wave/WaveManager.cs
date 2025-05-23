using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class WaveManager : MonoBehaviour
{
    [Header("Spawn Area")]
    private Transform player;
    [SerializeField] Vector3 spawnArea = new Vector3(2, 3, 0);
    [SerializeField] Vector3 spawnBounds = new Vector3(7, 7, 0);

    [Header("Wave Data")]
    public WaveStats[] wavestats = new WaveStats[10];
    public int curWave = 1;
    private int curWaveValue;
    public List<GameObject> enemiesToSpawn = new List<GameObject>();
    public List<GameObject> spawnedEnemies = new List<GameObject>();

    [Header("Wave Timer")]
    [SerializeField] private float waveTimer;
    [SerializeField] private float spawnInterval;
    private float spawnTimer;

    [Header("UI")]
    private OffScreenIndicator offScreenIndicator;
    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private bool waveStart;
    public bool WaveStart
    {
        get => waveStart;
        set
        {
            waveStart = value;
            if (value)
                GenerateWave();
        }
    }
    private void Awake()
    {
        player = FindFirstObjectByType<Player>().transform;
        offScreenIndicator = GetComponent<OffScreenIndicator>();
    }

    private void FixedUpdate()
    {
        if (!waveStart) return;

        if (spawnTimer <= 0)
        {
            if (enemiesToSpawn.Count > 0)
                SpawnEnemy();
            else
                GenerateWave();
        }
        else
        {
            spawnTimer -= Time.fixedDeltaTime;
            waveTimer -= Time.fixedDeltaTime;
        }

        if (waveTimer <= 0)
        {
            waveStart = false;
            curWave++;
            waveTimer = 1;
            timerText.text = 0.ToString();
            // wave ends, go to prepare phase
        }
        else
        {
            timerText.text = ((int)waveTimer).ToString();
        }
    }
    private void SpawnEnemy()
    {
        Vector3 spawnPos = GetSpawnPos(player.position);
        GameObject enemy = Instantiate(enemiesToSpawn[0], spawnPos, Quaternion.identity);
        enemy.transform.SetParent(transform);
        enemiesToSpawn.RemoveAt(0);
        spawnedEnemies.Add(enemy);
        offScreenIndicator.InitIndicators(enemy);
        spawnTimer = spawnInterval;
    }
    public void GenerateWave()
    {
        if (!waveStart || curWave > wavestats.Length) return;

        curWaveValue = wavestats[curWave - 1].waveValue;
        GenerateEnemiesToSpawn();
        spawnInterval =  wavestats[curWave - 1].waveDuration / enemiesToSpawn.Count;
        waveTimer = wavestats[curWave - 1].waveDuration; 
    }

    public void GenerateEnemiesToSpawn()
    {
        List<GameObject> generatedEnemies = new List<GameObject>();
        while (curWaveValue > 0 || generatedEnemies.Count < wavestats[curWave - 1].maxNumOfEnemies)
        {
            int randEnemyId = Random.Range(0, wavestats[curWave - 1].enemyPoll.Count);
            int randEnemyCost = wavestats[curWave - 1].enemyPoll[randEnemyId].cost;

            if (curWaveValue - randEnemyCost >= 0)
            {
                generatedEnemies.Add(wavestats[curWave - 1].enemyPoll[randEnemyId].enemyPrefab);
                curWaveValue -= randEnemyCost;
            }
            else if (curWaveValue <= 0)
            {
                break;
            }
        }
        enemiesToSpawn.Clear();
        enemiesToSpawn = generatedEnemies;
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

[System.Serializable]
public class Enemy
{
    public GameObject enemyPrefab;
    public int cost;
}
