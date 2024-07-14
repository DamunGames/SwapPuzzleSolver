using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
	public ResouceObjects ResrouceObjects;
	public HierarchyObjects HierarchyObjects;

	public SaveData SaveData;

	public Size ScreenSize;

	public bool IsEditable;
	public int PenColorId;
	public BoardData EditingBoardData;

	public BoardPanels BoardPanels;
	public PalletPanels PalletPanels;

	public BoardDataGUIWindow BoardDataGUIWindow;
	public PuzzleSolveGUIWindow PuzzleSolveGUIWindow;

	public void Save()
	{
		string saveStr = JsonUtility.ToJson(SaveData, true);
		Debug.Log($"Save: {saveStr}");
		PlayerPrefs.SetString(Define.SaveDataKey, saveStr);
		PlayerPrefs.Save();
	}

	public void Load()
	{
		string loadStr = PlayerPrefs.GetString(Define.SaveDataKey, Define.SaveDataDefaultValue);
		Debug.Log($"Load: {loadStr}");
		if (loadStr != Define.SaveDataDefaultValue) {
			SaveData = JsonUtility.FromJson<SaveData>(loadStr);
		}
		else {
			SaveData = new SaveData();
		}
	}
}
