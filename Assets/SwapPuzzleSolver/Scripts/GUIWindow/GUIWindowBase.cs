using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GUIWindowBase : IGUIWindow
{
	// �\���Ǘ����
	protected bool isOpened;
	protected Rect screenRect;

	protected GUIWindowBase() => screenRect = InitialScreenRect;

	// �C���^�[�t�F�[�X�̎���
	public string WindowTitle => WindowId.ToString();
	public virtual void Open() => isOpened = true;
	public virtual void Close() => isOpened = false;
	public virtual void OnGUI()
	{
		if (isOpened) {
			screenRect = GUILayout.Window((int)WindowId, screenRect, WindowFunction, WindowTitle);
		}
	}

	// ���ۃv���p�e�B�A���\�b�h
	protected abstract Define.WindowIdType WindowId { get; }
	protected abstract Rect InitialScreenRect { get; }
	protected abstract void WindowFunction(int windowId);
}
