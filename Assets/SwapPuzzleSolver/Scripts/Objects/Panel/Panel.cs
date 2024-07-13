using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Panel : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
	[SerializeField] RectTransform rectTransform;
	[SerializeField] Image image;
	[SerializeField] TextMeshProUGUI textMeshProUGUI;

	public Point Point;
	
	UnityAction<Panel> pointerDownOrDragEnterAction;

	// レイアウト設定
	public void SetSizeDelta(Vector2 sizeDelta) { if (rectTransform != null) rectTransform.sizeDelta = sizeDelta; }
	public void SetLocalPosition(Vector3 localPosition) { if (rectTransform != null) rectTransform.localPosition = localPosition; }

	// 表示内容設定
	public void SetColor(Color color) { if (image != null) image.color = color; }
	public void SetText(string text) => textMeshProUGUI?.SetText(text);

	// Action設定
	public void SetAction(UnityAction<Panel> unityAction) => pointerDownOrDragEnterAction = unityAction;

	// イベント関数
	public void OnPointerEnter(PointerEventData eventData)
	{
		Debug.Log("OnPointerEnter");
		if (eventData.eligibleForClick) {
			pointerDownOrDragEnterAction?.Invoke(this);
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		Debug.Log("OnPointerDown");
		pointerDownOrDragEnterAction?.Invoke(this);
	}

	// パネル色取得
	public static Color PanelColorIdToColor(int colorId)
	{
		if (colorId == Define.EmptyPanelColorId) {
			return Color.white;
		}
		else if (colorId == Define.BlockPanelColorId) {
			return Color.gray;
		}
		else {
			float h = (float)(colorId - Define.BlockPanelColorId - 1) / (float)(Define.PanelColorIdMax - Define.BlockPanelColorId);
			return Color.HSVToRGB(h, 1.0f, 1.0f);
		}
	}
}
