using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventoried<E>:MonoBehaviour where E:Entity
{
    public Inventory<E> inventory;
}
