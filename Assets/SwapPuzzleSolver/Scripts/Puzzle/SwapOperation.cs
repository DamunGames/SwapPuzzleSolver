using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 入れ替え操作情報
public class SwapOperation
{
	public Point Point;
	public Dir Dir;
	public SwapOperation(int x, int y, Dir dir)
	{
		Point = new Point(x, y);
		Dir = dir;
	}
}
