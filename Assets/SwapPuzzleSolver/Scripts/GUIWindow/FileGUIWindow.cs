using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileGUIWindow : GUIWindowBase
{
	protected override Define.WindowIdType WindowId => Define.WindowIdType.File;
	protected override Rect InitialScreenRect => new Rect(Vector2.zero, new Vector2(120.0f, 30.0f));

	protected override void WindowFunction(int windowId)
	{
		if (GUILayout.Button("Load")) {
			// TODO:ŽÀ‘•
		}

		if (GUILayout.Button("Save")) {
			// TODO:ŽÀ‘•
		}

		if (GUILayout.Button("Close")) {
			Close();
		}

		GUI.DragWindow();
	}
}
