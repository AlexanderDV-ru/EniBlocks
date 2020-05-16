using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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
	private static EntityId[] idsFromConfig(string path)
	{
		StreamReader reader = new StreamReader(path);
        string[] cfg=reader.ReadToEnd().Replace("\r\n","\n").Replace("\r","\n").Split('\n');
        reader.Close();
		List<EntityId> ids=new List<EntityId>();
		for(int ln=0;ln<cfg.Length;ln++)
			if(cfg[ln].Replace(" ","")!=""&&!cfg[ln].StartsWith("//"))
			{
				var v=JsonUtility.FromJson<EntityId>(cfg[ln]);
				ids.Add(v);
				if(v.name=="air")
					air=v;
			}
		return ids.ToArray();
	}
	public static readonly EntityId[] ids=idsFromConfig("Assets/Properties/ids.json");
	public static EntityId ByName(string name)
	{
		foreach(EntityId id in ids)
			if(id.name==name)
				return id;
		return EntityId.air;
	}

	private static EntityId air		=	new EntityId("air",new Vector2[]{new Vector2(0,15)},new int[]{0,0,0,0,0,0});
}
