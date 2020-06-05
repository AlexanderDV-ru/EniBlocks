using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{
	public World world;
	public Inventory hotbar;
	public Inventory inventory;
	public string name;
	public Permissions perms;
	Entity entity;
	public bool isRunning=false;
	public Vector2 mouseSenvisity=new Vector2(2,2);
	public Vector3 speeds=new Vector3(0.3f,0.3f,0.3f);
	public Vector3 jump=new Vector3(0,2,0);
	public Vector3 runModifiers=new Vector3(0,3,0);
	public Vector3 exMinEulerAngles=new Vector3(0,0,0);
	public Vector3 minEulerAngles=new Vector3(-90,0,0);
	public Vector3 exMaxEulerAngles=new Vector3(0,0,0);
	public Vector3 maxEulerAngles=new Vector3(90,0,0);
	public GameObject head;
    // Start is called before the first frame update
    void Start()
    {
        entity=Entity.create(EntityId.ByName(name));
    }

    // Update is called once per frame
    void Update()
    {

    }
	bool onGround()
	{
		RaycastHit hit;
		return Physics.Raycast(transform.position, -transform.up, out hit, 0.1f);
	}
	float ogr(float min,float cur,float max,bool hasMin,bool hasMax)
	{
		float ogr0=hasMin?(min>cur?min:cur):cur;
		ogr0=hasMax?(max<cur?max:cur):cur;
		return ogr0;
	}
	float ogr(float min,float cur,float max)
	{
		return ogr(min,cur,max,true,true);
	}
	Vector3 rel(Vector3 v)
	{
		return transform.right*v.x+transform.up*v.y+transform.forward*v.z;
	}
	public void MovementFU(Vector3 moveControl, float jumpControl, Vector3 rotateControl)
	{
		Vector3 nev=new Vector3();
		nev.x+= speeds.x*(perms.canMoveX?1:0)*moveControl.x			*(isRunning?runModifiers.x:1);
		nev.y+= speeds.y*(perms.canMoveY?1:0)*moveControl.y			*(isRunning?runModifiers.y:1);
		nev.z+= speeds.z*(perms.canMoveZ?1:0)*moveControl.z			*(isRunning?runModifiers.z:1);
		nev	+=	jump	*(perms.canJump	?1:0)*(jumpControl>0?1:0)	*(onGround()?1:0);
		Debug.Log(onGround());

		Vector3 movement=rel(nev);
		GetComponent<Rigidbody>().velocity+=movement;

		head.transform.Rotate(-rotateControl.y	*	mouseSenvisity.y*(perms.canRotateX?1:0),0,0);
		transform.Rotate(0,	rotateControl.x	*	mouseSenvisity.x*(perms.canRotateY?1:0),	0);
		head.transform.Rotate(0,	0,rotateControl.z*	mouseSenvisity.y*(perms.canRotateZ?1:0));


		//TODO transform.position=chunks.normalize(transform.position);
		float x=ogr(minEulerAngles.x,head.transform.eulerAngles.x,maxEulerAngles.x,exMinEulerAngles.x!=0,exMaxEulerAngles.x!=0);
		float y=ogr(minEulerAngles.y,head.transform.eulerAngles.y,maxEulerAngles.y,exMinEulerAngles.y!=0,exMaxEulerAngles.y!=0);
		float z=ogr(minEulerAngles.z,head.transform.eulerAngles.z,maxEulerAngles.z,exMinEulerAngles.z!=0,exMaxEulerAngles.z!=0);
		head.transform.eulerAngles=new Vector3(x,y,z);
	}
	public string gamemode="creative";
	public string touchmode="destroy",minemode="momental";
	public float creativeCounter=0,creativeTimeout=0.4f;
	public bool haveInteract=false;
	public float editRange=10;
	public Vector3 breakSelector, placeSelector;
	public float breakSpeed=1;
	public void InteractFU(float breakControl,float placeControl,float cloneControl, Ray ray)
	{
		if(gamemode=="creative"||true)
		{
			creativeCounter++;
			if(creativeCounter>creativeTimeout/Time.fixedDeltaTime)
			{
				creativeCounter=0;
				haveInteract=true;
			}
		}
		RaycastHit hit;
		Physics.Raycast(ray.origin, ray.direction, out hit, editRange);
		breakSelector=hit.point+ray.direction*0.1f;
		placeSelector=hit.point-ray.direction*0.1f;
		breakSelector=new Vector3((int)breakSelector.x,(int)breakSelector.y,(int)breakSelector.z);
		placeSelector=new Vector3((int)placeSelector.x,(int)placeSelector.y,(int)placeSelector.z);
		if(haveInteract)
		{
			if(breakControl>0&&perms.canBreak)
			{
				Entity block=world.SetBlock(new EntityLocation((int)breakSelector.x,(int)breakSelector.y,(int)breakSelector.z));
				block.hits-=minemode=="momental"?block.hits:Time.fixedDeltaTime*breakSpeed;
				if(block.hits<=0)
				{
					if(touchmode=="silk")
						hotbar.addItem(block.id.name);
					if(touchmode=="mine")
						hotbar.addItem(block.id.drop);
					world.SetBlock(new EntityLocation((int)breakSelector.x,(int)breakSelector.y,(int)breakSelector.z),Entity.create(EntityId.ByName("air")));
					haveInteract=false;
				}
			}
			if(placeControl>0&&perms.canPlace)
			{
				world.SetBlock(new EntityLocation((int)placeSelector.x,(int)placeSelector.y,(int)placeSelector.z),Entity.create(EntityId.ByName(hotbar.items[hotbar.selected])));
				haveInteract=false;
			}
			if(cloneControl>0&&perms.canClone)
			{
				hotbar.items[hotbar.selected]=world.SetBlock(new EntityLocation((int)breakSelector.x,(int)breakSelector.y,(int)breakSelector.z)).id.name;
				haveInteract=false;
			}
		}
	}
}
