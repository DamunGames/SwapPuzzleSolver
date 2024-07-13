using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPanels
{
	GameData gameData;
	BoardData showPanelData;
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

		showPanelData = boardData;

		float panelSize = GetPanelSize(showPanelData);
		Vector2 sizeDelta = new Vector2(panelSize, panelSize);

		panels = new List<Panel>();
		for (int y = 0; y < showPanelData.Size.Height; y++) {
			for (int x = 0; x < showPanelData.Size.Width; x++) {
				GameObject gameObject = GameObject.Instantiate(gameData.ResrouceObjects.PanelPrefab, gameData.HierarchyObjects.BoardPanelParent);
				Panel panel = gameObject.GetComponent<Panel>();
				panel.Point = new Point(x, y);
				panel.SetAction(isEditable ? PointerDownOrDragEnterAction : null);

				panels.Add(panel);

				SetPanelDispItems(showPanelData, panel);
			}
		}

		Resize();
	}

	// 表示サイズ更新
	public void Resize()
	{
		// エリア
		float boardAreaSize = GetBoardAreaSize();
		gameData.HierarchyObjects.BoardAreaRectTransform.sizeDelta = new Vector2(boardAreaSize, boardAreaSize);

		// パネル
		float panelSize = GetPanelSize(showPanelData);
		Vector2 sizeDelta = new Vector2(panelSize, panelSize);
		if (panels != null) {
			foreach (var panel in panels) {
				panel.SetSizeDelta(sizeDelta);
				panel.SetLocalPosition(GetLocalPosition(panel.Point.X, panel.Point.Y, panelSize));
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
		gameData.EditingBoardData.SetPanelColorId(panel.Point, gameData.PenColorId);
		SetPanelDispItems(gameData.EditingBoardData, panel);
	}
}
