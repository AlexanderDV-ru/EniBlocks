using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory<E> : EntityContainer<E> where E:Entity
{
    public E[] items;
    public int selectedSlot=0;
    // Start is called before the first frame update
    void Start()
    {
        if(items==null)
            init();
    }
    public void init()
    {
        items=new E[]{};
    }

    // Update is called once per frame
    void Update()
    {

    }
}
