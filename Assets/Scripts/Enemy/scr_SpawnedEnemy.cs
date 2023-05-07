using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_SpawnedEnemy : MonoBehaviour
{
    public scr_EnemySpawner spawner;

    private void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.EnemyDestroyed();
        }
    }
}
