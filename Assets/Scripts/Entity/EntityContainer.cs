using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityContainer<E> : MonoBehaviour where E:Entity
{
    public E EntityIn(EntityLocation loc){return null;}
    public E EntityIn(EntityLocation loc,E newE){return null;}
}
