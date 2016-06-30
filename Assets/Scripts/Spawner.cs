using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    public Wave[] waves;

    Wave currentWave;
    public int currentWaveNumber;

    int enemiesRemainingToSpawn;
    int enemiesRemainingAlive;
    float nextSpawnTime;

    float animSpeed = 0;
    float movespeed = 0;
    float hittokillplayer = 0;
    float health = 0;
    
    
    MapGenerator map;

    void Start()
    {
        map = FindObjectOfType<MapGenerator>();
        GetComponent<HudIntegration>().NextWave();
    }

    void Update()
    {
        if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
        {
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

            StartCoroutine(SpawnEnemy());
        }
    }

    IEnumerator SpawnEnemy()
    {
        float spawnDelay = 1;
        float tileFlashSpeed = 4;

        Transform randomTile = map.GetRandomOpenTile();
        Material tileMat = randomTile.GetComponent<Renderer>().material;
        Color initialColor = tileMat.color;
        Color flashColor = Color.red;
        float spawnTimer = 0;

        while (spawnTimer < spawnDelay)
        {

            tileMat.color = Color.Lerp(initialColor, flashColor, Mathf.PingPong(spawnTimer * tileFlashSpeed, 1));
            spawnTimer += Time.deltaTime;
            yield return null;
        }

        System.Random rnd = new System.Random();
        int x = rnd.Next(0, currentWave.EnemysToSpawn.Length);
        tileMat.color = initialColor;

        if (currentWaveNumber == 0)
        {
            animSpeed = 1;
            movespeed = currentWave.moveSpeed[x];
            hittokillplayer = currentWave.hitsToKillPlayer[x];
            health = currentWave.enemyHealth[x];
        }

        else
        {
            Debug.Log(currentWaveNumber / 10f);
            animSpeed = 1 + (currentWaveNumber / 10f);
            movespeed = currentWave.moveSpeed[x] + (currentWaveNumber /10f);
            hittokillplayer = currentWave.hitsToKillPlayer[x] + (currentWaveNumber / 5f);
            health = currentWave.enemyHealth[x] + (currentWaveNumber * 1.24f);
        }

        Enemy spawnedEnemy = Instantiate(currentWave.EnemysToSpawn[x], randomTile.position + Vector3.up, Quaternion.identity) as Enemy;  
        spawnedEnemy.OnDeath += OnEnemyDeath;
        spawnedEnemy.SetCharacteristics(movespeed, hittokillplayer, health, animSpeed);
    }

    void OnEnemyDeath()
    {
        enemiesRemainingAlive--;
        if (enemiesRemainingAlive == 0)
        {
            GetComponent<HudIntegration>().NextWave();
        }
    }

    public void NextWave()
    {
        currentWaveNumber++;
        if (currentWaveNumber % 5 == 0)
        {
            int rnd = Random.Range(0, map.GetComponent<MapGenerator>().maps.Length);
            map.GetComponent<MapGenerator>().maps[rnd].seed = "";
            map.GetComponent<MapGenerator>().mapIndex = rnd;
            map.GetComponent<MapGenerator>().GenerateMap();
            GameObject.FindGameObjectWithTag("Player").transform.position = Vector3.zero + Vector3.up;
        }
        currentWave = waves[(currentWaveNumber - 1)%waves.Length];
        enemiesRemainingToSpawn = currentWave.enemyCount;
        enemiesRemainingAlive = enemiesRemainingToSpawn;
        GetComponent<HudIntegration>().countdown = 5;
    }

    [System.Serializable]
    public class Wave
    {
        public int enemyCount;
        public float timeBetweenSpawns;
        public Enemy[] EnemysToSpawn;

        public float[] moveSpeed;
        public int[] hitsToKillPlayer;
        public float[] enemyHealth;

    }

}
