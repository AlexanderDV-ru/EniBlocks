using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    public Rigidbody rigidbody;
    public Controller controller;
    public Vector2 mouseSenvisity=new Vector2(2,2);
    public Vector3 speeds=new Vector3(0.3f,0.3f,0.3f);
    public Vector3 jump=new Vector3(0,2,0);
    public Vector3 runModifiers=new Vector3(0,3,0);
    public Vector3 exMinEulerAngles=new Vector3(0,0,0);
    public Vector3 minEulerAngles=new Vector3(-90,0,0);
    public Vector3 exMaxEulerAngles=new Vector3(0,0,0);
    public Vector3 maxEulerAngles=new Vector3(90,0,0);

    public Camera cam;
	public bool isRunning=false;
    void Start()
    {
		cam.farClipPlane=controller.chunks.chunksWidth*controller.chunks.chunkWidth-4;
    }
	bool onGround()
	{
		RaycastHit hit;
		return Physics.Raycast(transform.position, -transform.up, out hit, 0.1f);
	}
	public void AtFixedUpdate()
	{
		Vector3 nev=new Vector3();
		nev.x+= speeds.x*(controller.perms.moveX?1:0)*Input.GetAxis("Horizontal")*(	isRunning?runModifiers.x:1);
		nev.y+= speeds.y*(controller.perms.moveY?1:0)*Input.GetAxis("Fly")	*	(	isRunning?runModifiers.y:1);
		nev.z+= speeds.z*(controller.perms.moveZ?1:0)*Input.GetAxis("Vertical")*(	isRunning?runModifiers.z:1);
		nev	+=	jump	*(controller.perms.jump	?1:0)*(Input.GetAxis("Jump")>0?1:0)*(onGround()?1:0);

		Vector3 movement=rel(nev);
		rigidbody.velocity+=movement;

		cam.transform.Rotate(-Input.GetAxis("Mouse Y")	*	mouseSenvisity.y*(controller.perms.rotateX?1:0),0,0);
		transform.Rotate(0,	Input.GetAxis("Mouse X")	*	mouseSenvisity.x*(controller.perms.rotateY?1:0),	0);
		cam.transform.Rotate(0,	0,Input.GetAxis("Mouse Z")*	mouseSenvisity.y*(controller.perms.rotateZ?1:0));


		transform.position=controller.chunks.normalize(transform.position);
		float x=ogr(minEulerAngles.x,cam.transform.eulerAngles.x,maxEulerAngles.x,exMinEulerAngles.x!=0,exMaxEulerAngles.x!=0);
		float y=ogr(minEulerAngles.y,cam.transform.eulerAngles.y,maxEulerAngles.y,exMinEulerAngles.y!=0,exMaxEulerAngles.y!=0);
		float z=ogr(minEulerAngles.z,cam.transform.eulerAngles.z,maxEulerAngles.z,exMinEulerAngles.z!=0,exMaxEulerAngles.z!=0);
		cam.transform.eulerAngles=new Vector3(x,y,z);
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
}
