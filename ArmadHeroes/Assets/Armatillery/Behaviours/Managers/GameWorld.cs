/***********************************
 * GameWorld.cs
 * Created by Daniel Weston 28/01/16
 * *********************************/
using UnityEngine;
using System.Collections;

public class GameWorld : MonoBehaviour 
{
    private static GameWorld m_instance;
    public static GameWorld instance { get { return m_instance; } }

    public int m_mapWidth = 100, m_mapHeight = 100;//default mapsize
    public float m_tileWidth = 0.32f, m_tileHeight = 0.32f;//default tilesize

    void Awake()
    {
        //init singleton 
        m_instance = this;
    }

}
