using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : EntityContainer<Block>
{
	//Options
	public int width=12,height=12,depth=12;
	public Chunks chunks;
	//World
	Block[,,] allBlocks;
	bool[,,,] allFaces;
	Vector3[] allVertices;
	Vector2[] allUvs;
	int[] allTriangles;

	public Block SetBlock(Location loc,Block block)
	{
		if(!testOnIn(loc))
			return null;
		Block last=allBlocks[loc.x,loc.y,loc.z];
		if(block!=null)
			allBlocks[loc.x,loc.y,loc.z]=block;
		return last;
	}
	public Block GetBlock(Location loc)
	{
		return SetBlock(loc,null);
	}
	public bool testOnIn(Location loc)
	{
		if(loc.x<0||loc.x>=width)
			return false;
		if(loc.y<0||loc.y>=height)
			return false;
		if(loc.z<0||loc.z>=depth)
			return false;
		return true;
	}

	// Start is called before the first frame update
	void Start()
	{
		//mirajes=new MeshFilter[8];
		Vector3[] offsets=new Vector3[]
		{
			new Vector3(-1*chunks.chunksWidth*chunks.chunkWidth,0,-1*chunks.chunksHeight*chunks.chunkHeight),
			new Vector3(-1*chunks.chunksWidth*chunks.chunkWidth,0, 0*chunks.chunksHeight*chunks.chunkHeight),
			new Vector3(-1*chunks.chunksWidth*chunks.chunkWidth,0,+1*chunks.chunksHeight*chunks.chunkHeight),
			new Vector3( 0*chunks.chunksWidth*chunks.chunkWidth,0,-1*chunks.chunksHeight*chunks.chunkHeight),

			new Vector3( 0*chunks.chunksWidth*chunks.chunkWidth,0,+1*chunks.chunksHeight*chunks.chunkHeight),
			new Vector3(+1*chunks.chunksWidth*chunks.chunkWidth,0,-1*chunks.chunksHeight*chunks.chunkHeight),
			new Vector3(+1*chunks.chunksWidth*chunks.chunkWidth,0, 0*chunks.chunksHeight*chunks.chunkHeight),
			new Vector3(+1*chunks.chunksWidth*chunks.chunkWidth,0,+1*chunks.chunksHeight*chunks.chunkHeight),
		};
		//for(int v=0;v<mirajes.Length;v++)
		//{
		//	GameObject go=new GameObject();
		//	go.transform.parent=transform;
		//	go.name="Miraje "+offsets[v];
		//	go.transform.localPosition=offsets[v];
		//	mirajes[v]=go.AddComponent<MeshFilter>();
		//	go.AddComponent<MeshRenderer>();
		//	go.GetComponent<MeshRenderer>().material=GetComponent<MeshRenderer>().material;
		//}
		BlocksGen();
		MeshUpdate(true);
	}
	public string[] defBlock;
	public void BlocksGen()
	{
		allBlocks=new Block[width,height,depth];
		allFaces=new bool[width,height,depth,BlockClass.facesCount];
		allVertices=new Vector3[width*height*depth*BlockClass.facesCount*BlockClass.verticesInFace];
		allUvs=new Vector2[width*height*depth*BlockClass.facesCount*BlockClass.verticesInFace];
		allTriangles=new int[width*height*depth*BlockClass.facesCount*BlockClass.indexesInFace];

		//Blocks set
		for(int x=0;x<width;x++)
			for(int y=0;y<height;y++)
				for(int z=0;z<depth;z++)
				{
					Block block=Block.create(defBlock[y%defBlock.Length]);
					SetBlock(new Location(x,y,z),block);
					GameObject go=new GameObject();
					go.transform.parent=transform;
					if(y==1)
						block.inventory=go.AddComponent<Inventory<Entity>>();
				}
	}
	public void FaceRecalc(Location loc,int f)
	{
		bool invisibleBlock=GetBlock(loc)==null||GetBlock(loc).type.textures.Length==0;
		Block faceBlock=GetBlock(new Location(loc.x+BlockClass.directionsOfFaces[f,0],loc.y+BlockClass.directionsOfFaces[f,1],loc.z+BlockClass.directionsOfFaces[f,2]));
		bool canBeSeen=faceBlock==null?true:faceBlock.type.transparent;
		bool visible=!invisibleBlock&&canBeSeen;

		int pos=((loc.x*height+loc.y)*depth+loc.z)*BlockClass.facesCount+f;
		for(int v=0;v<BlockClass.verticesInFace;v++)
		{
			allVertices[pos*BlockClass.verticesInFace+v]=visible?new Vector3(loc.x,loc.y,loc.z)+BlockClass.vertices[BlockClass.verticesIndexesOfFaces[f,v]]:new Vector3(0,0,0);
			if(visible)
				allUvs[pos*BlockClass.verticesInFace+v]=new Vector2(GetBlock(loc).type.textures[f,v].x/Ids.ins.xTexturesCount,GetBlock(loc).type.textures[f,v].y/Ids.ins.yTexturesCount);

			if(v==3)
			{
				allTriangles[pos*BlockClass.indexesInFace+3]=pos*BlockClass.verticesInFace+0;
				allTriangles[pos*BlockClass.indexesInFace+4]=pos*BlockClass.verticesInFace+2;
				allTriangles[pos*BlockClass.indexesInFace+5]=pos*BlockClass.verticesInFace+3;
			}
			else allTriangles[pos*BlockClass.indexesInFace+v]=pos*BlockClass.verticesInFace+v;
		}
	}
	public void BlockRecalc(Location loc)
	{
		if(testOnIn(loc))
			for(int f=0;f<BlockClass.facesCount;f++)
				FaceRecalc(loc,f);
	}

	public void NearBlockRecalc(Location loc)
	{
		BlockRecalc(new Location(loc.x-1,loc.y,loc.z));
		BlockRecalc(new Location(loc.x+1,loc.y,loc.z));
		BlockRecalc(new Location(loc.x,loc.y-1,loc.z));
		BlockRecalc(new Location(loc.x,loc.y+1,loc.z));
		BlockRecalc(new Location(loc.x,loc.y,loc.z-1));
		BlockRecalc(new Location(loc.x,loc.y,loc.z+1));
		BlockRecalc(new Location(loc.x,loc.y,loc.z));
	}
	public void MeshRecalc()
	{
		for(int x=0;x<width;x++)
			for(int y=0;y<height;y++)
				for(int z=0;z<depth;z++)
					for(int f=0;f<BlockClass.facesCount;f++)
						FaceRecalc(new Location(x,y,z),f);
	}
	MeshFilter[] mirajes;
	public void MeshUpdate(bool recalc)
	{
		if(recalc)
			MeshRecalc();

		Mesh mesh=GetComponent<MeshFilter>().mesh;
		mesh.vertices=allVertices;
		mesh.triangles=allTriangles;
		mesh.uv=allUvs;
		mesh.RecalculateNormals();
		GetComponent<MeshCollider>().sharedMesh=null;
		GetComponent<MeshCollider>().sharedMesh=mesh;
		//for(int v=0;v<mirajes.Length;v++)
		//	mirajes[v].mesh=mesh;
	}
	public bool blocksChanged=false;
	public string changeCmd;
	void OnCollisionEnter(Collision collision)
	{
		if(collision.transform.GetComponent<Alchemical>())
			foreach (ContactPoint contact in collision.contacts)
			{
				Vector3 point=contact.point-transform.position;
				int x=(int)point.x,y=(int)point.y,z=(int)point.z;
				x=x<0?0:(x>=width?width-1:x);
				y=y<0?0:(y>=height?height-1:y);
				z=z<0?0:(z>=depth?depth-1:z);
				SetBlock(new Location(x,y,z),Block.create(collision.transform.GetComponent<Alchemical>().id));
				NearBlockRecalc(new Location(x,y,z));
			}
		doUpd=true;
	}
	public bool doUpd=false;
	public void MakeBlocksChange(string cmd)
	{
		string[] args=cmd.Split(' ');
		string xcs=args[0],ycs=args[1],zcs=args[2];
		string ids=args[3];
		string xls=args[4],yls=args[5],zls=args[6];
		string xos=args.Length>=8?args[7]:"0",yos=args.Length>=9?args[8]:"0",zos=args.Length>=10?args[9]:"0";
		BlockClass type=BlockClass.ById(ids);
		if(type==null)
		{
			Debug.Log(ids+" is not in ids");
			return;
		}
		Debug.Log(ids);
		int xc=int.Parse(xcs),yc=int.Parse(ycs),zc=int.Parse(zcs);
		int xl=int.Parse(xls),yl=int.Parse(yls),zl=int.Parse(zls);
		int xo=int.Parse(xos),yo=int.Parse(yos),zo=int.Parse(zos);
		for(int x=xc+xo;x<xl+xo;x++)
		for(int y=yc+yo;y<yl+yo;y++)
		for(int z=zc+zo;z<zl+zo;z++)
			Debug.Log(SetBlock(new Location(x,y,z),Block.create(ids)));
	}
	// Update is called once per frame
	void Update()
	{
		if(blocksChanged)
		{
			MakeBlocksChange(changeCmd);
			MeshUpdate(true);
			blocksChanged=false;
		}
		if(doUpd)
		{
			MeshUpdate(false);
			doUpd=false;
		}
	}
}
