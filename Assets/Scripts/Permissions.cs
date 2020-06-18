using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Permissions
{
	public bool _moveX = true, _moveY = false, _moveZ = true;
	public bool _jump = true;
	public bool _rotateX = true, _rotateY = true, _rotateZ = false;
	public bool _break = true, _place = true, _clone = false;
	public bool _interact = true;

	public bool _fromNothing = true, _intoNothing = true;

	public bool canMoveX { get => _moveX; }
	public bool canMoveY { get => _moveY; }
	public bool canMoveZ { get => _moveZ; }

	public bool canJump { get => _jump; }

	public bool canRotateX { get => _rotateX; }
	public bool canRotateY { get => _rotateY; }
	public bool canRotateZ { get => _rotateZ; }

	public bool canBreak { get => _break; }
	public bool canPlace { get => _place; }
	public bool canClone { get => _clone; }

	public bool canInteract { get => _interact; }

	public bool canFromNothing { get => _fromNothing; }
	public bool canIntoNothing { get => _intoNothing; }
}
