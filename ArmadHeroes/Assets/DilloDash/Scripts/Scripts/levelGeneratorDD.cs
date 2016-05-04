using UnityEngine;
using System.Collections.Generic;

namespace DilloDash
{
    public class levelGeneratorDD : MonoBehaviour
    {
        private static levelGeneratorDD _instance = null;
        public static levelGeneratorDD instance { get { return _instance; } }

        // ground tile
        [SerializeField] GameObject groundTile = null; // effect 2 - slow

        // boost tile
        [SerializeField] GameObject boostTile = null; // effect 3 - fast

        // slip tile
        [SerializeField] GameObject slipTile = null; // effect 4 - slidy

        // All tiles for the track
        [SerializeField] GameObject trackTile = null; // effect 1 - track
        [SerializeField] GameObject trackCornerTileTopRight = null;
        [SerializeField] GameObject trackCornerTileTopleft = null;
        [SerializeField] GameObject trackCornerTileBottomRight = null;
        [SerializeField] GameObject trackCornerTileBottomLeft = null;        

        // Colours
        [SerializeField] Color groundColour = Color.white;        
        [SerializeField] Color trackColour = Color.white;
        [SerializeField] Color trackCornerColour = Color.white;
        [SerializeField] Color boostColour = Color.white;
        [SerializeField] Color slipColour = Color.white;
        [SerializeField] Color tarmacColour = Color.white;

        // size/spacing of each tile
        [SerializeField] float _tileSize = 3.5f;

        public bool powerupDebug = false;
        public Powerup powerupOverride;

        // list/arrays
        List<tileDataDD> tiles;

        int[] effects; // what each tile will do (managed in player manager)

        // Use this for initialization
        void Awake()
        {
            _instance = this;
        }

        // Update is called once per frame
        void Update()
        {

        }

        // public API
        public List<tileDataDD> getTiles()
        {
            return tiles;
        }

        public void generateLevel(float levelWidth, float levelHeight)
        {
            if(tiles == null)
            {
                tiles = new List<tileDataDD>();
            }            

            // initialise the road corners array
            GameObject[] roadCorners = new GameObject[4];
            roadCorners[0] = trackCornerTileBottomLeft;
            roadCorners[1] = trackCornerTileTopleft;
            roadCorners[2] = trackCornerTileBottomRight;
            roadCorners[3] = trackCornerTileTopRight;

            for (int i = 0; i < levelWidth; ++i)
            {
                for(int j = 0; j < levelHeight; ++j)
                {
                    // initialise the new tile
                    GameObject actualTile = null;
                    tileDataDD tile = null;

                    Color activeColour = gameObject.GetComponent<TextureLoaderDD>().GetTexturePixelColour(i, j);

                    // establish what type of tile it will be
                    if (activeColour == trackColour) // track tile
                    {
                        actualTile = Instantiate(trackTile); // instantiate tile
                        tile = actualTile.AddComponent<tileDataDD>(); // add the tile data component
                        tile.effect = TileEffect.TRACK; // assign the effect
                    }
                    else if (activeColour == groundColour) // ground tile
                    {
                        actualTile = Instantiate(groundTile);
                        tile = actualTile.AddComponent<tileDataDD>();
                        tile.effect = TileEffect.SAND;
                    }
                    
                    else if (activeColour == trackCornerColour) // track corner tile
                    {
                        actualTile = Instantiate(calculateTileRotation(i, j, trackColour, roadCorners, roadCorners[3]));
                        GameObject baseTile = Instantiate(calculateBaseTile(i, j, groundColour, groundTile, trackTile));
                        baseTile.transform.parent = actualTile.transform;
                        baseTile.transform.localPosition = Vector3.zero;
                        tile = actualTile.AddComponent<tileDataDD>();
                        tile.effect = returnCorner(i,j, trackColour, roadCorners);
                        
                    }
                    else if (activeColour == boostColour) // boost tile
                    {
                        actualTile = Instantiate(boostTile); // instantiate tile
                        tile = actualTile.AddComponent<tileDataDD>(); // add the tile data component
                        tile.effect = TileEffect.BOOST; // assign the effect
                        actualTile.AddComponent<spriteSort>();
                        actualTile.GetComponent<SpriteRenderer>().sortingLayerName = "Background";

                        GameObject baseTile = Instantiate(calculateBaseTile(i, j, groundColour, groundTile, trackTile));
                        baseTile.transform.parent = actualTile.transform;
                        baseTile.transform.localPosition = Vector3.zero;
                    }
                    else if (activeColour == slipColour) // Slip tile
                    {
                        actualTile = Instantiate(slipTile); // instantiate tile
                        tile = actualTile.AddComponent<tileDataDD>(); // add the tile data component
                        tile.effect = TileEffect.SLIP; // assign the effect
                    }
                    else if (activeColour == tarmacColour)// Tarmac Tile, should have road placed over.
                    {
                        actualTile = Instantiate(groundTile); // instantiate tile
                        tile = actualTile.AddComponent<tileDataDD>(); // add the tile data component
                        tile.effect = TileEffect.TARMAC; // assign the effect
                    }
                    else // default 
                    {
                        actualTile = Instantiate(groundTile);
                        tile = actualTile.AddComponent<tileDataDD>();
                        tile.effect = TileEffect.DEFAULT;
                    }

                    tile.tileSize = _tileSize;
                    tile.position = new Vector2(i * tile.tileSize, j * tile.tileSize).toIso();

                    // define tile's pointer to world counterpart
                    tile.tilePointer = actualTile;
                    // assign world tile's position etc
                    actualTile.transform.position = tile.position;
                    actualTile.transform.localScale *= _tileSize;
                    actualTile.transform.parent = gameObject.transform;

                    // add to list of tiles (doesn't work)
                    tiles.Add(tile);
                }
            }
        }

