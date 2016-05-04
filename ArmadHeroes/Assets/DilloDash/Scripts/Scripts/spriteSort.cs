using UnityEngine;
using System.Collections;

namespace DilloDash
{
    [ExecuteInEditMode]
    public class spriteSort : MonoBehaviour
    {
        
        public Transform targetTransform;
        SpriteRenderer myRenderer;

        void Start()
        {
            myRenderer = GetComponent<SpriteRenderer>();
            
        }

        void Update()
        {
            if (targetTransform == null)
            {
                targetTransform = transform;
            }
            myRenderer.sortingOrder = -(int)(targetTransform.position.y);
        }

    }
}
