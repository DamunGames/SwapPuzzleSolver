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

	// GUIWindow
	public const float GUIWindowStartY = 300.0f;
	public enum WindowIdType : int
	{
		BoardData,
	}
}
