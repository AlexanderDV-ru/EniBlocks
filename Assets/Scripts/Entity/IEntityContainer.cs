using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityContainer
{
    Entity set(EntityLocation location, Entity entity=null);
}
