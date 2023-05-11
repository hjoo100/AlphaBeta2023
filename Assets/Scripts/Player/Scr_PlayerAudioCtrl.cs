using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PlayerAudioCtrl : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip meleeHit,meleeNothit,rangeSword,rapidHitAudio;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAudio(int num)
    {
        if(num == 0)
        {//Play melee not hit sound
            audioSource.clip = meleeNothit;
            audioSource.Play();
        }
        if(num == 1)
        {//Play melee hit sound
            audioSource.clip = meleeHit;
            audioSource.Play();
        }
        if(num == 2)
        {
            //Play ranged sword sound
            audioSource.clip = rangeSword;
            audioSource.Play();
        }

        if(num == 4)
        {
            //Play rapidHit audio sound
            audioSource.clip = rapidHitAudio;
            audioSource.Play();
        }

        
    }
}
