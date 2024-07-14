using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dir
{
	public enum DirType
	{
		None,
		Up,
		Down,
		Left,
		Right,
	}

	public int X;
	public int Y;
	public Dir(DirType dirType) => SetDirType(dirType);
	public Dir Clone() => new Dir(ToDirType());

	public void SetDirType(DirType dirType)
	{
		switch (dirType) {
			case DirType.Up:
				X = 0;
				Y = 1;
				break;
			case DirType.Down:
				X = 0;
				Y = -1;
				break;
			case DirType.Left:
				X = -1;
				Y = 0;
				break;
			case DirType.Right:
				X = 1;
				Y = 0;
				break;
			case DirType.None:
			default:
				X = 0;
				Y = 0;
				break;
		}
	}

	public DirType ToDirType()
	{
		if (X == 0 && Y == 1) {
			return DirType.Up;
		}
		else if (X == 0 && Y == -1) {
			return DirType.Down;
		}
		else if (X == -1 && Y == 0) {
			return DirType.Left;
		}
		else if (X == 1 && Y == 0) {
			return DirType.Right;
		}
		return DirType.None;
	}
}