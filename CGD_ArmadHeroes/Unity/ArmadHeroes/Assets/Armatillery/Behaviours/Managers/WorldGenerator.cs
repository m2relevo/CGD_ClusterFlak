/// <summary>
/// Generates the world paths using A* pathfinding
/// Created and implemented by David Dunnings - 12/01/16
/// Commented by Sam Endean - 15/01/16
/// Re-commented by David Dunnings - 02/02/16
/// </summary>

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using EaseyEase;

namespace Armatillery
{

    public class WorldTile
    {
        public Vector2 Position; //Tile's position in the world
        public Vector2 index; //Tile's index value
                              //0 = Not on a path
        public int pathID = 0; //Tile path
        public float weight = Random.Range(0f, 11f); //Random weight for the pathfinding function
        public float h = 0f; //Heuristic value
        public int g = 0;
        public bool m_checked = false;
        public GameObject m_worldTileObject;
    }

    public class WorldGenerator : MonoBehaviour
    {
        private static WorldGenerator m_instance = null;
        public static WorldGenerator instance { get { return m_instance; } }

        public GameObject m_worldTile;
        public Sprite m_pathSprite;


        Color[] colors = new Color[10];

        public Vector2 m_startPos = Vector2.zero,
             Left, Down, Right, Up;

        public List<Vector2> m_trackStarts, m_trackEnds;

        public int obstacleCount = 10;

        public List<Vector3> obstacleLocations;

        public List<List<WorldTile>> allPaths; //holds all paths (which will be generated)

        public int m_mapWidth = 64, m_mapHeight = 36;

        public float m_tileHeight = 0.3f;

		public WorldTile[,] m_world;

        List<WorldTile> animatedTiles = new List<WorldTile>();

        void Awake()
        {
            m_instance = this;

            //allPaths = new List<List<WorldTile>>();

            ////Define colours for debugging path
            //colors[0] = Color.clear;
            //colors[1] = Color.red;
            //colors[2] = Color.green;
            //colors[3] = Color.blue;
            //colors[4] = Color.cyan;
            //colors[5] = Color.magenta;
            //colors[6] = Color.yellow;
            //colors[7] = Color.black;
            //colors[8] = Color.grey;
            //colors[9] = Color.white;

            ////Create a 2D array of m_mapWidth and m_mapHeight
            //m_world = new WorldTile[m_mapWidth, m_mapHeight];
            ////Setup the tiles and add them to world tile with their index and position
            //for (int x = 0; x < m_mapWidth; x++)
            //{
            //    for (int y = 0; y < m_mapHeight; y++)
            //    {
            //        WorldTile wt = new WorldTile();
            //        wt.index = new Vector2(x, y);
            //        wt.weight = Mathf.PerlinNoise(((float)x / (float)m_mapWidth) * 10f, ((float)y / (float)m_mapHeight) * 10f);
            //        wt.Position = m_startPos + new Vector2((x * m_tileHeight) - ((m_mapWidth * m_tileHeight) / 2f), (y * m_tileHeight) - ((m_mapHeight * m_tileHeight) / 2f));
            //        //wt.Position = new Vector2(x * m_tileHeight, y * m_tileHeight);
            //        GameObject go = Instantiate<GameObject>(m_worldTile);
            //        //go.GetComponent<SpriteRenderer>().color = Color.white * wt.weight; //Viual representation
            //        go.transform.parent = transform;
            //        go.transform.position = wt.Position.toIso();
            //        wt.m_worldTileObject = go;
            //        m_world[x, y] = wt;
            //    }
            //}

            //// create the paths for all 4 positions around the map
            ////4 Players
            //for (int i = 0; i < 4; i++)
            //{
            //    List<WorldTile> path;
            //    path = CreatePath(m_trackStarts[i], m_trackEnds[i]);
            //    StartCoroutine(ShowBuildingPath(path, i));
            //    //adds path to allPaths for further use
            //    allPaths.Add(path);
            //}

            //running = true;
        }

