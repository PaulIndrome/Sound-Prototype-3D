using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(handleTest))]
public class handleTestEditor : Editor {

    //for a lack of "null" instantiation, do with unlikely to reach maxValues
    Vector2 lastPos = new Vector2(float.MaxValue, float.MaxValue);
    handleTest targ;

    void OnSceneGUI()
    {
        targ = (handleTest)target; //get target

        handleTest targetCopy = targ;

        EditorGUI.BeginChangeCheck();
        //instantiate at parent's position
        if (lastPos == new Vector2(float.MaxValue, float.MaxValue))
        {
            lastPos = targ.transform.position;
        }


        if(Mathf.Abs(targ.innerRangeUp - targ.innerRangeDown) <= 0.25f || Mathf.Abs(targ.innerRangeRight - targ.innerRangeLeft) <= 0.25f)
        {
            targ.innerRangeDown -= 0.25f;
            targ.innerRangeUp += 0.25f;
            targ.innerRangeRight += 0.25f;
            targ.innerRangeLeft -= 0.25f;
        }
        

        //if parent's position changes
        if (targ.transform.hasChanged)
        {
            Vector2 diff = (Vector2)targ.transform.position - lastPos; //difference vector goal - current position
            /*
             * update ranges with difference vector's x and y values
             */
            targ.outerRangeLeft += diff.x;
            targ.outerRangeRight += diff.x;
            targ.outerRangeDown += diff.y;
            targ.outerRangeUp += diff.y;

            targ.innerRangeLeft += diff.x;
            targ.innerRangeRight += diff.x;
            targ.innerRangeDown += diff.y;
            targ.innerRangeUp += diff.y;

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
        Rect innerRect = Rect.MinMaxRect(targ.innerRangeLeft, targ.innerRangeDown, targ.innerRangeRight, targ.innerRangeUp);

        //use the rectangle's Min and Max range to generate the SolidRectangle ... just for that, damn.
        Vector3[] rectVerts = new Vector3[4]
       {
            new Vector3(innerRect.xMax, innerRect.yMax, 0),
            new Vector3(innerRect.xMin, innerRect.yMax, 0),
            new Vector3(innerRect.xMin, innerRect.yMin, 0),
            new Vector3(innerRect.xMax, innerRect.yMin, 0)
       };

        Handles.DrawSolidRectangleWithOutline(rectVerts, new Color(1, 1, 0, 0.05f), Color.yellow);

        //draw the Handles for the ranges according to the rectangles Min and Max values
        Handles.color = Color.green;
        targ.innerRangeUp = Handles.FreeMoveHandle(new Vector3((innerRect.xMax + innerRect.xMin) / 2, innerRect.yMax, 0),
                Quaternion.identity, .75f, new Vector3(.5f, .5f, .5f), Handles.RectangleHandleCap).y;
        Handles.color = Color.red;
        targ.innerRangeLeft = Handles.FreeMoveHandle(new Vector3(innerRect.xMin, (innerRect.yMax + innerRect.yMin) / 2, 0),
                Quaternion.identity, .75f, new Vector3(.5f, .5f, .5f), Handles.RectangleHandleCap).x;
        Handles.color = Color.green;
        targ.innerRangeDown = Handles.FreeMoveHandle(new Vector3((innerRect.xMax + innerRect.xMin) / 2, innerRect.yMin, 0),
                Quaternion.identity, .75f, new Vector3(.5f, .5f, .5f), Handles.RectangleHandleCap).y;
        Handles.color = Color.red;
        targ.innerRangeRight = Handles.FreeMoveHandle(new Vector3(innerRect.xMax, (innerRect.yMax + innerRect.yMin) / 2, 0),
                Quaternion.identity, .75f, new Vector3(.5f, .5f, .5f), Handles.RectangleHandleCap).x;

        // what happens if the ranges conflict?
        /*
         * it occurs that pushing a range into the opposing range moves the entire rectangle
         */
        if (targ.innerRangeDown > targ.innerRangeUp)
        {
            float temp = targ.innerRangeDown;
            targ.innerRangeDown = targ.innerRangeUp;
            targ.innerRangeUp = temp;
        }
        if (targ.innerRangeLeft > targ.innerRangeRight)
        {
            float temp = targ.innerRangeLeft;
            targ.innerRangeLeft = targ.innerRangeRight;
            targ.innerRangeRight = temp;
        }

        Handles.color = Color.blue;

        /*
         * Following is the code that enables the center handle to move all ranges and thus the entire rectangle
         */
        EditorGUI.BeginChangeCheck();
        //rectangle was here
        Vector2 prevCenter = innerRect.center;
        //rectangle will be here
        Vector2 afterCenter = Handles.FreeMoveHandle(prevCenter, Quaternion.identity, 1f, new Vector3(0.5f, 0.5f, 0.5f), Handles.RectangleHandleCap);
        if (EditorGUI.EndChangeCheck())
        {
            // get difference of rectangles position and change the RANGES by the difference's x and y values, not the rectangle!
            Vector2 centerDiff = afterCenter - prevCenter;
            targ.innerRangeLeft += centerDiff.x;
            targ.innerRangeRight += centerDiff.x;
            targ.innerRangeDown += centerDiff.y;
            targ.innerRangeUp += centerDiff.y;
        }

        targ.innerRectPos = innerRect.position;
        targ.innerRectSize = innerRect.size;

    }

    void handleOuterRanges()
    {
        /*EditorGUI.BeginChangeCheck();
        Vector2 prevPos = targ.transform.position;
        Vector2 posDiff = new Vector2((targ.outerRangeRight - targ.outerRangeLeft), (targ.outerRangeUp - targ.outerRangeDown)) - prevPos;
        if (EditorGUI.EndChangeCheck())
        {
            
            Vector2 newPos = prevPos + posDiff;
            targ.transform.position = newPos;
        }*/
        
        //create outer rectangle according to the updated ranges
        Rect outerRect = Rect.MinMaxRect(targ.outerRangeLeft, targ.outerRangeDown, targ.outerRangeRight, targ.outerRangeUp);

        //use the rectangle's Min and Max range to generate the SolidRectangle ... just for that, damn.
        Vector3[] rectVerts = new Vector3[4]
       {
            new Vector3(outerRect.xMax, outerRect.yMax, 0.1f),
            new Vector3(outerRect.xMin, outerRect.yMax, 0.1f),
            new Vector3(outerRect.xMin, outerRect.yMin, 0.1f),
            new Vector3(outerRect.xMax, outerRect.yMin, 0.1f)
       };

        Handles.DrawSolidRectangleWithOutline(rectVerts, new Color(1, 0, 1, 0.05f), Color.magenta);

        //draw the Handles for the ranges according to the rectangles Min and Max values
        Handles.color = Color.green;
        targ.outerRangeUp = Handles.FreeMoveHandle(new Vector3((outerRect.xMax + outerRect.xMin) / 2, outerRect.yMax, 0),
                Quaternion.identity, .75f, new Vector3(.5f, .5f, .5f), Handles.RectangleHandleCap).y;
        Handles.color = Color.red;
        targ.outerRangeLeft = Handles.FreeMoveHandle(new Vector3(outerRect.xMin, (outerRect.yMax + outerRect.yMin) / 2, 0),
                Quaternion.identity, .75f, new Vector3(.5f, .5f, .5f), Handles.RectangleHandleCap).x;
        Handles.color = Color.green;
        targ.outerRangeDown = Handles.FreeMoveHandle(new Vector3((outerRect.xMax + outerRect.xMin) / 2, outerRect.yMin, 0),
                Quaternion.identity, .75f, new Vector3(.5f, .5f, .5f), Handles.RectangleHandleCap).y;
        Handles.color = Color.red;
        targ.outerRangeRight = Handles.FreeMoveHandle(new Vector3(outerRect.xMax, (outerRect.yMax + outerRect.yMin) / 2, 0),
                Quaternion.identity, .75f, new Vector3(.5f, .5f, .5f), Handles.RectangleHandleCap).x;

        // what happens if the ranges conflict?
        /*
         * it occurs that pushing a range into the opposing range moves the entire rectangle
         */
        if (targ.outerRangeDown > targ.innerRangeDown)
        {
            float temp = targ.outerRangeDown;
            targ.outerRangeDown = targ.innerRangeDown;
            targ.innerRangeDown = temp;
        }
        if (targ.outerRangeUp < targ.innerRangeUp)
        {
            float temp = targ.outerRangeUp;
            targ.outerRangeUp = targ.innerRangeUp;
            targ.innerRangeUp = temp;
        }
        if (targ.outerRangeLeft > targ.innerRangeLeft)
        {
            float temp = targ.outerRangeLeft;
            targ.outerRangeLeft = targ.innerRangeLeft;
            targ.innerRangeLeft = temp;
        }
        if (targ.outerRangeRight < targ.innerRangeRight)
        {
            float temp = targ.outerRangeRight;
            targ.outerRangeRight = targ.innerRangeRight;
            targ.innerRangeRight = temp;
        }

       





    }
}
