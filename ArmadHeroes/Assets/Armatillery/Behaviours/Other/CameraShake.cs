/// <summary>
/// CameraShake class created and implemented by Craig Tinney - ?
/// Updated by Daniel Weston 10/01/16
/// </summary>
using UnityEngine;
using System.Collections;
public class CameraShake : MonoBehaviour
{
    public GameObject CameraParent;//attached parent object of camera

    /// <summary>
    /// To be called by anything that wishes to shakeadat camera
    /// </summary>
    public void ShakeCamera()
    {
        //iTween.ShakePosition(CameraParent, iTween.Hash("x", 0.1f, "y", 0.1f, "isLocal", false, "time", 0.6f));
    }
}
