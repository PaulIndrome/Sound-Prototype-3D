using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dualRangeScript : MonoBehaviour
{

    bool withinInnerRange;

    public float[] outerBounds; // 0 = top, 1 = right, 2 = bottom, 3 = left
    public float[] innerBounds; // 0 = top, 1 = right, 2 = bottom, 3 = left

    public float outerBoundLeft;
    public float outerBoundRight;
    public float outerBoundTop;
    public float outerBoundBottom;
    public float innerBoundLeft;
    public float innerBoundRight;
    public float innerBoundTop;
    public float innerBoundBottom;

    float currentDistance;
    float currentVolume;
    [Range(0, 1)]
    public float maxVolume;
    [Range(0, 1)]
    public float yInfluence;
    [Range(-1, 1)]
    public float LMRBias;

    public AudioSource audioS;
    public AnimationCurve proximCurve;
    private GameObject player;

    void Start()
    {
        audioS = GetComponent<AudioSource>();
        audioS.volume = 0;



    }

    void Update()
    {
        if (player && audioS.isPlaying)
        {
            if (withinInnerRange)
            {
                audioS.volume = maxVolume;
            }
            else
            {
               /* if (player.transform.position.x > innerRangePosX) //if player right of inner, interpolate right inner end to right outer end
                {
                    currentDistance = Mathf.Sqrt(Mathf.Pow((transform.position.x - player.transform.position.x), 2) + (Mathf.Pow((transform.position.y - player.transform.position.y), 2)) * yInfluence);
                    currentVolume = maxVolume * proximCurve.Evaluate(currentDistance / Mathf.Sqrt(Mathf.Pow((maxDistanceX), 2) + (Mathf.Pow((maxDistanceY), 2)) * yInfluence));
                    audioS.volume = currentVolume;
                }*/

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
        //innerRange = innerRangeCol;
        //innerRangeSizeX = innerRange.size.x;
        //innerRangePosX = innerRange.transform.position.x;
    }


}