        // calculates which base tile to place under the corner tile
        GameObject calculateBaseTile(int i, int j, Color groundColour, GameObject groundTile, GameObject trackTile)
        {
            bool[] adjacentTiles = new bool[4];

            adjacentTiles[0] = gameObject.GetComponent<TextureLoaderDD>().GetTexturePixelColour(i - 1, j) == groundColour ? true : false; // left
            adjacentTiles[1] = gameObject.GetComponent<TextureLoaderDD>().GetTexturePixelColour(i + 1, j) == groundColour ? true : false; // right
            adjacentTiles[2] = gameObject.GetComponent<TextureLoaderDD>().GetTexturePixelColour(i, j - 1) == groundColour ? true : false; // up
            adjacentTiles[3] = gameObject.GetComponent<TextureLoaderDD>().GetTexturePixelColour(i, j + 1) == groundColour ? true : false; // down

            int counter = 0;

            // iterate through the tiles
            for(int k = 0; k < adjacentTiles.Length; k++)
            {
                //if the tile is true
                if(adjacentTiles[k])
                {
                    // increment counter
                    ++counter;
                    if(counter >= 2)
                    { 
                        // return the ground tile
                        return groundTile;
                    }
                }
            }
            // else return the track tile
            return trackTile;
        }

        // calculates which corner tile rotation to return
        GameObject calculateTileRotation(int i, int j, Color targetColour, GameObject[] tileCorners, GameObject defaultTile)
        {
            bool[] adjacentTiles = new bool[4];

            adjacentTiles[0] = gameObject.GetComponent<TextureLoaderDD>().GetTexturePixelColour(i - 1, j) == targetColour ? true : false; // left
            adjacentTiles[1] = gameObject.GetComponent<TextureLoaderDD>().GetTexturePixelColour(i + 1, j) == targetColour ? true : false; // right
            adjacentTiles[2] = gameObject.GetComponent<TextureLoaderDD>().GetTexturePixelColour(i, j - 1) == targetColour ? true : false; // up
            adjacentTiles[3] = gameObject.GetComponent<TextureLoaderDD>().GetTexturePixelColour(i, j + 1) == targetColour ? true : false; // down

            if (adjacentTiles[0] && adjacentTiles[2]) // bottom left
            {
                return tileCorners[0];
            }
            if (adjacentTiles[0] && adjacentTiles[3]) // top left
            {
                return tileCorners[1];
            }
            if (adjacentTiles[1] && adjacentTiles[2]) // bottom right
            {
                return tileCorners[2];
            }
            if (adjacentTiles[0] && adjacentTiles[2]) // top right
            {
                return tileCorners[3];
            }

            return defaultTile; // not gonna lie - this is always going to be the top right tile... don't know why
        }

        TileEffect returnCorner (int i, int j, Color targetColour, GameObject[] tileCorners)
        {
            bool[] adjacentTiles = new bool[4];

            adjacentTiles[0] = gameObject.GetComponent<TextureLoaderDD>().GetTexturePixelColour(i - 1, j) == targetColour ? true : false; // left
            adjacentTiles[1] = gameObject.GetComponent<TextureLoaderDD>().GetTexturePixelColour(i + 1, j) == targetColour ? true : false; // right
            adjacentTiles[2] = gameObject.GetComponent<TextureLoaderDD>().GetTexturePixelColour(i, j - 1) == targetColour ? true : false; // up
            adjacentTiles[3] = gameObject.GetComponent<TextureLoaderDD>().GetTexturePixelColour(i, j + 1) == targetColour ? true : false; // down

            if (adjacentTiles[0] && adjacentTiles[2]) // bottom left
            {
                return TileEffect.TRACKCORNER_D;
            }
            if (adjacentTiles[0] && adjacentTiles[3]) // top left
            {
                return TileEffect.TRACKCORNER_L;
            }
            if (adjacentTiles[1] && adjacentTiles[2]) // bottom right
            {
                return TileEffect.TRACKCORNER_R;
            }
            if (adjacentTiles[0] && adjacentTiles[2]) // top right
            {
                return TileEffect.TRACKCORNER_U;
            }
            return TileEffect.TRACKCORNER_U;

        }

    }

    public enum TileEffect
    {
        TRACK, SAND, BOOST, POWERUP, SLIP,  TARMAC, DEFAULT, TRACKCORNER_U, TRACKCORNER_D, TRACKCORNER_L, TRACKCORNER_R
    }
}
