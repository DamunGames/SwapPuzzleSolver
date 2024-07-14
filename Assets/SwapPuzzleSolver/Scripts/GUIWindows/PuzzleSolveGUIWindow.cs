using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleSolveGUIWindow : GUIWindowBase
{
	protected override Define.WindowIdType WindowId => Define.WindowIdType.PuzzleSolve;
	protected override Rect InitialScreenRect => new Rect(new Vector2(0.0f, 120.0f), new Vector2(120.0f, 30.0f));

	enum StateType
	{
		Main,
		Calculating,
		TraceSolveOperations,
	}

	UnityAction windowFunction;

	PuzzleSolver puzzleSolver;

	int puzzleSolveTraceStepIdx;

	public PuzzleSolveGUIWindow(GameData gameData) : base(gameData)
	{
		puzzleSolver = new PuzzleSolver();
	}

	public override void Open()
	{
		SetState(StateType.Main);
		base.Open();
	}

	protected override void WindowFunction(int windowId)
	{
		windowFunction?.Invoke();

		GUI.DragWindow();
	}

	void WindowFunctionMain()
	{
		if (GUILayout.Button("Calculate Solve")) {
			puzzleSolver.SetBoardData(gameData.EditingBoardData.Clone());
			puzzleSolver.StartCalculateSolve();
			SetState(StateType.Calculating);
		}
		GUILayout.Label("");
		GUILayout.Label("");
		GUILayout.Label("");
	}

	void WindowFunctionCalculating()
	{
		GUILayout.Label("Calculating...");

		if (GUILayout.Button("Calcel")) {
			puzzleSolver.CancelCalculateSolve();
			SetState(StateType.Main);
		}

		GUILayout.Label("");
		GUILayout.Label("");

		if (!puzzleSolver.IsRunning) {
			if (puzzleSolver.IsSuccess) {
				puzzleSolveTraceStepIdx = 0;
				ShowSolveStep();
				SetState(StateType.TraceSolveOperations);
			}
			else {
				SetState(StateType.Main);
			}
		}
	}
	
	void WindowFunctionTraceSolveOperations()
	{
		if (GUILayout.Button("OK")) {
			SetState(StateType.Main);
		}

		if (GUILayout.Button(">")) {
			puzzleSolveTraceStepIdx++;
			if (puzzleSolveTraceStepIdx >= puzzleSolver.BoardBySolveSteps.Count) {
				puzzleSolveTraceStepIdx = 0;
			}
			ShowSolveStep();
		}

		GUILayout.Label($"{puzzleSolveTraceStepIdx}");

		if (GUILayout.Button("<")) {
			puzzleSolveTraceStepIdx--;
			if (puzzleSolveTraceStepIdx < 0) {
				puzzleSolveTraceStepIdx = puzzleSolver.BoardBySolveSteps.Count - 1;
			}
			ShowSolveStep();
		}
	}

	void SetState(StateType stateType)
	{
		switch (stateType) {
			case StateType.Main:
				windowFunction = WindowFunctionMain;
				break;
			case StateType.Calculating:
				windowFunction = WindowFunctionCalculating;
				break;
			case StateType.TraceSolveOperations:
				windowFunction = WindowFunctionTraceSolveOperations;
				break;
			default:
				break;
		}
	}

	void ShowSolveStep()
	{
		SwapOperation swapOperationArrow = null;
		if (puzzleSolveTraceStepIdx < puzzleSolver.SolveOperations.Count) {
			swapOperationArrow = puzzleSolver.SolveOperations[puzzleSolveTraceStepIdx];
		}
		gameData.BoardPanels.Show(puzzleSolver.BoardBySolveSteps[puzzleSolveTraceStepIdx], false, swapOperationArrow);
	}
}
