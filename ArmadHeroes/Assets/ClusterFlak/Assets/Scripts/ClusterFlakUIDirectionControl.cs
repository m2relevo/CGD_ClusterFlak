using UnityEngine;

public class ClusterFlakUIDirectionControl : MonoBehaviour
{
    // This class is used to make sure world space UI
    // elements such as the health bar face the correct direction.

    public bool m_UseRelativeRotation = true;       // Use relative rotation should be used for this gameobject?
    public bool m_UseRelativePosition = true;


    private Quaternion m_RelativeRotation;          // The local rotatation at the start of the scene.
    private Vector3 m_RelativePosition;


    private void Start()
    {
        m_RelativeRotation = transform.parent.localRotation;
        m_RelativePosition = transform.parent.localPosition;
    }


    private void Update()
    {
        if (m_UseRelativeRotation)
        {
            transform.rotation = m_RelativeRotation;
        }

        if (m_UseRelativePosition)
        {
            transform.position = m_RelativePosition;
        }
    }
}

///yeah...I just read the comments and the code. All it does is set its rotation to be relative to whatever its parented to. Kinda coolio.