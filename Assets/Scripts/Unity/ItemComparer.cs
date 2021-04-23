using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemComparer : MonoBehaviour
{
    public Transform Item1Discription;
    public Transform Item1Stats;

    public Transform Item2Discription;
    public Transform Item2Stats;

    public Font font;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetitemsToCompare(Item item1, Item item2)
    {
        SetItem(item1, item1, Item1Discription, Item1Stats);
        SetItem(item2, item1, Item2Discription, Item2Stats);
    }



    private void SetItem(Item item, Item otherItem, Transform ItemDiscription, Transform ItemStats)
    {
        foreach (Transform child in ItemStats)
            Destroy(child.gameObject);
        if (item != null)
            foreach (var stat in item.stats)
            {
                GameObject obj = new GameObject(stat.Key.ToString());
                obj.transform.parent = ItemStats;
                obj.AddComponent<HorizontalLayoutGroup>();
                string statName = stat.Key.ToString();
                statName.Replace('_', ' ');
                AddText(obj, statName, Color.white);
                AddText(obj, ": ", Color.white);
                AddText(obj, stat.Value.ToString(), Color.white);
                int otherItemStat = otherItem?.stats.ContainsKey(stat.Key) ?? false ? otherItem.stats[stat.Key] : 0;
                int delta =  stat.Value - otherItemStat;
                if (delta != 0)
                {
                    
                    AddText(obj, " " + (delta > 0 ? "+" : "") + delta, delta > 0 ? Color.green : (delta < 0 ? Color.red : Color.white));
                }

                ContentSizeFitter fit = obj.AddComponent<ContentSizeFitter>();
                fit.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                fit.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            }

    }

    private void AddText(GameObject parent, string text, Color color)
    {
        GameObject obj = new GameObject();
        obj.transform.parent = parent.transform;
        Text tex = obj.AddComponent<Text>();
        tex.font = font;
        tex.rectTransform.sizeDelta = Vector2.one * 20;
        tex.text = text;
        tex.color = color;
        ContentSizeFitter fit = obj.AddComponent<ContentSizeFitter>();
        fit.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        fit.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }
}
