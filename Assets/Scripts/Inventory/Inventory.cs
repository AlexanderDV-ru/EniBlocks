using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	public string[] items;
	public int selected;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
	public void addItem(string name)
	{
		for(int i=0;i<items.Length;i++)
			if(EntityId.ByName(items[i]).name=="air")
			{
				items[i]=name;
				break;
			}
	}
}
