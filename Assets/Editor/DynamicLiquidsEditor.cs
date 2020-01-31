using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (DynamicLiquid))]
public class DynamicLiquidsEditor : Editor
{
    private void OnSceneGUI()
    {
        DynamicLiquid liquid = (DynamicLiquid)target;
        Handles.color = Color.white;
        Vector3[] lines = {
            new Vector3(liquid.bound.left+liquid.transform.position.x, liquid.bound.top+liquid.transform.position.y, 0), new Vector3(liquid.bound.right+liquid.transform.position.x, liquid.bound.top+liquid.transform.position.y, 0),
            new Vector3(liquid.bound.right+liquid.transform.position.x, liquid.bound.top+liquid.transform.position.y, 0), new Vector3(liquid.bound.right+liquid.transform.position.x, liquid.bound.bottom+liquid.transform.position.y, 0),
            new Vector3(liquid.bound.right+liquid.transform.position.x, liquid.bound.bottom+liquid.transform.position.y, 0), new Vector3(liquid.bound.left+liquid.transform.position.x, liquid.bound.bottom+liquid.transform.position.y, 0),
            new Vector3(liquid.bound.left+liquid.transform.position.x, liquid.bound.bottom+liquid.transform.position.y, 0), new Vector3(liquid.bound.left+liquid.transform.position.x, liquid.bound.top+liquid.transform.position.y, 0)
        };
        Handles.DrawLines(lines);

    }
}