		void Start ()
		{
			//pass data to EnemyManager
			//EnemyManager.instance.SetSpawnPoints(allPaths);
		}

        void GenerateRandomObstacles()
        {
            for (int i = 0; i < obstacleCount; i++)
            {

            }
        }

        /// <summary>
        /// Using the track starts calc the visible the 
        /// viewable game world edges.
        /// </summary>
        void GetWorldCorners()
        {
            Left = m_world[(int)m_trackStarts[0].x, (int)m_trackStarts[0].y].Position.toIso();
            Down = m_world[(int)m_trackStarts[1].x, (int)m_trackStarts[1].y].Position.toIso();
            Right = m_world[(int)m_trackStarts[2].x, (int)m_trackStarts[2].y].Position.toIso();
            Up = m_world[(int)m_trackStarts[3].x, (int)m_trackStarts[3].y].Position.toIso();
        }

        public IEnumerator ShowBuildingPath(List<WorldTile> path, int pathID)
        {
            for (int i = 0; i < path.Count; i++)
            {
                path[i].pathID = pathID + 1;
                yield return new WaitForSeconds(0.001f);
                path[i].m_worldTileObject.GetComponent<SpriteRenderer>().sprite = m_pathSprite;
                path[i].m_worldTileObject.GetComponent<SpriteRenderer>().color = colors[path[i].pathID];
            }
        }

        /// <summary>
        /// Creates the path
        /// </summary>
        /// <returns>The path as a list of tiles</returns>
        /// <param name="start">the start location of the path</param>
        /// <param name="end">the end location of the path</param>
        public List<WorldTile> CreatePath(Vector2 start, Vector2 end)
        {
			//if end is not supplied, just path to the tower
			if (end == start)
			{
				end = m_trackEnds [0];
			}

            List<WorldTile> path = new List<WorldTile>();

            List<WorldTile> m_allWorldTiles = new List<WorldTile>();

            //set up the distance weight for this tile
            m_world[(int)start.x, (int)start.y].g = 1;
            m_allWorldTiles.Add(m_world[(int)start.x, (int)start.y]);

            //poplate all tiles with a heuristic value
            bool finishedFill = false;
            while (!finishedFill)
            {
                List<WorldTile> newTiles = new List<WorldTile>();
                finishedFill = true;
                for (int i = 0; i < m_allWorldTiles.Count; i++)
                {
                    if (!m_allWorldTiles[i].m_checked)
                    {
                        m_allWorldTiles[i].m_checked = true;
                        finishedFill = false;

                        //pull all of the tile's neighbors into a list
                        List<WorldTile> neighbours = GetAllNeighbours(m_allWorldTiles[i].index);

                        //goes through neighbors and finds the first that has not been given a heuristic value
                        for (int z = 0; z < neighbours.Count; z++)
                        {
                            if (neighbours[z].h == 0f)
                            {
                                neighbours[z].h = Vector2.Distance(neighbours[z].index, end);
                                //neighbours[z].h *= neighbours[z].h;
                                newTiles.Add(neighbours[z]);
                            }
                        }
                    }
                }
                m_allWorldTiles.AddRange(newTiles);
            }

            //Now all world tiles have been given a heuristic and are contained in allworldtiles
            WorldTile currentTile = m_allWorldTiles[0];
            float weightHeuristicTotal = 1f;

            //pathfind until the last processed tile is the goal tile
            while (!(currentTile.index.x == end.x && currentTile.index.y == end.y))
            {
                if (!path.Contains(currentTile))
                {
                    path.Add(currentTile);
                }
                //find all neighbours of the current tile
                List<WorldTile> neighbours = GetAllNeighbours(currentTile.index);

                float lowestVal = float.MaxValue;
                WorldTile lowestTile = neighbours[0];

                //iterate through all neighbors
                for (int i = 0; i < neighbours.Count; i++)
                {
                    if ((neighbours[i].h) + neighbours[i].weight * 3f < lowestVal)
                    {
                        lowestVal = (neighbours[i].h) + neighbours[i].weight * 3f;
                        lowestTile = neighbours[i];
                    }
                }
                weightHeuristicTotal += lowestVal;

                //make the lowest tile the new current tile
                currentTile = lowestTile;
            }

            //RESET TILES for the next path to be planned
            for (int x = 0; x < m_mapWidth; x++)
            {
                for (int y = 0; y < m_mapHeight; y++)
                {
                    m_world[x, y].g = 0;
                    m_world[x, y].m_checked = false;
                }
            }
            path.Add(m_world[(int)end.x, (int)end.y]);
            

            return path;
        }

