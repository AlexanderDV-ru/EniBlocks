using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
class EntityIds
{
	public EntityId[] ids;
}
[System.Serializable]
public struct EntityId
{
	public string name;
	public string type;
	public string subtype;
	public Vector2[] textures;
	public string[] props;
	public Vector3[] verts;
	public int[,] tris;
	public Vector2[] uvs;
	public int[] faces;
	public bool transparent { get => hasProp("transparent"); }
	public bool burning { get => hasProp("burning"); }
	public string _drop;
	public string drop { get => _drop != null ? _drop : name; }
	public float hits;
	public bool hasProp(string name)
	{
		if (props == null)
			return false;
		for (int v = 0; v < props.Length; v++)
			if (props[v] == name)
				return true;
		return false;
	}
	public EntityId(string name, Vector2[] textures, int[] faces, Vector3[] verts = null, int[,] tris = null, Vector2[] uvs = null, string[] props = null, string type = "block", string subtype = "solid", string _drop = null, float hits = 100)
	{
		this.verts = verts;
		this.tris = tris;
		this.uvs = uvs;
		this.props = props == null ? new string[] { } : props;
		this.name = name;
		this.type = type;
		this.subtype = subtype;
		this.textures = textures;
		this.faces = faces;
		this._drop = _drop == null ? name : _drop;
		this.hits = hits;
		Debug.Log(name + " " + _drop + " " + this._drop);
	}
	private static EntityId[] idsFromConfig(string path)
	{
		/*StreamReader reader = new StreamReader(path);
		string[] cfg = reader.ReadToEnd().Replace("\r\n", "\n").Replace("\r", "\n").Split('\n');
		reader.Close();
		List<EntityId> ids = new List<EntityId>();
		for (int ln = 0; ln < cfg.Length; ln++)
			if (cfg[ln].Split('#')[0].Replace(" ", "") != "")
			{
				var v = JsonUtility.FromJson<EntityId>(cfg[ln].Split('#')[0]);
				ids.Add(v);
				if (v.name == "air")
					air = v;
			}
		return ids.ToArray();*/
		StreamReader reader = new StreamReader(path);
		string cfg = reader.ReadToEnd();
		reader.Close();
		//Debug.Log(JsonUtility.FromJson<Dictionary<string, List<EntityId>>>(cfg).Keys.ToArray());
		return JsonUtility.FromJson<EntityIds>(cfg).ids;
	}
	public static readonly EntityId[] ids = idsFromConfig("Assets/Properties/ids.json");
	public static EntityId ByName(string name)
	{
		foreach (EntityId id in ids)
			if (id.name == name)
				return id;
		return EntityId.air;
	}

	private static EntityId air = new EntityId("air", new Vector2[] { new Vector2(0, 15) }, new int[] { 0, 0, 0, 0, 0, 0 });
}
