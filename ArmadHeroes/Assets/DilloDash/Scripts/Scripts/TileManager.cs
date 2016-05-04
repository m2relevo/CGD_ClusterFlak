using UnityEngine;
using System.Collections.Generic;

namespace DilloDash
{
    public class TileManager : MonoBehaviour
    {
        private static TileManager singleton;
        public static TileManager Singleton() { return singleton; }

        private List<tileDataDD> tiles = null;

        void Awake()
        {
            singleton = this;
        }

        // Use this for initialization
        void Start()
        {
            tiles = levelGeneratorDD.instance.getTiles();
        }

        // Update is called once per frame
        void Update()
        {

        }


        // returns the effect of the tile currently on
        public TileEffect whatTileEffect(Vector2 _pos)
        {
            Vector2  _posCart = _pos.toCart();
            Vector2 tempTilePos;
            float cornerDistance;
            for (int i = 0; i < tiles.Count; i++)
            {
                tempTilePos = tiles[i].position.toCart();
                cornerDistance = tiles[i].tileSize / 2;

                if (_posCart.x > (tempTilePos.x - cornerDistance) && _posCart.x < tempTilePos.x + cornerDistance)
                {
                    if (_posCart.y > (tempTilePos.y - cornerDistance) && _posCart.y < tempTilePos.y + cornerDistance)
                    {
                        TileEffect effect = tiles[i].effect;
                        //If a corner tile, work out wether they are on the sand part or the track part
                        if (effect == TileEffect.TRACKCORNER_L || effect == TileEffect.TRACKCORNER_R || effect == TileEffect.TRACKCORNER_U || effect == TileEffect.TRACKCORNER_D)
                        {                            
                            switch(effect)
                            {
                                case TileEffect.TRACKCORNER_L:
                                    if (_pos.x < tiles[i].position.x)
                                    {
                                        return TileEffect.TRACK;
                                    }
                                    else
                                        return TileEffect.SAND;
                                    
                                case TileEffect.TRACKCORNER_R:
                                    if (_pos.x > tiles[i].position.x)
                                    {
                                        return TileEffect.TRACK;
                                    }
                                    else
                                        return TileEffect.SAND;
                                    
                                case TileEffect.TRACKCORNER_D:
                                    if (_pos.y < tiles[i].position.y)
                                    {
                                        return TileEffect.TRACK;
                                    }
                                    else
                                        return TileEffect.SAND;
                                    
                                case TileEffect.TRACKCORNER_U:
                                    if (_pos.y > tiles[i].position.y)
                                    {
                                        return TileEffect.TRACK;
                                    }
                                    else
                                        return TileEffect.SAND;                                    
                                
                            }                            
                        }
                        else
                        {
                            return effect;
                        }                        
                    }
                }
            }
            return 0;
        }

        TileEffect sortCorners(TileEffect _tile)
        {
            if (_tile == TileEffect.TRACKCORNER_L || _tile == TileEffect.TRACKCORNER_R || _tile == TileEffect.TRACKCORNER_U || _tile == TileEffect.TRACKCORNER_D)
            {
                return _tile;
            }
            else
            {
                return _tile;
            }
        }

        // returns the gameobject of the tile currently on
        public GameObject whichTile(Vector2 pos)
        {
            Vector2 intPos = new Vector2((pos.x / transform.localScale.x), (pos.y / transform.localScale.y));

            for (int i = 0; i < tiles.Count; ++i)
            {
                float distance = Vector2.Distance(new Vector2(tiles[i].position.x / transform.localScale.x, tiles[i].position.y / transform.localScale.x).toCart(), intPos.toCart());
                float minDistance = (tiles[i].tileSize / 2) / transform.localScale.x + 0.05f;

                if (distance < minDistance)
                {
                    return tiles[i].gameObject;
                }
            }

            Debug.Log("Error - not on tile");
            return null; // does nothing
        }
    }
}
