using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CameraShakeHandler : MonoBehaviour {

    #region Singleton
    public static CameraShakeHandler Instance;
    #endregion

    private Vector3 camStartPosition;
    Tweener m_tweener;

    // Use this for initialization
    void Awake () {
        Instance = this;
        camStartPosition = Camera.main.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Shake()
    {
        m_tweener.Kill();
        Camera.main.transform.position = camStartPosition;
        m_tweener = Camera.main.DOShakePosition(1.0f, 1);
    }
}
