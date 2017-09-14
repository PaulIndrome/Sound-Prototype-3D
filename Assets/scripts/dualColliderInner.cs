using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dualRangeColliders : MonoBehaviour {

    dualRangeParent parent;
    BoxCollider thisCol;

	// Use this for initialization
	void Start () {
        parent = GetComponentInParent<dualRangeParent>();
        thisCol = GetComponent<BoxCollider>();
        parent.SetInnerRange(thisCol);
	}
	
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            parent.InnerRangeEntered();
        }
    }

    void OnTriggerExited(Collider other)
    {
        if (other.tag == "Player")
        {
            parent.InnerRangeExited();
        }
    }
}
