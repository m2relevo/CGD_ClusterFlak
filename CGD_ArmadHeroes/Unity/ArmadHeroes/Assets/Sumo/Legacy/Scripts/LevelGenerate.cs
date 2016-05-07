using UnityEngine;
using System.Collections;

public class LevelGenerate : MonoBehaviour {

    public int xPos, yPos, roomWidth, roomHeight;
    public GameObject arenaTile;

    public void setUpRoom()
    {
        roomWidth = 6;
        roomHeight = 6;
    }

	
	// Update is called once per frame
	void Update ()
    {
        for (int x = 0; x <= roomHeight; x++)
        {
            for (int y = 0; y <= roomWidth; y++)
            {

            }
        }
	}
}
