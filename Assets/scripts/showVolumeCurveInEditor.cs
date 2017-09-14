using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(proximity2DMusic))]
public class showVolumeCurveInEditor : Editor {

    void OnSceneGUI()
    {
        proximity2DMusic musicBox = (proximity2DMusic)target;

        float colPosX = musicBox.boxCol.transform.position.x;
        float colSizeX = musicBox.boxCol.size.x;
        float colPosY = musicBox.boxCol.transform.position.y;
        float colSizeY = musicBox.boxCol.size.y;

        List<Vector3> verts = new List<Vector3>();

        foreach (Keyframe kf in musicBox.proximCurve.keys)
        {
            verts.Add(new Vector3(
                colPosX + (colSizeX / 2) * kf.time,
                colPosY - (colSizeY / 2) + colSizeY * kf.value,
                0
                           ));
            verts.Add(new Vector3(
                colPosX - (colSizeX / 2) * kf.time,
                colPosY - (colSizeY / 2) + colSizeY * kf.value,
                0
                           ));
        }

        Handles.DrawPolyLine(verts.ToArray());

    }


}
