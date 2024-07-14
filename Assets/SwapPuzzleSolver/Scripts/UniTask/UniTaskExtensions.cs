using System;
using System.Threading;
using Cysharp.Threading.Tasks;

// UniTaskの拡張関数に登録できるdelegate定義
public delegate void OnCancelOrComplete(bool isCancelled);
public delegate void OnCancelOrKill(bool isCancelled);
public delegate void OnCompleteOrKill(bool isCompleted);

/// <summary>
/// UniTaskの拡張関数
/// </summary>
static class UniTaskExtensions
{
	// UniTaskのCancel時にonCancelを発火
	public static async UniTask WithOnCancel(this UniTask uniTask, Action onCancel, CancellationTokenSource tokenSource)
	{
		// Cancel or TrySet*を待つcompletionSource
		AutoResetUniTaskCompletionSource completionSource = AutoResetUniTaskCompletionSource.Create();
		UniTask.Void(async () => {
			try {
				// Cancel or TrySet*を待つ
				await completionSource.Task.AttachExternalCancellation(tokenSource.Token);
			}
			catch (OperationCanceledException) {
				// Cancelの後処理
				onCancel?.Invoke();
			}
		});

		// uniTask完了を待つ
		await uniTask;

		// completionSourceにも結果を伝えtry内のawaitを終了
		completionSource.TrySetResult();
	}

	// UniTaskの完了時にonCompleteを発火
	public static async UniTask WithOnComplete(this UniTask uniTask, Action onComplete)
	{
		// uniTask完了を待つ
		await uniTask;

		// 完了の後処理
		onComplete?.Invoke();
	}

	// UniTaskのCancel時もしくは完了時にonKillを発火
	public static async UniTask WithOnKill(this UniTask uniTask, Action onKill, CancellationTokenSource tokenSource)
	{
		// Cancel or TrySet*を待つcompletionSource
		AutoResetUniTaskCompletionSource completionSource = AutoResetUniTaskCompletionSource.Create();
		UniTask.Void(async () => {
			try {
				// Cancel or TrySet*を待つ
				await completionSource.Task.AttachExternalCancellation(tokenSource.Token);
			}
			finally {
				// await終了 or Cancel or TrySet*の後処理(必ず通る)
				onKill?.Invoke();
			}
		});

		// uniTask完了を待つ
		await uniTask;

		// completionSourceにも結果を伝えtry内のawaitを終了
		completionSource.TrySetResult();
	}

	// UniTaskのCancel時にonCancelを、UniTask完了時にonCompleteを発火
	public static async UniTask WithOnCancelAndComplete(this UniTask uniTask, Action onCancel, Action onComplete, CancellationTokenSource tokenSource)
	{
		// Cancel or TrySet*を待つcompletionSource
		AutoResetUniTaskCompletionSource completionSource = AutoResetUniTaskCompletionSource.Create();
		UniTask.Void(async () => {
			try {
				// Cancel or TrySet*を待つ
				await completionSource.Task.AttachExternalCancellation(tokenSource.Token);
			}
			catch (OperationCanceledException) {
				// Cancelの後処理
				onCancel?.Invoke();
			}
		});

		// uniTask完了を待つ
		await uniTask;

		// 完了の後処理
		onComplete?.Invoke();

		// completionSourceにも結果を伝えtry内のawaitを終了
		completionSource.TrySetResult();
	}

	// UniTaskのCancel時にtrueを、UniTask完了時にfalseを引数にonCancelOrCompleteを発火
	public static async UniTask WithOnCancelOrComplete(this UniTask uniTask, OnCancelOrComplete onCancelOrComplete, CancellationTokenSource tokenSource)
	{
		// Cancel or TrySet*を待つcompletionSource
		AutoResetUniTaskCompletionSource completionSource = AutoResetUniTaskCompletionSource.Create();
		UniTask.Void(async () => {
			try {
				// Cancel or TrySet*を待つ
				await completionSource.Task.AttachExternalCancellation(tokenSource.Token);
			}
			catch (OperationCanceledException) {
				// Cancelの後処理
				onCancelOrComplete?.Invoke(true);
				onCancelOrComplete = null;
			}
		});

		// uniTask完了を待つ
		await uniTask;

		// 完了の後処理
		onCancelOrComplete?.Invoke(false);
		onCancelOrComplete = null;

		// completionSourceにも結果を伝えtry内のawaitを終了
		completionSource.TrySetResult();
	}

