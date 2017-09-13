using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceTrigger : MonoBehaviour {

    AudioSource audioS;
    GameObject player;

    bool isPlaying = false;

	// Use this for initialization
	void Start () {
        audioS = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        float distanceToPlayer = (player.transform.position - transform.position).magnitude;
		if(!isPlaying && distanceToPlayer < audioS.maxDistance)
        {
            audioS.Play();
            isPlaying = true;
        } else if (distanceToPlayer > audioS.maxDistance)
        {
            isPlaying = false;
            audioS.Pause();
        }


	}



}
