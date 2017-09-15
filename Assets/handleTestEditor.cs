using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(handleTest))]
public class handleTestEditor : Editor {

    //for a lack of "null" instantiation, do with unlikely to reach maxValues
    Vector2 lastPos;// = new Vector2(float.MaxValue, float.MaxValue);
    handleTest targ;

    public Rect outerRect;
    public Rect innerRect;

    void OnSceneGUI()
    {
        targ = (handleTest)target; //get target

        outerRect = targ.outerRect;
        innerRect = targ.innerRect;

        handleTest targetCopy = targ;

        EditorGUI.BeginChangeCheck();
        //instantiate at parent's position
        /*if (lastPos == new Vector2(float.MaxValue, float.MaxValue))
        {
            lastPos = targ.transform.position;
        }*/


        if(Mathf.Abs(targ.innerRect.yMax - targ.innerRect.yMin) <= 0.25f || Mathf.Abs(targ.innerRect.xMax - targ.innerRect.xMin) <= 0.25f)
        {
            targ.innerRect.yMin -= 0.25f;
            targ.innerRect.yMax += 0.25f;
            targ.innerRect.xMax += 0.25f;
            targ.innerRect.xMin -= 0.25f;
        }
        

        //if parent's position changes
        if (targ.transform.hasChanged)
        {
            Vector2 diff = (Vector2)targ.transform.position - lastPos; //difference vector goal - current position
            /*
             * update ranges with difference vector's x and y values
             */
            targ.outerRect.xMin += diff.x;
            targ.outerRect.xMax += diff.x;
            targ.outerRect.yMin += diff.y;
            targ.outerRect.yMax += diff.y;

            targ.innerRect.xMin += diff.x;
            targ.innerRect.xMax += diff.x;
            targ.innerRect.yMin += diff.y;
            targ.innerRect.yMax += diff.y;

            targ.transform.hasChanged = false;

            //update lastPos with parent's position for future evaluations
            lastPos = targ.transform.position;
        }

        handleInnerRanges();

        handleOuterRanges();

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(targ, "Any change to the ranges or positon");
            targ = targetCopy;
        }


    }

    void handleInnerRanges()
    {
        //create rectangle according to the updated ranges
        //Rect targ.innerRect = Rect.MinMaxRect(targ.innerRect.xMin, targ.innerRect.yMin, targ.innerRect.xMax, targ.innerRect.yMax);

        //use the rectangle's Min and Max range to generate the SolidRectangle ... just for that, damn.
        Vector3[] rectVerts = new Vector3[4]
       {
            new Vector3(targ.innerRect.xMax, targ.innerRect.yMax, 0),
            new Vector3(targ.innerRect.xMin, targ.innerRect.yMax, 0),
            new Vector3(targ.innerRect.xMin, targ.innerRect.yMin, 0),
            new Vector3(targ.innerRect.xMax, targ.innerRect.yMin, 0)
       };

        Handles.DrawSolidRectangleWithOutline(rectVerts, new Color(1, 1, 0, 0.05f), Color.yellow);
        
        //draw the Handles for the ranges according to the rectangles Min and Max values
        Handles.color = Color.green;
        targ.innerRect.yMax = Handles.FreeMoveHandle(new Vector3((targ.innerRect.xMax + targ.innerRect.xMin) / 2, targ.innerRect.yMax, 0),
                Quaternion.identity, .75f, new Vector3(.5f, .5f, .5f), Handles.RectangleHandleCap).y;
        Handles.color = Color.red;
        targ.innerRect.xMin = Handles.FreeMoveHandle(new Vector3(targ.innerRect.xMin, (targ.innerRect.yMax + targ.innerRect.yMin) / 2, 0),
                Quaternion.identity, .75f, new Vector3(.5f, .5f, .5f), Handles.RectangleHandleCap).x;
        Handles.color = Color.green;
        targ.innerRect.yMin = Handles.FreeMoveHandle(new Vector3((targ.innerRect.xMax + targ.innerRect.xMin) / 2, targ.innerRect.yMin, 0),
                Quaternion.identity, .75f, new Vector3(.5f, .5f, .5f), Handles.RectangleHandleCap).y;
        Handles.color = Color.red;
        targ.innerRect.xMax = Handles.FreeMoveHandle(new Vector3(targ.innerRect.xMax, (targ.innerRect.yMax + targ.innerRect.yMin) / 2, 0),
                Quaternion.identity, .75f, new Vector3(.5f, .5f, .5f), Handles.RectangleHandleCap).x;

        // what happens if the ranges conflict?
        /*
            * it occurs that pushing a range into the opposing range moves the entire rectangle
            */
        if (targ.innerRect.yMin > targ.innerRect.yMax)
        {
            float temp = targ.innerRect.yMin;
            targ.innerRect.yMin = targ.innerRect.yMax;
            targ.innerRect.yMax = temp;
        }
        if (targ.innerRect.xMin > targ.innerRect.xMax)
        {
            float temp = targ.innerRect.xMin;
            targ.innerRect.xMin = targ.innerRect.xMax;
            targ.innerRect.xMax = temp;
        }

        if (targ.innerRect.yMin < targ.outerRect.yMin)
        {
            targ.innerRect.yMin = targ.outerRect.yMin;
        }
        if (targ.innerRect.xMin < targ.outerRect.xMin)
        {
            targ.innerRect.xMin = targ.outerRect.xMin;
        }
        if (targ.innerRect.yMax > targ.outerRect.yMax)
        {
            targ.innerRect.yMax = targ.outerRect.yMax;
        }
        if (targ.innerRect.xMax > targ.outerRect.xMax)
        {
            targ.innerRect.xMax = targ.outerRect.xMax;
        }


        Handles.color = Color.magenta;

        /*
         * Following is the code that enables the center handle to move all ranges and thus the entire rectangle
         */
        EditorGUI.BeginChangeCheck();
        //rectangle was here
        Vector2 prevCenter = targ.innerRect.center;
        //rectangle will be here
        Vector2 afterCenter = Handles.FreeMoveHandle((Vector3)prevCenter, Quaternion.identity, 0.75f, new Vector3(0.5f, 0.5f, 0.5f), Handles.CircleHandleCap);
        if (EditorGUI.EndChangeCheck())
        {
            // get difference of rectangles position and change the RANGES by the difference's x and y values, not the rectangle!
            Vector2 centerDiff = afterCenter - prevCenter;
            if (targ.innerRect.xMin + centerDiff.x >= targ.outerRect.xMin && targ.innerRect.xMax + centerDiff.x <= targ.outerRect.xMax)
            {
                targ.innerRect.xMin += centerDiff.x;
                targ.innerRect.xMax += centerDiff.x;
            } else
            {
                centerDiff.x = (targ.innerRect.xMin + centerDiff.x < targ.outerRect.xMin) ? 
                        targ.outerRect.xMin - targ.innerRect.xMin :
                        targ.outerRect.xMax - targ.innerRect.xMax;
                targ.innerRect.xMin += centerDiff.x;
                targ.innerRect.xMax += centerDiff.x;
            }
            if (targ.innerRect.yMin + centerDiff.y >= targ.outerRect.yMin && targ.innerRect.yMax + centerDiff.y <= targ.outerRect.yMax)
            {
                targ.innerRect.yMin += centerDiff.y;
                targ.innerRect.yMax += centerDiff.y;
            }
            else
            {
                centerDiff.y = (targ.innerRect.yMin + centerDiff.y < targ.outerRect.yMin) ?
                        targ.outerRect.yMin - targ.innerRect.yMin :
                        targ.outerRect.yMax - targ.innerRect.yMax;
                targ.innerRect.yMin += centerDiff.y;
                targ.innerRect.yMax += centerDiff.y;
            }

        }

        //targ.targ.innerRectPos = targ.innerRect.position;
        //targ.targ.innerRectSize = targ.innerRect.size;

    }

    void handleOuterRanges()
    {
        //create outer rectangle according to the updated ranges
        //Rect targ.outerRect = Rect.MinMaxRect(targ.outerRect.xMin, targ.outerRect.yMin, targ.outerRect.xMax, targ.outerRect.yMax);

        //use the rectangle's Min and Max range to generate the SolidRectangle ... just for that, damn.
        Vector3[] rectVerts = new Vector3[4]
       {
            new Vector3(targ.outerRect.xMax, targ.outerRect.yMax, 0.1f),
            new Vector3(targ.outerRect.xMin, targ.outerRect.yMax, 0.1f),
            new Vector3(targ.outerRect.xMin, targ.outerRect.yMin, 0.1f),
            new Vector3(targ.outerRect.xMax, targ.outerRect.yMin, 0.1f)
       };

        Handles.color = Color.blue;
        Handles.DrawSolidRectangleWithOutline(rectVerts, new Color(0, 0, 1, 0.05f), Color.blue);

        EditorGUI.BeginChangeCheck();
        //draw the Handles for the ranges according to the rectangles Min and Max values
        Handles.color = Color.red;
        targ.outerRect.yMax = Handles.FreeMoveHandle(new Vector3((targ.outerRect.xMax + targ.outerRect.xMin) / 2, targ.outerRect.yMax, 0),
                Quaternion.identity, .75f, new Vector3(.5f, .5f, .5f), Handles.RectangleHandleCap).y;
        Handles.color = Color.green;
        targ.outerRect.xMin = Handles.FreeMoveHandle(new Vector3(targ.outerRect.xMin, (targ.outerRect.yMax + targ.outerRect.yMin) / 2, 0),
                Quaternion.identity, .75f, new Vector3(.5f, .5f, .5f), Handles.RectangleHandleCap).x;
        Handles.color = Color.red;
        targ.outerRect.yMin = Handles.FreeMoveHandle(new Vector3((targ.outerRect.xMax + targ.outerRect.xMin) / 2, targ.outerRect.yMin, 0),
                Quaternion.identity, .75f, new Vector3(.5f, .5f, .5f), Handles.RectangleHandleCap).y;
        Handles.color = Color.green;
        targ.outerRect.xMax = Handles.FreeMoveHandle(new Vector3(targ.outerRect.xMax, (targ.outerRect.yMax + targ.outerRect.yMin) / 2, 0),
                Quaternion.identity, .75f, new Vector3(.5f, .5f, .5f), Handles.RectangleHandleCap).x;

        if (EditorGUI.EndChangeCheck())
        {
            targ.transform.position = new Vector3((targ.outerRect.xMin + targ.outerRect.xMax) * 0.5f, (targ.outerRect.yMax + targ.outerRect.yMin) * 0.5f, targ.transform.position.z);
            lastPos = targ.transform.position;
        }

        // what happens if the ranges conflict?
        /*
         * it occurs that pushing a range into the opposing range moves the entire rectangle
         */

        if (targ.outerRect.yMin > targ.outerRect.yMax)
        {
            float temp = targ.outerRect.yMin;
            targ.outerRect.yMin = targ.outerRect.yMax;
            targ.outerRect.yMax = temp;
        }
        if (targ.outerRect.xMin > targ.outerRect.xMax)
        {
            float temp = targ.outerRect.xMin;
            targ.outerRect.xMin = targ.outerRect.xMax;
            targ.outerRect.xMax = temp;
        }






    }
}
