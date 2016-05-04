using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace DilloDash
{
    public class TextureLoaderDD : MonoBehaviour
    {
        private static TextureLoaderDD singleton;
        public static TextureLoaderDD Singleton() { return singleton; }
       
        // texture properties
        [SerializeField] private Texture2D mapTex = null;
        [SerializeField] private Color[] pix = null;
      
        private int width;
        private int height;

        void Awake()
        {
            singleton = this;
            int x = Mathf.FloorToInt(gameObject.transform.position.x);
            int y = Mathf.FloorToInt(gameObject.transform.position.y);

            width = Mathf.FloorToInt(mapTex.width);
            height = Mathf.FloorToInt(mapTex.height);

            pix = mapTex.GetPixels(x, y, width, height);

            gameObject.GetComponent<levelGeneratorDD>().generateLevel(width, height);
        }

        public Color GetTexturePixelColour(float _x, float _y)
        {
            return FindPosOnTex(_x, _y);
        }

        Color FindPosOnTex(float _x, float _y)
        {
            //Determined scaled x position on map
            /* _x += transform.localScale.x / 2;
             _x /= transform.localScale.x;
             _x *= width;
             
            //Determine scaled y position on map
            _y += transform.localScale.y / 2;
            //_y = transform.localScale.y - _y;
            _y /= transform.localScale.y;
            _y *= height;
            */

            //_x *= 100; // scale 1 pixel to 1 metre
            //_x /= transform.localScale.x; // scale according to texture size
            //_x += (width / 2); // texture starts at 0,0 top left corner
           

            //_y *= -100;
            //_y /= transform.localScale.y;
            //_y += (height / 2);
           
            
    

            ////Floor values and return colour
            int x = Mathf.FloorToInt(_x);
            int y = Mathf.FloorToInt(_y);
            return pix[(width * y) + x];
        } 
    }
}
