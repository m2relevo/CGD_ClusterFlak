using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Pools any given object
public class ParticlePooler : MonoBehaviour
{
    public List<ParticleSystem> itemList;
    public ParticleSystem item;
    public int size = 10;

    public int head = 0;

    void Start()
    {
        itemList = new List<ParticleSystem>();
        for (int i = 0; i < size; i++)
        {
            itemList.Add(Instantiate(item));
            itemList[i].transform.parent = this.transform;
        }
    }

    public ParticleSystem GetNext()
    {
        if (head == itemList.Count - 1)
        {
            head = 0;
            return itemList[head];
        }
        return itemList[head++];
    }
}
