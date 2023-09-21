using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float spawnRate = 1f;
    public int mapLenght;
    public int maxDistanceFromPlayer = 1;

    public GameObject[] enemyPrefabs;

    public int maxAmountOfEnemys;
    public int numberOfEnemys;

    private float nextSpawn;

    GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        // Evitar que generin enemics fora del mapa
        mapLenght = mapLenght - 1;
        for (int i = 0; i < maxAmountOfEnemys; i++)
        {
            Spawner();
        }

    }
    private void Update()
    {
        if (numberOfEnemys < maxAmountOfEnemys)
            Spawner();
    }

    void Spawner()
    {
        // Seleccionar un enemic aleatori 
        int rand = Random.Range(0, enemyPrefabs.Length);
        GameObject enemyToSpawn = enemyPrefabs[rand];

        // Seleccionar una posició aleatoria
        float spawnPosX = transform.position.x + Random.Range(-mapLenght, mapLenght);
        float spawnPosY = transform.position.y + Random.Range(-mapLenght, mapLenght);
        while (spawnPosX < (player.transform.position.x + maxDistanceFromPlayer) && spawnPosX > (player.transform.position.x - maxDistanceFromPlayer))
            spawnPosX = transform.position.x + Random.Range(-mapLenght, mapLenght);
        while (spawnPosY < (player.transform.position.y + maxDistanceFromPlayer) && spawnPosY > (player.transform.position.y - maxDistanceFromPlayer))
            spawnPosY = transform.position.y + Random.Range(-mapLenght, mapLenght);
        
        Vector2 spawnPos = new Vector2(spawnPosX, spawnPosY);


        Spawn(enemyToSpawn, spawnPos);
        numberOfEnemys++;
    }



    //Enemy Spawn Function
    private void Spawn(GameObject enemy, Vector2 position)
    {
        Instantiate(enemy, position, Quaternion.identity);
    }
}
