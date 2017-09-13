    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class proximity2DEffect : MonoBehaviour {

    AudioMixerGroup mixGroup;
    BoxCollider boxCol;

    float maxDistanceX;
    float maxDistanceY;
    [Range(0, 1)] public float yInfluence;

    // Use this for initialization
    void Start () {
        boxCol = GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnColliderEnter(Collider other)
    {

    }
}
