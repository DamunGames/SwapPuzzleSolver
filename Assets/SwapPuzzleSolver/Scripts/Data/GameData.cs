using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
	public SaveData SaveData;

	public BoardData EditingBoardData = new BoardData();

	public BoardDataGUIWindow BoardDataGUIWindow;

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
