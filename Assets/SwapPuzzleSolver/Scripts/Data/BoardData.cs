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
	public void SetPanelColorId(Point point, int panelColorId) => SetPanelColorId(point.X, point.Y, panelColorId);
	public void SetPanelColorId(int x, int y, int panelColorId)
	{
		if (IsOutOfRange(x, y)) return;
		rows[y].Grids[x] = panelColorId;
	}

	// 入れ替え操作可能判定
	public bool IsSwappable(int x, int y, Dir dir)
	{
		// 入れ替え操作可能色判定
		if (!IsSwappableColor(x, y) || !IsSwappableColor(x + dir.X, y + dir.Y)) return false;

		// 一時的に入れ替え操作実行
		if (!SwapDir(x, y, dir)) return false;

		// 揃った判定
		bool isSwappable = IsClearTargetPoint(x, y);

		// 一時的な入れ替え操作を取り消す
		SwapDir(x, y, dir);

		return isSwappable;
	}

	// 入れ替え操作実行
	public bool SwapDir(int x, int y, Dir dir)
	{
		if (IsOutOfRange(x, y)) return false;
		if (IsOutOfRange(x + dir.X, y + dir.Y)) return false;

		int tempColorId = GetPanelColorId(x, y);
		SetPanelColorId(x, y, GetPanelColorId(x + dir.X, y + dir.Y));
		SetPanelColorId(x + dir.X, y + dir.Y, tempColorId);
		return true;
	}

	// 揃ったパネルを削除
	public bool ClearCollectedPanels()
	{
		BoardData preClearBoardData = Clone();

		int clearCount = 0;
		for (int y = 0; y < preClearBoardData.Size.Height; y++) {
			for (int x = 0; x < preClearBoardData.Size.Width; x++) {
				if (preClearBoardData.IsClearTargetPoint(x, y)) {
					clearCount++;
					SetPanelColorId(x, y, Define.EmptyPanelColorId);
				}
			}
		}

		return clearCount > 0;
	}

	// 空に浮いたパネルを落下
	public bool FallPanels()
	{
		Dir upDir = new Dir(Dir.DirType.Up);
		int sumFallCount = 0;
		for (int i = 0; i < Size.Height - 1; i++) {
			int fallCount = 0;
			for (int y = 0; y < Size.Height - 1; y++) {
				for (int x = 0; x < Size.Width; x++) {
					int baseColorId = GetPanelColorId(x, y);
					if (baseColorId != Define.EmptyPanelColorId) continue;

					int upColorId = GetPanelColorId(x + upDir.X, y + upDir.Y);
					if (upColorId == Define.InvalidPanelColorId || upColorId == Define.EmptyPanelColorId || upColorId == Define.BlockPanelColorId) continue;

					fallCount++;
					SetPanelColorId(x, y, upColorId);
					SetPanelColorId(x + upDir.X, y + upDir.Y, Define.EmptyPanelColorId);
				}
			}
			if (fallCount <= 0) break;
			else sumFallCount += fallCount;
		}
		return sumFallCount > 0;
	}

	// 全クリア判定
	public bool IsAllCleared()
	{
		for (int y = 0; y < Size.Height; y++) {
			for (int x = 0; x < Size.Width; x++) {
				int panelColorId = rows[y].Grids[x];
				if (panelColorId == Define.EmptyPanelColorId || panelColorId == Define.BlockPanelColorId) continue;
				return false;
			}
		}
		return true;
	}

	// 入れ替え操作可能色判定
	bool IsSwappableColor(int x, int y)
	{
		int colorId = GetPanelColorId(x, y);
		return colorId != Define.InvalidPanelColorId
			&& colorId != Define.EmptyPanelColorId
			&& colorId != Define.BlockPanelColorId;
	}

	// 削除対象判定
	bool IsClearTargetPoint(int x, int y)
	{
		int baseColorId = GetPanelColorId(x, y);
		if (baseColorId == Define.InvalidPanelColorId
		|| baseColorId == Define.EmptyPanelColorId
		|| baseColorId == Define.BlockPanelColorId) {
			return false;
		}

		Dir upDir = new Dir(Dir.DirType.Up);
		Dir downDir = new Dir(Dir.DirType.Down);
		Dir leftDir = new Dir(Dir.DirType.Left);
		Dir rightDir = new Dir(Dir.DirType.Right);

		int upColorId = GetPanelColorId(x + upDir.X, y + upDir.Y);
		int downColorId = GetPanelColorId(x + downDir.X, y + downDir.Y);
		int leftColorId = GetPanelColorId(x + leftDir.X, y + leftDir.Y);
		int rightColorId = GetPanelColorId(x + rightDir.X, y + rightDir.Y);

		// 間
		if (baseColorId == upColorId && baseColorId == downColorId) return true;
		if (baseColorId == leftColorId && baseColorId == rightColorId) return true;

		int up2ColorId = GetPanelColorId(x + upDir.X * 2, y + upDir.Y * 2);
		int down2ColorId = GetPanelColorId(x + downDir.X * 2, y + downDir.Y * 2);
		int left2ColorId = GetPanelColorId(x + leftDir.X * 2, y + leftDir.Y * 2);
		int right2ColorId = GetPanelColorId(x + rightDir.X * 2, y + rightDir.Y * 2);

		// 連続
		if (baseColorId == upColorId && baseColorId == up2ColorId) return true;
		if (baseColorId == downColorId && baseColorId == down2ColorId) return true;
		if (baseColorId == leftColorId && baseColorId == left2ColorId) return true;
		if (baseColorId == rightColorId && baseColorId == right2ColorId) return true;

		return false;
	}
}
