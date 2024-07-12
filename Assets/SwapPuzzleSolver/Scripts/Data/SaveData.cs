using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
	public BoardData[] BoardDatas;

	public SaveData() => BoardDatas = new BoardData[Define.SaveBoardDataNum];
}
