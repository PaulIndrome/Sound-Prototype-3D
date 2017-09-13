using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSoundScript : MonoBehaviour {

    AudioSource source;

    public AudioClip[] runClips;
    public AudioClip[] walkClips;

    // Use this for initialization
    void Start () {
        source = GetComponent<AudioSource>();
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void evaluateAnimationEvent(int s)
    {
        switch (s)
        {
            case 1:
                //Debug.Log("run");
                source.clip = runClips[Random.Range(0, runClips.Length)];
                source.volume = Random.Range(0.1f, 0.2f);
                source.Play();
                break;
            case 2:
                source.clip = runClips[Random.Range(0, runClips.Length)];
                source.volume = Random.Range(0.05f, 0.12f);
                source.Play();
                break;
            default:
                Debug.Log("Default");
                break;
        }
    }
}