	// UniTask完了時にonCompleteを、UniTaskの完了時もしくはCancel時にonKillを発火
	public static async UniTask WithOnCompleteAndKill(this UniTask uniTask, Action onComplete, Action onKill, CancellationTokenSource tokenSource)
	{
		// Cancel or TrySet*を待つcompletionSource
		AutoResetUniTaskCompletionSource completionSource = AutoResetUniTaskCompletionSource.Create();
		UniTask.Void(async () => {
			try {
				// Cancel or TrySet*を待つ
				await completionSource.Task.AttachExternalCancellation(tokenSource.Token);
			}
			finally {
				// await終了 or Cancel or TrySet*の後処理(必ず通る)
				onKill?.Invoke();
			}
		});

		// uniTask完了を待つ
		await uniTask;

		// 完了の後処理
		onComplete?.Invoke();

		// completionSourceにも結果を伝えtry内のawaitを終了
		completionSource.TrySetResult();
	}

	// UniTask完了時にonCompleteを、UniTaskの完了時もしくはCancel時にonKillを発火
	public static async UniTask WithOnCompleteOrKill(this UniTask uniTask, OnCompleteOrKill onCompleteOrKill, CancellationTokenSource tokenSource)
	{
		// Cancel or TrySet*を待つcompletionSource
		AutoResetUniTaskCompletionSource completionSource = AutoResetUniTaskCompletionSource.Create();
		UniTask.Void(async () => {
			try {
				// Cancel or TrySet*を待つ
				await completionSource.Task.AttachExternalCancellation(tokenSource.Token);
			}
			finally {
				// await終了 or Cancel or TrySet*の後処理(必ず通る)
				onCompleteOrKill?.Invoke(false);
				onCompleteOrKill = null;
			}
		});

		// uniTask完了を待つ
		await uniTask;

		// 完了の後処理
		onCompleteOrKill?.Invoke(true);
		onCompleteOrKill = null;

		// completionSourceにも結果を伝えtry内のawaitを終了
		completionSource.TrySetResult();
	}

	// UniTaskのCancel時にonCancelを、UniTaskの完了時もしくはCancel時にonKillを発火
	public static async UniTask WithOnCancelAndKill(this UniTask uniTask, Action onCancel, Action onKill, CancellationTokenSource tokenSource)
	{
		// Cancel or TrySet*を待つcompletionSource
		AutoResetUniTaskCompletionSource completionSource = AutoResetUniTaskCompletionSource.Create();
		UniTask.Void(async () => {
			try {
				// Cancel or TrySet*を待つ
				await completionSource.Task.AttachExternalCancellation(tokenSource.Token);
			}
			catch (OperationCanceledException) {
				// Cancelの後処理
				onCancel?.Invoke();
			}
			finally {
				// await終了 or Cancel or TrySet*の後処理(必ず通る)
				onKill?.Invoke();
			}
		});

		// uniTask完了を待つ
		await uniTask;

		// completionSourceにも結果を伝えtry内のawaitを終了
		completionSource.TrySetResult();
	}

	// UniTaskのCancel時にtrueを、Cancel時にfalseを引数にonCancelOrKillを発火
	public static async UniTask WithOnCancelOrKill(this UniTask uniTask, OnCancelOrKill onCancelOrKill, CancellationTokenSource tokenSource)
	{
		// Cancel or TrySet*を待つcompletionSource
		AutoResetUniTaskCompletionSource completionSource = AutoResetUniTaskCompletionSource.Create();
		UniTask.Void(async () => {
			try {
				// Cancel or TrySet*を待つ
				await completionSource.Task.AttachExternalCancellation(tokenSource.Token);
			}
			catch (OperationCanceledException) {
				// Cancelの後処理
				onCancelOrKill?.Invoke(true);
				onCancelOrKill = null;
			}
			finally {
				// await終了 or Cancel or TrySet*の後処理(必ず通る)
				onCancelOrKill?.Invoke(false);
				onCancelOrKill = null;
			}
		});

		// uniTask完了を待つ
		await uniTask;

		// completionSourceにも結果を伝えtry内のawaitを終了
		completionSource.TrySetResult();
	}

	// UniTaskのCancel時にonCancelを、UniTask完了時にonCompleteを、UniTaskの完了時もしくはCancel時にonKillを発火
	public static async UniTask WithOnCancelAndCompleteAndKill(this UniTask uniTask, Action onCancel, Action onComplete, Action onKill, CancellationTokenSource tokenSource)
	{
		// Cancel or TrySet*を待つcompletionSource
		AutoResetUniTaskCompletionSource completionSource = AutoResetUniTaskCompletionSource.Create();
		UniTask.Void(async () => {
			try {
				// Cancel or TrySet*を待つ
				await completionSource.Task.AttachExternalCancellation(tokenSource.Token);
			}
			catch (OperationCanceledException) {
				// Cancelの後処理
				onCancel?.Invoke();
			}
			finally {
				// await終了 or Cancel or TrySet*の後処理(必ず通る)
				onKill?.Invoke();
			}
		});

		// uniTask完了を待つ
		await uniTask;

		// 完了の後処理
		onComplete?.Invoke();

		// completionSourceにも結果を伝えtry内のawaitを終了
		completionSource.TrySetResult();
	}
}