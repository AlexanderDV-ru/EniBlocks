using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserController : Controller
{
	public Ids ids;
	public GameObject crosshair;

	bool paused=false;
	bool lastInv;
	// Update is called once per frame
	void FixedUpdate()
	{
		if(Input.GetAxis("Inventory")>0&&!lastInv)
			paused=!paused;
		lastInv=Input.GetAxis("Inventory")>0;
		crosshair.SetActive(!paused);
		Cursor.visible=paused;
		Cursor.lockState=paused?CursorLockMode.None:CursorLockMode.Locked;
		if(!paused)
		{
			movement.AtFixedUpdate();
			placement.AtFixedUpdate();
		}
	}
}
