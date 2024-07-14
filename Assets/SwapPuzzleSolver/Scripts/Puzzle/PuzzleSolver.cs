using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

public class PuzzleSolver
{
	// 操作可能情報
	class SwappableOperation
	{
		public int X, Y;
		public List<Dir> Dirs;

		public SwappableOperation(int x, int y, List<Dir> dirs)
		{
			X = x;
			Y = y;
			Dirs = dirs;
		}
	}

	// 調査ステップ情報
	class StepCondition
	{
		public BoardData BoardData;
		public List<SwappableOperation> SwappableOperations;

		public int LastSwapOperationIdx;
		public int LastSwapOperationDirIdx;

		public StepCondition(BoardData boardData)
		{
			BoardData = boardData.Clone();
			SwappableOperations = new List<SwappableOperation>();
		}

		public StepCondition(StepCondition src)
		{
			BoardData = src.BoardData.Clone();
			SwappableOperations = new List<SwappableOperation>();
		}

		public SwapOperation GetLastSwapOpeartion()
		{
			if (LastSwapOperationIdx < 0 || LastSwapOperationIdx >= SwappableOperations.Count) return null;
			if (LastSwapOperationDirIdx < 0 || LastSwapOperationDirIdx >= SwappableOperations[LastSwapOperationIdx].Dirs.Count) return null;
			return new SwapOperation(
				SwappableOperations[LastSwapOperationIdx].X,
				SwappableOperations[LastSwapOperationIdx].Y,
				SwappableOperations[LastSwapOperationIdx].Dirs[LastSwapOperationDirIdx]
				);
		}
	}

	public bool IsRunning { get; private set; }
	public bool IsSuccess { get; private set; }

	BoardData baseBoardData;
	List<StepCondition> stepConditions;
	public List<SwapOperation> SolveOperations { get; private set; }
	public List<BoardData> BoardBySolveSteps { get; private set; }
	CancellationTokenSource cancellationTokenSource;

	// 計算元のボード情報を設定
	public void SetBoardData(BoardData boardData)
	{
		baseBoardData = boardData;
	}

	// 計算開始
	public void StartCalculateSolve()
	{
		if (baseBoardData == null) {
			Debug.LogWarning("Board data is null.");
			return;
		}

		IsRunning = true;
		IsSuccess = false;

		cancellationTokenSource?.Cancel();
		cancellationTokenSource = new CancellationTokenSource();

		stepConditions = new List<StepCondition>();
		stepConditions.Add(new StepCondition(baseBoardData));
		SolveOperations = new List<SwapOperation>();
		BoardBySolveSteps = new List<BoardData>();
		CalculateSolveRecursive(0).WithOnKill(OnKillCalculateSolve, cancellationTokenSource).Forget();
	}

	// 計算キャンセル
	public void CancelCalculateSolve() => cancellationTokenSource?.Cancel();

	// 計算終了
	void OnKillCalculateSolve()
	{
		IsRunning = false;
	}

	async UniTask CalculateSolveRecursive(int stepIdx)
	{
		await UniTask.DelayFrame(1, cancellationToken: cancellationTokenSource.Token);
		if (!IsRunning) return;

		// ステップをインクリメント
		StepCondition prevStepCondition = stepConditions[stepIdx];
		StepCondition stepCondition = new StepCondition(prevStepCondition);
		stepConditions.Add(stepCondition);
		stepIdx++;

		// 操作可能ポイントの羅列
		CalculateSwappables(stepCondition);

		// 操作可能ポイントを総当たり
		for (int i = 0; i < stepCondition.SwappableOperations.Count; i++) {
			var swappableOperation = stepCondition.SwappableOperations[i];

			// 操作のポイントを保存
			stepCondition.LastSwapOperationIdx = i;

			for (int j = 0; j < swappableOperation.Dirs.Count; j++) {
				var dir = swappableOperation.Dirs[j];

				// 直前のボードの状態を復元
				stepCondition.BoardData = prevStepCondition.BoardData.Clone();

				// 実際に操作してパネルを削除
				stepCondition.BoardData.SwapDir(swappableOperation.X, swappableOperation.Y, dir);
				do {
					stepCondition.BoardData.ClearCollectedPanels();
				} while (stepCondition.BoardData.FallPanels());

				// 操作の向きを保存
				stepCondition.LastSwapOperationDirIdx = j;

				if (stepCondition.BoardData.IsAllCleared()) {
					// 全クリア成功までの履歴を保存
					for (int k = 1; k <= stepIdx; k++) {
						SolveOperations.Add(stepConditions[k].GetLastSwapOpeartion());
						BoardBySolveSteps.Add(stepConditions[k - 1].BoardData.Clone());
					}
					BoardBySolveSteps.Add(stepConditions[stepIdx].BoardData.Clone());
					IsSuccess = true;

					// ステップをデクリメント
					if (stepConditions.Count > 0) {
						stepConditions.RemoveAt(stepConditions.Count - 1);
					}

					// 捜査を終了
					cancellationTokenSource?.Cancel();
					return;
				}
				else {
					// 次のステップへ
					await CalculateSolveRecursive(stepIdx);
				}
			}
		}

		// ステップをデクリメント
		if (stepConditions.Count > 0) {
			stepConditions.RemoveAt(stepConditions.Count - 1);
		}
	}

	// 操作可能な情報を羅列
	void CalculateSwappables(StepCondition stepCondition)
	{
		List<Dir> dirs = new List<Dir>() { new Dir(Dir.DirType.Up), new Dir(Dir.DirType.Down), new Dir(Dir.DirType.Left), new Dir(Dir.DirType.Right) };

		stepCondition.SwappableOperations = new List<SwappableOperation>();
		// 各座標を順次処理
		for (int y = 0; y < stepCondition.BoardData.Size.Height; y++) {
			for (int x = 0; x < stepCondition.BoardData.Size.Width; x++) {
				// 操作可能向きを順次判定
				List<Dir> swappableDirs = new List<Dir>();
				foreach (var dir in dirs) {
					if (stepCondition.BoardData.IsSwappable(x, y, dir)) {
						swappableDirs.Add(dir);
					}
				}

				// 操作可能向きを保存
				if (swappableDirs.Count > 0) {
					stepCondition.SwappableOperations.Add(new SwappableOperation(x, y, swappableDirs));
				}
			}
		}
	}
}
