using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairCollision : MonoBehaviour {

    public bool canUseStairs;

    public GameObject targetObject;
    private GameObject player;
    public Texture tex;

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void LateUpdate () {
		if(canUseStairs && Input.GetKeyDown(KeyCode.W))
        {
            player.transform.position = targetObject.transform.position;
            canUseStairs = false;
        }
        Debug.DrawLine(transform.position, targetObject.transform.position);
    }

    void OnTriggerEnter(Collider other)
    {

        if(other.tag == "Player") {
            canUseStairs = true;
            player = other.gameObject;
        }
    }

    void OnTriggerExit()
    {
        canUseStairs = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, targetObject.transform.position);
    }

    void OnGUI()
    {
        if (canUseStairs)
        {
            Vector3 screenPos = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().WorldToScreenPoint(transform.position);
            screenPos.Scale(new Vector3(1, 1.33f, 0));
            GUI.DrawTexture(new Rect(screenPos, new Vector2(20, 20)), tex, ScaleMode.ScaleToFit,true);
        } 
    }

}