        /// <summary>
        /// Gets all neighbours.
        /// </summary>
        /// <returns>The all neighbours.</returns>
        /// <param name="pos">Position.</param>
        List<WorldTile> GetAllNeighbours(Vector2 pos)
        {
            List<WorldTile> result = new List<WorldTile>();
            if (pos.x > 0)
            {
                result.Add(m_world[(int)pos.x - 1, (int)pos.y]);
            }
            if (pos.x < m_mapWidth - 1)
            {
                result.Add(m_world[(int)pos.x + 1, (int)pos.y]);
            }

            if (pos.y > 0)
            {
                result.Add(m_world[(int)pos.x, (int)pos.y - 1]);
            }
            if (pos.y < m_mapHeight - 1)
            {
                result.Add(m_world[(int)pos.x, (int)pos.y + 1]);
            }
            return result;
        }

        public void Ripple(int x, int y, int range)
        {
            List<WorldTile> checkedTiles = new List<WorldTile>();
            List<WorldTile> potentialTiles = new List<WorldTile>();

            potentialTiles.Add(m_world[x, y]);
            checkedTiles.Add(m_world[x, y]);
            for (int i = 0; i < range; i++)
            {
                List<WorldTile> potentialTilesThisIteration = new List<WorldTile>();
                for (int k = 0; k < potentialTiles.Count; k++)
                {
                    potentialTilesThisIteration.AddRange(GetAllNeighbours(potentialTiles[k].index));
                }
                potentialTiles.AddRange(potentialTilesThisIteration);
                potentialTiles.AddRange(GetAllNeighbours(new Vector2(x, y)));
                for (int j = 0; j < potentialTiles.Count; j++)
                {
                    if (checkedTiles.Contains(potentialTiles[j])){
                        potentialTiles.RemoveAt(j);
                        j--;
                    }
                    else
                    {
                        checkedTiles.Add(potentialTiles[j]);
                    }
                }
            }
            for (int i = 0; i < checkedTiles.Count; i++)
            {
                float d = 0.15f * Distance(x, y, checkedTiles[i].index.x, checkedTiles[i].index.y);
                StartCoroutine(Bounce(checkedTiles[i], d));
            }
        }

        private float Distance(float x1, float y1, float x2, float y2)
        {
            float d = 0f;
            d = ((x2 - x1) * (x2 - x1)) + ((y2 - y1) * (y2 - y1));
            return Mathf.Sqrt(d);
        }

        private IEnumerator Bounce(WorldTile go, float delay = 0f)
        {
            if (!animatedTiles.Contains(go))
            {
                Transform trans = go.m_worldTileObject.transform;
                animatedTiles.Add(go);
                yield return new WaitForSeconds(delay);
                float duration = 0.5f;
                EaseyTimer timer = new EaseyTimer(duration, true);
                float t = 0f;
                Vector3 startPos = trans.position;
                while (t < duration)
                {
                    trans.position = startPos + new Vector3(0f, Easey.Ease(Easey.EaseType.CircOut, 0f, 0.3f, timer), 0f);
                    t += Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
                t = 0f;
                timer = new EaseyTimer(duration, true);
                while (t < duration)
                {
                    trans.position = startPos + new Vector3(0f, Easey.Ease(Easey.EaseType.CircIn, 0.3f, 0f, timer), 0f);
                    t += Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
                trans.position = startPos;
                animatedTiles.Remove(go);
            }
        }
    }
}
