using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BoardData
{
	[Serializable]
	public class Row
	{
		public int[] Grids;
		public Row(int size) => Grids = new int[size];
	}

	[SerializeField] Size size;
	public Size Size { get { return size; } }
	[SerializeField] Row[] rows;

	public BoardData() => Resize(new Size(1, 1));
	public BoardData(int width, int height) => Resize(width, height);

	public BoardData Clone()
	{
		BoardData clone = new BoardData(size.Width, size.Height);
		for (int y = 0; y < size.Height; y++) {
			for (int x = 0; x < size.Width; x++) {
				clone.rows[y].Grids[x] = rows[y].Grids[x];
			}
		}
		return clone;
	}

	// サイズ変更
	public void Resize(Size src) => Resize(src.Width, src.Height);
	public void Resize(int width, int height)
	{
		size = new Size(width, height);
		rows = new Row[size.Height];
		for (int i = 0; i < size.Height; i++) {
			rows[i] = new Row(size.Width);
		}
	}

	// 範囲外判定
	public bool IsOutOfRange(Point point) => IsOutOfRange(point.X, point.Y);
	public bool IsOutOfRange(int x, int y)
	{
		if (x < 0 || y < 0) return true;
		if (x >= Size.Width || y >= Size.Height) return true;
		if (y >= rows.Length || x >= rows[y].Grids.Length) return true;
		return false;
	}

	// パネル色
	public int GetPanelColorId(Point point) => GetPanelColorId(point.X, point.Y);
	public int GetPanelColorId(int x, int y)
	{
		if (IsOutOfRange(x, y)) return Define.InvalidPanelColorId;
		return rows[y].Grids[x];
	}
	public void SetPanelColorId(Point point, int panelColorId)
	{
		if (IsOutOfRange(point)) return;
		rows[point.Y].Grids[point.X] = panelColorId;
	}
}
