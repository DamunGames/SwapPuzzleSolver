using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPanels
{
	GameData gameData;
	List<Panel> panels = new List<Panel>();

	public BoardPanels(GameData gameData) => this.gameData = gameData;

	// 盤面表示
	public void Show(BoardData boardData, bool isEditable)
	{
		if (panels != null && panels.Count > 0) {
			foreach (var item in panels) {
				GameObject.Destroy(item.gameObject);
			}
		}

		float panelSize = GetPanelSize(boardData);
		Vector2 sizeDelta = new Vector2(panelSize, panelSize);

		panels = new List<Panel>();
		for (int y = 0; y < boardData.Size.Height; y++) {
			for (int x = 0; x < boardData.Size.Width; x++) {
				GameObject gameObject = GameObject.Instantiate(gameData.ResrouceObjects.PanelPrefab, gameData.HierarchyObjects.BoardPanelParent);
				Panel panel = gameObject.GetComponent<Panel>();
				panel.Point = new Point(x, y);
				panel.SetSizeDelta(sizeDelta);
				panel.SetLocalPosition(GetLocalPosition(x, y, panelSize));
				panel.SetAction(isEditable ? PointerDownOrDragEnterAction : null);

				panels.Add(panel);

				SetPanelDispItems(boardData, panel);
			}
		}
	}

	// サイズ取得
	int GetLineNum(BoardData boardData) => Math.Max(boardData.Size.Width, boardData.Size.Height);
	float GetScreenSizeMin() => Mathf.Min(Screen.width, Screen.height);
	public float GetBoardAreaSize() => Mathf.Max(GetScreenSizeMin() - Define.BoardAreaMargin, 200.0f);
	float GetPanelSize(BoardData boardData) => GetBoardAreaSize() / (float)GetLineNum(boardData);

	// 座標取得
	Vector3 GetLocalPosition(int x, int y, float panelSize)
	{
		float panelSizeHalf = panelSize * 0.5f;
		return new Vector3(x * panelSize + panelSizeHalf, y * panelSize + panelSizeHalf);
	}

	// パネル表示内容設定
	void SetPanelDispItems(BoardData boardData, Panel panel)
	{
		int panelColorId = boardData.GetPanelColorId(panel.Point.X, panel.Point.Y);
		panel.SetText(panelColorId.ToString());
		panel.SetColor(Panel.PanelColorIdToColor(panelColorId));
	}

	// 入力Action
	void PointerDownOrDragEnterAction(Panel panel)
	{
		int panelColorId = gameData.EditingBoardData.GetPanelColorId(panel.Point.X, panel.Point.Y);

		panelColorId++;
		if (panelColorId > Define.PanelColorIdMax) panelColorId = Define.EmptyPanelColorId;
		gameData.EditingBoardData.SetPanelColorId(panel.Point, panelColorId);

		SetPanelDispItems(gameData.EditingBoardData, panel);
	}
}
