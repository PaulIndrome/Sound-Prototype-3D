using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class stingerTrigger : MonoBehaviour {

    public AudioSource stingerSource;
    public AudioClip stingerFromLeft;
    public AudioClip stingerFromRight;
    CharacterController playerCC;

    [Range(0,1)]
    public float volumeLeft;
    [Range(0, 1)]
    public float volumeRight;

    void Start()
    {
        stingerSource = FindObjectOfType<Camera>().GetComponentInChildren<AudioSource>();
    }

	void OnTriggerEnter(Collider other)
    {
        if (stingerFromLeft == null && stingerFromRight == null)
            this.enabled = false;
        if(other.tag == "Player")
        {
            playerCC = other.gameObject.GetComponent<CharacterController>();

            if(playerCC.velocity.x > 0 && stingerFromLeft != null)
            {
                stingerSource.PlayOneShot(stingerFromLeft, volumeLeft);
            } else if (playerCC.velocity.x < 0 && stingerFromRight != null)
            {
                stingerSource.PlayOneShot(stingerFromRight, volumeRight);
            } 
        }
        
    }
}
