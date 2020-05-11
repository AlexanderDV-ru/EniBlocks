using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
	//Options
	public int w=12,h=12,d=12;
	//World
	Block[,,] allBlocks;
	bool[,,,] allFaces;
	Vector3[] allVertices;
	Vector2[] allUvs;
	int[] allTriangles;

	public Block SetBlock(int x,int y,int z,Block block)
	{
		if(!testOnIn(x,y,z))
			return null;
		Block last=allBlocks[x,y,z];
		allBlocks[x,y,z]=block;
		return last;
	}
	public Block GetBlock(int x,int y,int z)
	{
		if(!testOnIn(x,y,z))
			return null;
		return allBlocks[x,y,z];
	}
	public bool testOnIn(int x,int y,int z)
	{
		if(x<0||x>=w)
			return false;
		if(y<0||y>=h)
			return false;
		if(z<0||z>=d)
			return false;
		return true;
	}

	// Start is called before the first frame update
	void Start()
	{
		BlocksGen();
		MeshUpdate(true);
	}
	public string[] defBlock;
	public void BlocksGen()
	{
		allBlocks=new Block[w,h,d];
		allFaces=new bool[w,h,d,BlockType.facesCount];
		allVertices=new Vector3[w*h*d*BlockType.facesCount*BlockType.verticesInFace];
		allUvs=new Vector2[w*h*d*BlockType.facesCount*BlockType.verticesInFace];
		allTriangles=new int[w*h*d*BlockType.facesCount*BlockType.indexesInFace];

		//Blocks set
		for(int x=0;x<w;x++)
			for(int y=0;y<h;y++)
				for(int z=0;z<d;z++)
				{
					Block block=Block.create(defBlock[y%defBlock.Length]);
					SetBlock(x,y,z,block);
					GameObject go=new GameObject();
					go.transform.parent=transform;
					if(y==1)
						block.inventory=go.AddComponent<Inventory>();
				}
	}
	public void FaceRecalc(int x,int y,int z,int f)
	{
		bool invisibleBlock=GetBlock(x,y,z)==null||GetBlock(x,y,z).type.textures.Length==0;
		Block faceBlock=GetBlock(x+BlockType.directionsOfFaces[f,0],y+BlockType.directionsOfFaces[f,1],z+BlockType.directionsOfFaces[f,2]);
		bool canBeSeen=faceBlock==null?true:faceBlock.type.transparent;
		bool visible=!invisibleBlock&&canBeSeen;

		int pos=((x*h+y)*d+z)*BlockType.facesCount+f;
		for(int v=0;v<BlockType.verticesInFace;v++)
		{
			allVertices[pos*BlockType.verticesInFace+v]=visible?new Vector3(x,y,z)+BlockType.vertices[BlockType.verticesIndexesOfFaces[f,v]]:new Vector3(0,0,0);
			if(visible)
				allUvs[pos*BlockType.verticesInFace+v]=new Vector2(GetBlock(x,y,z).type.textures[f,v].x/Ids.ins.xTexturesCount,GetBlock(x,y,z).type.textures[f,v].y/Ids.ins.yTexturesCount);

			if(v==3)
			{
				allTriangles[pos*BlockType.indexesInFace+3]=pos*BlockType.verticesInFace+0;
				allTriangles[pos*BlockType.indexesInFace+4]=pos*BlockType.verticesInFace+2;
				allTriangles[pos*BlockType.indexesInFace+5]=pos*BlockType.verticesInFace+3;
			}
			else allTriangles[pos*BlockType.indexesInFace+v]=pos*BlockType.verticesInFace+v;
		}
	}
	public void BlockRecalc(int x,int y,int z)
	{
		for(int f=0;f<BlockType.facesCount;f++)
			FaceRecalc(x,y,z,f);
	}

	public void NearBlockRecalc(int x,int y,int z)
	{
		if(testOnIn(x-1,y,z)) BlockRecalc(x-1,y,z);
		if(testOnIn(x+1,y,z)) BlockRecalc(x+1,y,z);
		if(testOnIn(x,y-1,z)) BlockRecalc(x,y-1,z);
		if(testOnIn(x,y+1,z)) BlockRecalc(x,y+1,z);
		if(testOnIn(x,y,z-1)) BlockRecalc(x,y,z-1);
		if(testOnIn(x,y,z+1)) BlockRecalc(x,y,z+1);
		if(testOnIn(x,y,z)) BlockRecalc(x,y,z);
	}
	public void MeshRecalc()
	{
		for(int x=0;x<w;x++)
			for(int y=0;y<h;y++)
				for(int z=0;z<d;z++)
					for(int f=0;f<BlockType.facesCount;f++)
						FaceRecalc(x,y,z,f);
	}
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
				x=x<0?0:(x>=w?w-1:x);
				y=y<0?0:(y>=h?h-1:y);
				z=z<0?0:(z>=d?d-1:z);
				SetBlock(x,y,z,Block.create(collision.transform.GetComponent<Alchemical>().id));
				NearBlockRecalc(x,y,z);
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
		BlockType type=(BlockType)ItemType.ById(ids);
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
			Debug.Log(SetBlock(x,y,z,Block.create(ids)));
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
