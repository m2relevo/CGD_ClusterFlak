using UnityEngine;
using System.Collections;

public class SpriteSetter : MonoBehaviour {

    public Sprite horizontalFenceSprite;
    public Sprite verticalFenceSprite;
    public Sprite diagonalHighLowFenceSprite;
    public Sprite diagonalLowHighFenceSprite;
    public Sprite lookoutSprite;

    private GameObject[] horizontalFences;
    private GameObject[] verticalFences;
    private GameObject[] highLowFences;
    private GameObject[] lowHighFences;
    private GameObject[] lookouts;

    void Start()
    {
        FillGameObjectArrays();
        SetTheSprites();
	}

    void FillGameObjectArrays()
    {
        if (horizontalFences == null)
        {
            horizontalFences = GameObject.FindGameObjectsWithTag("ShellShock/HORIZONTALFENCE");
        }
        if (verticalFences == null)
        {
            verticalFences = GameObject.FindGameObjectsWithTag("ShellShock/VERTICALFENCE");
        }
        if (highLowFences == null)
        {
            highLowFences = GameObject.FindGameObjectsWithTag("ShellShock/HIGHLOWFENCE");
        }
        if (lowHighFences == null)
        {
            lowHighFences = GameObject.FindGameObjectsWithTag("ShellShock/LOWHIGHFENCE");
        }
        if (lookouts == null)
        {
            lookouts = GameObject.FindGameObjectsWithTag("ShellShock/LOOKOUT");
        }
    }

    void SetTheSprites()
    {
        foreach (GameObject fence in horizontalFences)
        {
            fence.GetComponent<SpriteRenderer>().sprite = horizontalFenceSprite;
        }
        foreach (GameObject fence in verticalFences)
        {
            fence.GetComponent<SpriteRenderer>().sprite = verticalFenceSprite;
        }
        foreach (GameObject fence in highLowFences)
        {
            fence.GetComponent<SpriteRenderer>().sprite = diagonalHighLowFenceSprite;
        }
        foreach (GameObject fence in lowHighFences)
        {
            fence.GetComponent<SpriteRenderer>().sprite = diagonalLowHighFenceSprite;
        }
        foreach (GameObject lookout in lookouts)
        {
            lookout.GetComponent<SpriteRenderer>().sprite = lookoutSprite;
        }
    }
	

}
