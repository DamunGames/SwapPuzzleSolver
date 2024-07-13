using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGUIWindow
{
	public string WindowTitle { get; }

	public void Open();
	public void Close();

	public void OnGUI();
}
