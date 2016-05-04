using UnityEngine;
using System.Collections;

namespace DilloDash
{

    [ExecuteInEditMode]
    public class PositionTest : MonoBehaviour
    {
        public TileEffect tile;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            tile = TileManager.Singleton().whatTileEffect(transform.position);
        }
    }
}