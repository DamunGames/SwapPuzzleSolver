using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point
{
	public int X;
	public int Y;

	public Point(int x, int y)
	{
		X = x;
		Y = y;
	}

	public Point Clone() => new Point(X, Y);
}
