using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GUIWindowBase : IGUIWindow
{
	// 表示管理情報
	protected bool isOpened;
	protected Rect screenRect;

	protected GUIWindowBase() => screenRect = InitialScreenRect;

	// インターフェースの実装
	public string WindowTitle => WindowId.ToString();
	public virtual void Open() => isOpened = true;
	public virtual void Close() => isOpened = false;
	public virtual void OnGUI()
	{
		if (isOpened) {
			screenRect = GUILayout.Window((int)WindowId, screenRect, WindowFunction, WindowTitle);
		}
	}

	// 抽象プロパティ、メソッド
	protected abstract Define.WindowIdType WindowId { get; }
	protected abstract Rect InitialScreenRect { get; }
	protected abstract void WindowFunction(int windowId);
}
