using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 定数定義
public static class Define
{
	// SaveData
	public const int SaveBoardDataNum = 20;
	public const string SaveDataKey = "SaveData";
	public const string SaveDataDefaultValue = "Empty";

	// BoardData
	public const int GridSizeMax = 10;
	public const float BoardAreaMargin = 150.0f;

	// Pallet
	public const float PenMarkSizeScale = 0.5f;

	// PanelColor
	public const int InvalidPanelColorId = -1;
	public const int EmptyPanelColorId = 0;
	public const int BlockPanelColorId = 1;
	public const int PanelColorIdMax = 8;

	// GUIWindow
	public const float GUIWindowStartY = 300.0f;
	public enum WindowIdType : int
	{
		BoardData,
	}
}
