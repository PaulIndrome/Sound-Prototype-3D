using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(handleTest))]
public class handleTestEditor : Editor {



    void OnSceneGUI()
    {
        handleTest tf = (handleTest)target;

        if (tf.innerPos == null)
        {
            tf.innerPos = tf.transform.position;
        }
        
        //tf.innerTransform.SetParent(tf.transform);

        Rect rect = new Rect(new Vector2(tf.innerPos.x - tf.innerRangeLeft, tf.innerPos.y - tf.innerRangeDown), new Vector2(tf.innerRangeRight + tf.innerRangeLeft, tf.innerRangeUp + tf.innerRangeDown));
        
        //Rect rect = new Rect(new Vector2(tf.innerTransform.position.x - tf.innerRangeLeft, tf.innerTransform.position.y - tf.innerRangeDown), new Vector2(tf.innerRangeRight + tf.innerRangeLeft, tf.innerRangeUp + tf.innerRangeDown));
        
        /*Vector3[] verts = new Vector3[4]
        {
            new Vector3(tf.transform.position.x + tf.innerRangeRight, tf.transform.position.y + tf.innerRangeUp, 0),
            new Vector3(tf.transform.position.x - tf.innerRangeLeft, tf.transform.position.y + tf.innerRangeUp, 0),
            new Vector3(tf.transform.position.x - tf.innerRangeLeft, tf.transform.position.y - tf.innerRangeDown, 0),
            new Vector3(tf.transform.position.x + tf.innerRangeRight, tf.transform.position.y - tf.innerRangeDown, 0)
        };*/

        Vector3[] rectVerts = new Vector3[4]
       {
            new Vector3(rect.xMax, rect.yMax, 0),
            new Vector3(rect.xMin, rect.yMax, 0),
            new Vector3(rect.xMin, rect.yMin, 0),
            new Vector3(rect.xMax, rect.yMin, 0)
       };

        //Handles.DrawPolyLine(new Vector3[] { new Vector3(rect.xMax, rect.yMax, 0), new Vector3(rect.xMax, rect.yMin, 0), new Vector3(rect.xMin, rect.yMax, 0), new Vector3(rect.xMin, rect.yMin, 0) });

        Handles.DrawSolidRectangleWithOutline(rectVerts, new Color(1,0,0,0.05f), Color.red);

        tf.innerRangeUp = Handles.ScaleValueHandle(tf.innerRangeUp, new Vector3((rect.xMax + rect.xMin) / 2, rect.yMax, 0), Quaternion.identity, 4, Handles.RectangleHandleCap, 0);
        tf.innerRangeLeft = Handles.ScaleValueHandle(tf.innerRangeLeft, new Vector3(rect.xMin, (rect.yMax + rect.yMin) / 2, 0), Quaternion.identity, 4, Handles.RectangleHandleCap, 0);
        tf.innerRangeDown = Handles.ScaleValueHandle(tf.innerRangeDown, new Vector3((rect.xMax + rect.xMin) / 2, rect.yMin, 0), Quaternion.identity, 4, Handles.RectangleHandleCap, 0);
        tf.innerRangeRight = Handles.ScaleValueHandle(tf.innerRangeRight, new Vector3(rect.xMax, (rect.yMax + rect.yMin) / 2, 0), Quaternion.identity, 4, Handles.RectangleHandleCap, 0);


        EditorGUI.BeginChangeCheck();
        Vector3 vec = Handles.PositionHandle(new Vector2(tf.innerPos.x, tf.innerPos.y), Quaternion.identity);
        //tf.innerPos = Handles.PositionHandle(new Vector2(tf.innerTransform.position.x, tf.innerTransform.position.y), Quaternion.identity);
        if (EditorGUI.EndChangeCheck())
        {
            tf.innerPos = tf.transform.position;
            tf.innerPos += vec;
        }


        tf.rectPos = rect.position;
        tf.rectSize = rect.size;

        /*
        tf.innerRangeUp = Handles.ScaleValueHandle(tf.innerRangeUp, (verts[1] - verts[0])/2 + verts[0], Quaternion.identity, 4, Handles.RectangleHandleCap, 0);
        tf.innerRangeLeft = Handles.ScaleValueHandle(tf.innerRangeLeft, (verts[2] - verts[1]) / 2 + verts[1], Quaternion.identity, 4, Handles.RectangleHandleCap, 0);
        tf.innerRangeDown = Handles.ScaleValueHandle(tf.innerRangeDown, (verts[3] - verts[2]) / 2 + verts[2], Quaternion.identity, 4, Handles.RectangleHandleCap, 0);
        tf.innerRangeRight = Handles.ScaleValueHandle(tf.innerRangeRight, (verts[0] - verts[3]) / 2 + verts[3], Quaternion.identity, 4, Handles.RectangleHandleCap, 0);
        */
    }
}
