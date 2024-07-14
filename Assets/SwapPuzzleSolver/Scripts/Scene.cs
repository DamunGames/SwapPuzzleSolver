using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene : MonoBehaviour
{
	[SerializeField] ResouceObjects resourceObjects;
	[SerializeField] HierarchyObjects hierarchyObjects;

	GameData gameData = new GameData();

	List<IGUIWindow> guiWindows = new List<IGUIWindow>();
	string[] windowOpenButtonNames;
	int currentGUIWindowIdx;

	void Start()
	{
		gameData.ResrouceObjects = resourceObjects;
		gameData.HierarchyObjects = hierarchyObjects;

		gameData.Load();

		gameData.ScreenSize = new Size(Screen.width, Screen.height);

		gameData.IsEditable = true;
		gameData.EditingBoardData = new BoardData();

		gameData.BoardPanels = new BoardPanels(gameData);

		gameData.PalletPanels = new PalletPanels(gameData);
		gameData.PalletPanels.Initialize();

		gameData.BoardDataGUIWindow = new BoardDataGUIWindow(gameData);
		RegisterWindow(gameData.BoardDataGUIWindow);

		gameData.PuzzleSolveGUIWindow = new PuzzleSolveGUIWindow(gameData);
		RegisterWindow(gameData.PuzzleSolveGUIWindow);

		windowOpenButtonNames = new string[guiWindows.Count];
		for (int i = 0; i < guiWindows.Count; i++) {
			windowOpenButtonNames[i] = guiWindows[i].WindowTitle;
		}

		guiWindows[currentGUIWindowIdx].Open();

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
		// 択一でWindowを選ぶ
		int prevWindowIdx = currentGUIWindowIdx;
		currentGUIWindowIdx = GUILayout.SelectionGrid(currentGUIWindowIdx, windowOpenButtonNames, 1);
		if (currentGUIWindowIdx != prevWindowIdx) {
			guiWindows[prevWindowIdx].Close();
			guiWindows[currentGUIWindowIdx].Open();
		}

		guiWindows[currentGUIWindowIdx].OnGUI();
	}

	void RegisterWindow(IGUIWindow guiWindow, bool isOpenable = true)
	{
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
