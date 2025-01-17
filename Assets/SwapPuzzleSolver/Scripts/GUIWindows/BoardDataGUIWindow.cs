using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardDataGUIWindow : GUIWindowBase
{
	protected override Define.WindowIdType WindowId => Define.WindowIdType.BoardData;
	protected override Rect InitialScreenRect => new Rect(new Vector2(0.0f, 120.0f), new Vector2(120.0f, 30.0f));

	enum StateType
	{
		WithSaveData,
		CreatingNewBoard,
	}

	StateType stateType;

	int boardDataIdx;
	string[] dataIdxSelectionGridContents;
	bool isImmidiateLoad = true;

	Size creatingBoardSize;
	string[] creatingBoardSizeSelectionGridContents;

	public BoardDataGUIWindow(GameData gameData) : base(gameData)
	{
		dataIdxSelectionGridContents = new string[Define.SaveBoardDataNum];
		for (int i = 0; i < Define.SaveBoardDataNum; i++) {
			dataIdxSelectionGridContents[i] = i.ToString();
		}

		creatingBoardSizeSelectionGridContents = new string[Define.GridSizeMax];
		for (int i = 0; i < Define.GridSizeMax; i++) {
			creatingBoardSizeSelectionGridContents[i] = (i+1).ToString();
		}
	}

	public override void Open()
	{
		stateType = StateType.WithSaveData;
		ShowSelectedBoardData();
		base.Open();
	}

	public void ShowSelectedBoardData()
	{
		gameData.EditingBoardData = gameData.SaveData.BoardDatas[boardDataIdx].Clone();
		gameData.BoardPanels.Show(gameData.EditingBoardData, true);
	}

	protected override void WindowFunction(int windowId)
	{
		switch (stateType) {
			case StateType.WithSaveData:
				WithSaveDataWindowFunction();
				break;
			case StateType.CreatingNewBoard:
				CreatingNewBoardWindowFunction();
				break;
			default:
				break;
		}

		GUI.DragWindow();
	}

	// セーブデータと作業データのやり取りメニュー
	void WithSaveDataWindowFunction()
	{
		GUILayout.Label($"BoardDataIdx:{boardDataIdx}");
		int prevBoardDataIdx = boardDataIdx;
		boardDataIdx = GUILayout.SelectionGrid(boardDataIdx, dataIdxSelectionGridContents, Define.SaveBoardDataNum / 2);
		if (boardDataIdx != prevBoardDataIdx && isImmidiateLoad) {
			ShowSelectedBoardData();
		}

		isImmidiateLoad = GUILayout.Toggle(isImmidiateLoad, "ImmidiateLoad");

		if (GUILayout.Button("Load")) {
			ShowSelectedBoardData();
		}

		if (GUILayout.Button("Save")) {
			gameData.SaveData.BoardDatas[boardDataIdx] = gameData.EditingBoardData.Clone();
			gameData.Save();
		}

		if (GUILayout.Button("New Board")) {
			creatingBoardSize = new Size(0, 0);
			stateType = StateType.CreatingNewBoard;
		}
	}

	// 新ボード作成メニュー
	void CreatingNewBoardWindowFunction()
	{
		GUILayout.Label("Creating new board");

		if (GUILayout.Button("Cancel")) {
			stateType = StateType.WithSaveData;
		}

		GUILayout.Label($"Width:{creatingBoardSize.Width + 1}");
		creatingBoardSize.Width = GUILayout.SelectionGrid(creatingBoardSize.Width, creatingBoardSizeSelectionGridContents, Define.GridSizeMax);

		GUILayout.Label($"Height:{creatingBoardSize.Height + 1}");
		creatingBoardSize.Height = GUILayout.SelectionGrid(creatingBoardSize.Height, creatingBoardSizeSelectionGridContents, Define.GridSizeMax);

		if (GUILayout.Button("Create")) {
			gameData.EditingBoardData = new BoardData(creatingBoardSize.Width + 1, creatingBoardSize.Height + 1);
			gameData.BoardPanels.Show(gameData.EditingBoardData, true);
			stateType = StateType.WithSaveData;
		}
	}
}
