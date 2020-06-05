using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Permissions : MonoBehaviour
{
    public bool _moveX=true,_moveY=false,_moveZ=true;
    public bool _jump=true;
    public bool _rotateX=true,_rotateY=true,_rotateZ=false;
    public bool _break=true,_place=true,_clone=false;

	public bool canMoveX{get=>_moveX;}
	public bool canMoveY{get=>_moveY;}
	public bool canMoveZ{get=>_moveZ;}

	public bool canJump{get=>_jump;}

	public bool canRotateX{get=>_rotateX;}
	public bool canRotateY{get=>_rotateY;}
	public bool canRotateZ{get=>_rotateZ;}

	public bool canBreak{get=>_break;}
	public bool canPlace{get=>_place;}
	public bool canClone{get=>_clone;}
}
