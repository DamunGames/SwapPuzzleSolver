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

		gameData.ScreenSize = new Size(Screen.width, Screen.height);

		gameData.IsEditable = true;
		gameData.EditingBoardData = new BoardData();

		gameData.BoardPanels = new BoardPanels(gameData);
		gameData.BoardPanels.Show(gameData.EditingBoardData, true);

		gameData.PalletPanels = new PalletPanels(gameData);
		gameData.PalletPanels.Initialize();

		gameData.BoardDataGUIWindow = new BoardDataGUIWindow(gameData);
		RegisterWindow(gameData.BoardDataGUIWindow);

		UpdateScreenSize();
	}

	void Update()
	{
		if (gameData.ScreenSize.Width != Screen.width || gameData.ScreenSize.Height != Screen.height) {
			UpdateScreenSize();
		}
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

	void UpdateScreenSize()
	{
		gameData.ScreenSize.Width = Screen.width;
		gameData.ScreenSize.Height = Screen.height;

		gameData.BoardPanels.Resize();

		gameData.PalletPanels.Resize();
	}
}
