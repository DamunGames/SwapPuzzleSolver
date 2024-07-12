using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Size
{
	public int Width;
	public int Height;

	public Size(int width, int height)
	{
		Width = width;
		Height = height;
	}

	public Size Clone() => new Size(Width, Height);
}
