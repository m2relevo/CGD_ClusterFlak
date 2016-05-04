using UnityEngine;
using System.Collections;

public class LevelBuild : MonoBehaviour
{
    
    public GameObject floorTile;
    const int mapWidth = 25, mapHeight = 25;
    float tileSize;
    bool negline;

    // Use this for initialization
    void Start()
    {
        
        //mapWidth = 50;
        //mapHeight = 50;
        tileSize = 1.25f;
        int[,] mapArr = new int[mapHeight, mapWidth];

        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                if (i == 0 || j == 0 || i == mapHeight - 1 || j == mapWidth - 1)
                {
                    mapArr[i,j] = 2;
                }

                else
                {
                    mapArr[i, j] = 1;
                }

            }
        }

        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                if (mapArr[i, j] == 1)
                {
                    if (!negline)
                    {
                        Instantiate(floorTile, new Vector3(tileSize * j, tileSize * (i * 0.5f)), Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(floorTile, new Vector3(tileSize * (j * 1.5f), tileSize * i), Quaternion.identity);
                    }
                }
            }
            negline = !negline;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {

    }
}
