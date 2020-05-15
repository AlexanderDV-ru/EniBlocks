using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public struct EntityId
{
	public string name;
	public string type;
	public string subtype;
    public Vector2[] textures;
    public int[] faces;
	public bool transparent{get{return subtype[0]=='_';}}
    public EntityId(string name,Vector2[] textures,int[] faces)
    {
		this.name=name;
		this.type="block";
		this.subtype="solid";
        this.textures=textures;
        this.faces=faces;
    }
	public static EntityId[] fromConfig()
	{
		StreamReader reader = new StreamReader("Assets/Properties/ids.json");
        string[] cfg=reader.ReadToEnd().Replace("\r\n","\n").Replace("\r","\n").Split('\n');
        reader.Close();
		List<EntityId> eids=new List<EntityId>();
		for(int ln=0;ln<cfg.Length;ln++)
			if(cfg[ln].Replace(" ","")!=""&&!cfg[ln].StartsWith("//"))
			{
				var v=JsonUtility.FromJson<EntityId>(cfg[ln]);
				eids.Add(v);
				if(v.name=="air")
					air=v;
			}
		return eids.ToArray();
	}
	public static EntityId[] ids=fromConfig();
	public static EntityId ByName(string name)
	{
		foreach(EntityId id in ids)
			if(id.name==name)
				return id;
		return EntityId.air;
	}

	private static EntityId air		=	new EntityId("air",new Vector2[]{new Vector2(0,15)},new int[]{0,0,0,0,0,0});
}
