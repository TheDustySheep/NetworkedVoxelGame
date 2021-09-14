using System;
using System.Threading;

namespace Scripts.Utils
{
	public class WaitForMultipleThreadedTask : UnityEngine.CustomYieldInstruction
	{
		/// <summary>
		/// Greatet than 0 If the thread is still running
		/// </summary>
		private int threadsRunning = 0;

		/// <summary>
		/// When the given task finishes, <see cref="keepWaiting"/> returns true.
		/// </summary>
		/// <param name="task">Task to execute in the thread</param>
		public WaitForMultipleThreadedTask(int iterations, Action<int> tasks)
		{
			threadsRunning = iterations;

			for (int i = 0; i < iterations; i++)
            {
				new Thread(() => { tasks(i); threadsRunning--; }).Start();
			}
		}

		/// <summary>
		/// If the coroutine should keep waiting
		/// </summary>
		/// <value>If the thread is still running</value>
		public override bool keepWaiting { get { return threadsRunning == 0; } }
	}
}