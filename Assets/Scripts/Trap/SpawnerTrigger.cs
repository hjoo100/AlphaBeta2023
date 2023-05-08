using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTrigger : MonoBehaviour
{
    bool spawnerEnabled = false;
    public scr_EnemySpawner Spawner;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Object " + collision.gameObject.name + " with tag " + collision.gameObject.tag + " touched the trigger.");
        if (collision.gameObject.CompareTag("Player") == false)
        {
            Debug.Log("non-player touched the trigger.");
            return;
        }
        if(!spawnerEnabled)
        {
            Debug.Log("player touched the trigger.");
            spawnerEnabled = true;
            Spawner.StartSpawning();
        }
    }
}
