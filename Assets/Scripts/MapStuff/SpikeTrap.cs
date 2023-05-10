using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField]
    private float spikeSpeed = 3f;
    [SerializeField]
    private float damage = 10f;
    [SerializeField]
    private float spikeDelay = 1f;
    [SerializeField]
    private float spikeHeight = 2f;
    [SerializeField]
    private float resetDelay = 5f; // delay before trap resets

    private Vector3 initialPosition;
    private bool isActivated = false;
    private bool isEnabled = false;
    private bool playerInside = false; 

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (isEnabled)
        {
            if (isActivated)
            {
                transform.position = Vector3.MoveTowards(transform.position, initialPosition + Vector3.up * spikeHeight, spikeSpeed * Time.deltaTime);
            }
            else if (!playerInside) // only move down if player is not on the spikes
            {
                transform.position = Vector3.MoveTowards(transform.position, initialPosition, spikeSpeed * Time.deltaTime);
            }
        }
    }

    public IEnumerator ActivateTrap(Collider2D player) // Pass the player collider to the coroutine
    {
        isEnabled = true;
        player.GetComponent<Scr_PlayerCtrl>().takeDmg(damage); // Deal damage immediately when spikes are activated

        if (!isActivated)
        {
            isActivated = true;
            yield return new WaitForSeconds(spikeDelay);
            isActivated = false;

            // Start resetting trap
            yield return new WaitForSeconds(resetDelay);
            isEnabled = false; // Reset finished
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true; // Player entered the spikes
            StartCoroutine(ActivateTrap(other)); // Pass the player collider to the coroutine
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false; // Player left the spikes
        }
    }
}