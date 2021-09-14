using System;
using System.Threading;

namespace Scripts.Utils
{
	/// <summary>
	/// A CustomYieldInstruction that executes a task on a new thread and keeps waiting until it's done.
	/// http://JacksonDunstan.com/articles/3746
	/// </summary>
	public class WaitForThreadedTask : UnityEngine.CustomYieldInstruction
	{
		/// <summary>
		/// If the thread is still running
		/// </summary>
		private bool isRunning;

		/// <summary>
		/// When the given task finishes, <see cref="keepWaiting"/> returns true.
		/// </summary>
		/// <param name="task">Task to execute in the thread</param>
		public WaitForThreadedTask(Action task)
		{
			isRunning = true;
			new Thread(() => { task(); isRunning = false; }).Start();
		}

		/// <summary>
		/// If the coroutine should keep waiting
		/// </summary>
		/// <value>If the thread is still running</value>
		public override bool keepWaiting { get { return isRunning; } }
	}
}