using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemComparer : MonoBehaviour
{
    public Transform Item1Discription;
    public Transform Item1Stats;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetItem(Item item)
    {
        foreach (var stat in item.stats)
        {
            GameObject obj = new GameObject(stat.Key.ToString());
            obj.transform.parent = Item1Stats;
            Text tex = obj.AddComponent<Text>();
            tex.text = stat.Key + ": " + stat.Value;
        }
        
    }
}
