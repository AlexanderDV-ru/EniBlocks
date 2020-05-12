using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : Entity
{
    public MobClass type;

    public static Mob create(MobClass type)
    {
        Mob e=Entity.create<Mob>(EntityType.Item);
        e.type=type!=null?type:MobClass.ById("air");
        return e;
    }
    public static Mob create(string id)
    {
        return Mob.create(MobClass.ById(id));
    }
}
