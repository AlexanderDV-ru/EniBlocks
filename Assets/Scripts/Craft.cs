using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Craft
{
	private static Dictionary<string,List<string>> groupsFromConfig(string path)
	{
		Dictionary<string,List<string>> groups=new Dictionary<string,List<string>>();

		StreamReader reader = new StreamReader(path);
		string[] cfg=reader.ReadToEnd().Replace("\r\n","\n").Replace("\r","\n").Split('\n');
		reader.Close();

		for(int v=0;v<EntityId.ids.Length;v++)
		{
			List<string> list=new List<string>();
			list.Add(EntityId.ids[v].name);
			groups.Add(EntityId.ids[v].name,list);
		}
		return groups;
	}
	public static char spl0=';',spl1=',',spl2='=';
	public static string comment="//";
	private static Dictionary<string[,],string[]> recipesFromConfig(string path)
	{
		Dictionary<string[,],string[]> recipes=new Dictionary<string[,],string[]>();

		StreamReader reader = new StreamReader(path);
		string[] cfg=reader.ReadToEnd().Replace("\r\n","\n").Replace("\r","\n").Split('\n');
		reader.Close();

		Dictionary<string,string[]> replacing=new Dictionary<string,string[]>();

		for(int l=0;l<cfg.Length;l++)
			if(cfg[l].Replace(" ","")!="")
				if(cfg[l].StartsWith(comment))
					replacing.Add(cfg[l].Substring(2).Split(spl2)[0],cfg[l].Substring(2).Split(spl2)[1].Split(spl0));
				else
				{
					List<string> s=new List<string>();
					s.Add(cfg[l]);
					foreach(var v in replacing)
					{
						List<string> strs=new List<string>();
						for(var v1=0;v1<s.Count;v1++)
							for(var v2=0;v2<v.Value.Length;v2++)
								strs.Add(s[v1].Replace(v.Key,v.Value[v2]));
						s=strs;
					}
					for(int c=0;c<s.Count;c++)
					{
						string[] ing=s[c].Split(spl2)[1].Split(spl0);
						List<string[]> recips1=new List<string[]>();
						if(ing.Length==3)
						{
							recips1.Add(new string[]{ing[0],ing[1],ing[2]});
						}
						if(ing.Length==2)
						{
							recips1.Add(new string[]{ing[0],ing[1],""});
							recips1.Add(new string[]{"",ing[0],ing[1]});
						}
						if(ing.Length==1)
						{
							recips1.Add(new string[]{"","",ing[0]});
							recips1.Add(new string[]{"",ing[0],""});
							recips1.Add(new string[]{ing[0],"",""});
						}
						List<string[,]> recips=new List<string[,]>();
						for(int v=0;v<recips1.Count;v++)
						{
							int max=0;
							for(int v2=0;v2<recips1[v].Length;v2++)
								max=max>recips1[v][v2].Split(spl1).Length?max:recips1[v][v2].Split(spl1).Length;
							for(int v2=0;v2<recips1[v].Length;v2++)
								recips1[v][v2]=(recips1[v][v2].Split(spl1).Length>0&&recips1[v][v2].Split(spl1)[0].Replace(" ","")!=""?recips1[v][v2].Split(spl1)[0]:"air")+spl1+(recips1[v][v2].Split(spl1).Length>1&&recips1[v][v2].Split(spl1)[1].Replace(" ","")!=""?recips1[v][v2].Split(spl1)[1]:"air")+spl1+(recips1[v][v2].Split(spl1).Length>2&&recips1[v][v2].Split(spl1)[2].Replace(" ","")!=""?recips1[v][v2].Split(spl1)[2]:"air");
							if(max==3)
							{
								recips.Add(new string[,]
								{
									{recips1[v][0].Split(spl1)[0],recips1[v][0].Split(spl1)[1],recips1[v][0].Split(spl1)[2],},
									{recips1[v][1].Split(spl1)[0],recips1[v][1].Split(spl1)[1],recips1[v][1].Split(spl1)[2],},
									{recips1[v][2].Split(spl1)[0],recips1[v][2].Split(spl1)[1],recips1[v][2].Split(spl1)[2],},
								});
							}
							if(max==2)
							{
								recips.Add(new string[,]
								{
									{recips1[v][0].Split(spl1)[0],recips1[v][0].Split(spl1)[1],"air"},
									{recips1[v][1].Split(spl1)[0],recips1[v][1].Split(spl1)[1],"air"},
									{recips1[v][2].Split(spl1)[0],recips1[v][2].Split(spl1)[1],"air"},
								});
								recips.Add(new string[,]
								{
									{"air",recips1[v][0].Split(spl1)[0],recips1[v][0].Split(spl1)[1]},
									{"air",recips1[v][1].Split(spl1)[0],recips1[v][1].Split(spl1)[1]},
									{"air",recips1[v][2].Split(spl1)[0],recips1[v][2].Split(spl1)[1]},
								});
							}
							if(max==1)
							{
								recips.Add(new string[,]
								{
									{recips1[v][0].Split(spl1)[0],"air","air"},
									{recips1[v][1].Split(spl1)[0],"air","air"},
									{recips1[v][2].Split(spl1)[0],"air","air"},
								});
								recips.Add(new string[,]
								{
									{"air",recips1[v][0].Split(spl1)[0],"air"},
									{"air",recips1[v][1].Split(spl1)[0],"air"},
									{"air",recips1[v][2].Split(spl1)[0],"air"},
								});
								recips.Add(new string[,]
								{
									{"air","air",recips1[v][0].Split(spl1)[0]},
									{"air","air",recips1[v][1].Split(spl1)[0]},
									{"air","air",recips1[v][2].Split(spl1)[0]},
								});
							}
						}

						for(int v=0;v<recips.Count;v++)
							recipes.Add(recips[v],s[c].Split(spl2)[0].Split(spl0));
					}
				}
		return recipes;
	}

	public static readonly Dictionary<string,List<string>> groups=groupsFromConfig("Assets/Properties/groups.cfg");
	public static readonly Dictionary<string[,],string[]> recipes=recipesFromConfig("Assets/Properties/recipes.cfg");

	public static string[] Do(string[,] items)
	{
		foreach(KeyValuePair<string[,],string[]> pair in recipes)
			if(pair.Key.Length==items.Length)
			{
				bool ok=true;
				for(int v=0;v<3;v++)
					for(int v1=0;v1<3;v1++)
					{
						bool ok2=false;
						//Debug.Log(pair.Key.Length+" "+v+" "+v1+" "+pair.Key[v,v1]);
						for(int v2=0;v2<groups[pair.Key[v,v1]].Count;v2++)
							if(EntityId.ByName(groups[pair.Key[v,v1]][v2]).name==EntityId.ByName(items[v,v1]).name)
								ok2=true;
						if(!ok2)
							ok=false;
					}
				if(ok)
					return pair.Value;
			}
		return new string[]{"air"};
	}
}
