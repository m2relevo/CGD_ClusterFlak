#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;


namespace DilloDash
{
    public class EditorPrefabPlacerDD : MonoBehaviour
    {
        [SerializeField] private bool isActive = false;
        [SerializeField] private GameObject[] prefabs;
        [SerializeField] private Camera cam;
        [SerializeField] private Vector2 offset;
        [SerializeField] bool isTile;
        int x = 100;
        int y = 100;
        // Use this for initialization
        void Start()
        {
            if(!isActive)
            {
                GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (isActive)
            {
                Vector2 pos = ExtensionMethods.toIso(new Vector2(x, y));
                transform.position = new Vector3(pos.x, pos.y, 0);
                cam.transform.position = new Vector3(pos.x, pos.y, -10);
                if (Input.GetKeyDown(KeyCode.W))
                {
                    y += 7;
                }
                if (Input.GetKeyDown(KeyCode.A))
                {
                    x -= 7;
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    y -= 7;
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    x += 7;
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Vector3 pos2 = ExtensionMethods.toIso(new Vector2(x, y));
                    pos2.x = pos2.x + offset.x;
                    pos2.y = pos2.y + offset.y;
                    // Instantiate(prefab, pos2, Quaternion.identity);

                    GameObject instantiateOBJ = prefabs.Length > 1 ? prefabs[Random.Range(0, prefabs.Length)] : prefabs[0];

                    GameObject obj = PrefabUtility.InstantiatePrefab(instantiateOBJ) as GameObject;
                    obj.transform.position = pos2;

                    if(isTile)
                        obj.transform.localScale *= 7;
                }
            }
        }
    }
}
#endif