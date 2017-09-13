using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class proximity2DMusic : MonoBehaviour {

    float maxDistanceX;
    float maxDistanceY;
    float currentDistance;
    float currentVolume;
    [Range(0, 1)] public float maxVolume;
    [Range(0, 1)] public float yInfluence;
    [Range(-1, 1)] public float LMRBias;

    public AudioSource audioS;
    public AnimationCurve proximCurve;
    private GameObject player;
    private BoxCollider boxCol;


	void Start () {
        audioS = GetComponent<AudioSource>();
        audioS.volume = 0;
        boxCol = GetComponent<BoxCollider>();
        maxDistanceX = boxCol.size.x / 2;
        maxDistanceY = boxCol.size.y / 2;
	}
	
	void Update () {
        if (player && audioS.isPlaying)
        {
            currentDistance = Mathf.Sqrt(Mathf.Pow((transform.position.x - player.transform.position.x),2) + (Mathf.Pow((transform.position.y - player.transform.position.y), 2)) * yInfluence);
            currentVolume = maxVolume * proximCurve.Evaluate(currentDistance / Mathf.Sqrt(Mathf.Pow((maxDistanceX), 2) + (Mathf.Pow((maxDistanceY), 2)) * yInfluence));
            audioS.volume = currentVolume;
            
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            player = other.gameObject;
            audioS.Play();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (player)
        {
            player = null;
            audioS.Pause();
            audioS.volume = 0;
        }
    }


}
