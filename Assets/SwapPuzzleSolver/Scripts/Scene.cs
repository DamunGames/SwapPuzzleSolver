using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene : MonoBehaviour
{
	GameData gameData = new GameData();

	List<IGUIWindow> openableWindows = new List<IGUIWindow>();
	List<IGUIWindow> guiWindows = new List<IGUIWindow>();

	void Start()
	{
		gameData.Load();

		gameData.BoardDataGUIWindow = new BoardDataGUIWindow(gameData);
		RegisterWindow(gameData.BoardDataGUIWindow);
	}

	void Update()
	{

	}

	void OnGUI()
	{
		// GUIWindow表示ボタン
		foreach (var guiWindow in openableWindows) {
			if (GUILayout.Button(guiWindow.WindowTitle)) {
				guiWindow.Open();
			}
		}

		// GUIWindowの表示処理
		foreach (var guiWindow in guiWindows) {
			guiWindow.OnGUI();
		}
	}

	void RegisterWindow(IGUIWindow guiWindow, bool isOpenable = true)
	{
		if (isOpenable) {
			openableWindows.Add(guiWindow);
		}
		guiWindows.Add(guiWindow);
	}
}
