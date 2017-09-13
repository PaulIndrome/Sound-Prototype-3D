using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dualRangeParent : MonoBehaviour {

    bool withinInnerRange;
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

    private BoxCollider outerRange;

    private BoxCollider innerRange;
    float innerRangeSizeX;
    float innerRangePosX;

	void Start () {
        audioS = GetComponent<AudioSource>();
        audioS.volume = 0;
        outerRange = GetComponent<BoxCollider>();
	}
	
	void Update () {
        if (player && audioS.isPlaying)
        {
            if (withinInnerRange)
            {
                audioS.volume = maxVolume;
            } else
            {
                if(player.transform.position.x > innerRangePosX) //if player right of inner, interpolate right inner end to right outer end
                {
                    currentDistance = Mathf.Sqrt(Mathf.Pow((transform.position.x - player.transform.position.x), 2) + (Mathf.Pow((transform.position.y - player.transform.position.y), 2)) * yInfluence);
                    currentVolume = maxVolume * proximCurve.Evaluate(currentDistance / Mathf.Sqrt(Mathf.Pow((maxDistanceX), 2) + (Mathf.Pow((maxDistanceY), 2)) * yInfluence));
                    audioS.volume = currentVolume;
                }
                
            }
            
            
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
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


    public void innerRangeEntered()
    {
        withinInnerRange = true;
    }

    public void innerRangeExited()
    {
        withinInnerRange = false;
    }

    public void setInnerRange(BoxCollider innerRangeCol)
    {
        innerRange = innerRangeCol;
        innerRangeSizeX = innerRange.size.x;
        innerRangePosX = innerRange.transform.position.x;
    }


}
