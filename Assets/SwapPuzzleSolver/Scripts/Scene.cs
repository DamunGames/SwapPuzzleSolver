using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene : MonoBehaviour
{
	[SerializeField] ResouceObjects resourceObjects;
	[SerializeField] HierarchyObjects hierarchyObjects;

	GameData gameData = new GameData();

	List<IGUIWindow> openableWindows = new List<IGUIWindow>();
	List<IGUIWindow> guiWindows = new List<IGUIWindow>();

	void Start()
	{
		gameData.ResrouceObjects = resourceObjects;
		gameData.HierarchyObjects = hierarchyObjects;

		gameData.Load();

		gameData.IsEditable = true;
		gameData.EditingBoardData = new BoardData();

		gameData.BoardPanels = new BoardPanels(gameData);
		gameData.BoardPanels.Show(gameData.EditingBoardData, true);

		gameData.BoardDataGUIWindow = new BoardDataGUIWindow(gameData);
		RegisterWindow(gameData.BoardDataGUIWindow);
	}

	void Update()
	{
		float boardAreaSize = gameData.BoardPanels.GetBoardAreaSize();
		gameData.HierarchyObjects.BoardAreaRectTransform.sizeDelta = new Vector2(boardAreaSize, boardAreaSize);
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
