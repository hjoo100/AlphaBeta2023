using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_JavelinBarrageObj : MonoBehaviour
{
    public float arrowAngleOffset = 15f;
    public GameObject bullet;

    AudioSource audioSrc;
    public AudioClip firesound;
    // Start is called before the first frame update
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        //FireJavelin();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FireJavelinByTimes(int time)
    {
        for(int i = 0; i<time; i++)
        {
            FireJavelin();
            
        }
    }
    public void FireJavelin()
    {
        float[] angles = { -2 * arrowAngleOffset, -arrowAngleOffset, 0f, arrowAngleOffset, 2 * arrowAngleOffset };
        float initialForce = 5f;

        foreach (float angle in angles)
        {
            GameObject arrowInstance = Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, 0));
            Quaternion arrowRotation = transform.rotation * Quaternion.Euler(0, 0, angle + 135f);
            arrowInstance.transform.rotation = arrowRotation * Quaternion.Euler(0, 0, 22.5f);

            Vector3 adjustedRight = Quaternion.Euler(0, 180, 0) * arrowInstance.transform.right;
            arrowInstance.transform.right = adjustedRight;

            Rigidbody2D arrowRb = arrowInstance.GetComponent<Rigidbody2D>();
            arrowRb.AddForce(arrowInstance.transform.up * initialForce, ForceMode2D.Impulse);
        }

        audioSrc.clip = firesound;
        audioSrc.Play();
    }
}
