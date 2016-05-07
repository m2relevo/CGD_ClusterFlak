/***********************************
 * ExtensionMethods.cs
 * Created by Daniel Weston 28/01/16
 * *********************************/
using UnityEngine;
using System.Collections;

public static class ExtensionMethods
{
    /// <summary>
    /// Converts the values within a Vector2 to 
    /// isometric coordinates 
    /// </summary>
    /// <param name="_val"></param>
    /// <returns></returns>
    public static Vector2 toIso(this Vector2 _val)
    {
        return new Vector2((_val.x - _val.y), (_val.x + _val.y) / 2.0f);
    }

    /// <summary>
    /// Converts the values within a Vector3 to 
    /// isometric coordinates 
    /// </summary>
    /// <param name="_val"></param>
    /// <returns></returns>
    public static Vector3 toIso(this Vector3 _val)
    {
        return new Vector3((_val.x - _val.y), (_val.x + _val.y) / 2.0f, 0.0f);
    }

    /// <summary>
    /// Converts the values within a Vector3 to 
    /// isometric coordinates while maintaining 
    /// standard x,y and z directions. Used for
    /// getting directional vectors from input.
    /// </summary>
    /// <param name="_val"></param>
    /// <returns></returns>
    public static Vector3 toDirectionalIso(this Vector3 _val)
    {
        return toIso(Quaternion.AngleAxis(45, Vector3.back) * _val);
    }

    /// <summary>
    /// Converts isometric values within a Vector2
    /// back into cart values
    /// </summary>
    /// </summary>
    /// <param name="_val"></param>
    /// <returns></returns>
    public static Vector2 toCart(this Vector2 _val)
    {
        return (new Vector2((2 * _val.y + _val.x) / 2.0f, (2 * _val.y - _val.x) / 2.0f));
    }

    /// <summary>
    /// Converts isometric values within a Vector3
    /// back into cart values
    /// </summary>
    /// <param name="_val"></param>
    /// <returns></returns>
    public static Vector3 toCart(this Vector3 _val)
    {
        return (new Vector3((_val.y * 2.0f + _val.x) / 2.0f, (2 * _val.y - _val.x) / 2.0f, 0.0f));
    }

    /// <summary>
    /// Converts isometric values within a Vector3
    /// back into cart values while maintaining 
    /// standard x,y and z directions.
    /// </summary>
    /// <param name="_val"></param>
    /// <returns></returns>
    public static Vector2 toDirectionalCart(this Vector2 _val)
    {
        return toIso(Quaternion.AngleAxis(45, Vector3.back) * _val);
    }

	public static float toIsoAngle(this float _val)
	{
		float zAngle = _val;
		float zAngleAbs = Mathf.Abs (zAngle);

		int[] isoAngles = new int[5] { 0,30,90,150,180 };
		
		int currentClosest = isoAngles [0];
		for(int i = 1; i<isoAngles.Length; i++){
			if(Mathf.Abs(zAngleAbs-isoAngles[i]) < Mathf.Abs(zAngleAbs-currentClosest)){
				currentClosest = isoAngles[i];
			}
		}

		if (zAngle < 0) {
			currentClosest *= -1;
		}

		return currentClosest;
	}
}