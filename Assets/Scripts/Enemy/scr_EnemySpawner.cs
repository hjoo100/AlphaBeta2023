using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyPrefabs;
    public int spawnAmount = 10;
    public float spawnInterval;
    public scr_camerafollow cameraFollowScript;
    public List<GameObject> exitObjects;
    public Vector2 cameraLockedMinPos, cameraLockedMaxPos;

    private int enemySpawned = 0;
    private bool stopSpawning = false; 

    private void Start()
    {
        cameraFollowScript = FindObjectOfType<scr_camerafollow>();
    }
    public void StartSpawning()
    {
        Debug.Log("start spawing!");
        StartCoroutine(SpawnEnemies());
        LockCameraAndExits();
    }

    private IEnumerator SpawnEnemies()
    {
        while (enemySpawned < spawnAmount) 
        {
            // Randomly choose a prefab from the list
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

            GameObject spawnedMonster = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            scr_SpawnedEnemy spawnedEnemyScript = spawnedMonster.AddComponent<scr_SpawnedEnemy>();
            spawnedEnemyScript.spawner = this;

            enemySpawned++;

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void EnemyDestroyed()
    {
        enemySpawned--;

        if (enemySpawned <= 0)
        {
            UnlockCameraAndExits();
        }
    }

    private void LockCameraAndExits()
    {
        cameraFollowScript.minPos = cameraLockedMinPos;
        cameraFollowScript.maxPos = cameraLockedMaxPos;

        foreach (GameObject exit in exitObjects)
        {
            exit.SetActive(true);
            
        }
    }

    private void UnlockCameraAndExits()
    {
        cameraFollowScript.ResetCameraBounds();

        foreach (GameObject exit in exitObjects)
        {
            exit.SetActive(false);
        }
    }
}
