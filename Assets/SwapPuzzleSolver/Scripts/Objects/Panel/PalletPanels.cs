using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalletPanels
{
	GameData gameData;
	List<Panel> panels;

	public PalletPanels(GameData gameData) => this.gameData = gameData;

	// 初期化
	public void Initialize()
	{
		if (panels != null && panels.Count > 0) {
			foreach (var item in panels) {
				GameObject.Destroy(item.gameObject);
			}
		}

		panels = new List<Panel>();
		for (int i = 0; i <= Define.PanelColorIdMax; i++) {
			GameObject gameObject = GameObject.Instantiate(gameData.ResrouceObjects.PanelPrefab, gameData.HierarchyObjects.PalletPanelParent);
			Panel panel = gameObject.GetComponent<Panel>();
			panel.Point = new Point(i, i);
			switch (i) {
				case 0:
					panel.SetText("Empty");
					break;
				case 1:
					panel.SetText("Block");
					break;
				default:
					panel.SetText(i.ToString());
					break;
			}
			panel.SetColor(Panel.PanelColorIdToColor(i));
			panel.SetAction(PointerDownOrDragEnterAction);

			panels.Add(panel);
		}

		// ペン表示位置更新
		gameData.HierarchyObjects.PenMarkRectTransform.SetParent(panels[gameData.PenColorId].RectTransform, false);

		Resize();
	}

	// 表示サイズ更新
	public void Resize()
	{
		float panelSize = GetPanelSize();

		// エリア
		Vector2 areaSizeDelta = gameData.HierarchyObjects.PalletAreaRectTransform.sizeDelta;
		areaSizeDelta.x = panelSize;
		gameData.HierarchyObjects.PalletAreaRectTransform.sizeDelta = areaSizeDelta;

		// ペンマーク
		gameData.HierarchyObjects.PenMarkRectTransform.sizeDelta = new Vector2(panelSize * Define.PenMarkSizeScale, panelSize * Define.PenMarkSizeScale);

		// パネル
		if (panels != null) {
			Vector2 sizeDelta = new Vector2(panelSize, panelSize);
			foreach (var panel in panels) {
				panel.SetSizeDelta(sizeDelta);
			}
		}
	}

	float GetPanelSize() => (float)gameData.ScreenSize.Height / (float)(Define.PanelColorIdMax + 1);

	// 入力Action
	void PointerDownOrDragEnterAction(Panel panel)
	{
		gameData.PenColorId = panel.Point.X;

		// Penカラー表示切り替え
		gameData.HierarchyObjects.PenMarkRectTransform.SetParent(panel.RectTransform, false);
	}
}
