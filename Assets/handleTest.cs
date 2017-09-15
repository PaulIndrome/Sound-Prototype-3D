using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handleTest : MonoBehaviour
{
    public float distance;

    public Rect outerRect;
    public Rect innerRect;

    GameObject player;

    Vector2 relevantPos;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        distance = Distance(player.transform.position, new Vector2(outerRect.xMin, outerRect.yMin), new Vector2(outerRect.xMax, outerRect.yMax));
    }

    /*void OnSceneGUI()
    {
        if(outerRect == null)
        {
            outerRect = Rect.MinMaxRect(-1, -1, 1, 1);
        }
        if (innerRect == null)
        {   
            innerRect = Rect.MinMaxRect(-1, -1, 1, 1);
        }
    }*/

    private float Distance(Vector2 playerPos, Vector2 outerMin, Vector2 outerMax)
    {
        relevantPos = Vector2.Max(outerMin - playerPos, playerPos - outerMax);
        return (Vector2.Max(Vector2.zero, relevantPos)).magnitude + Mathf.Min(0.0f, Mathf.Max(relevantPos.x, relevantPos.y));
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(relevantPos, player.transform.position);
    }



}
