
using System.Collections.Generic;
using System.Linq;

namespace Below
{
	/// <summary>
	/// fps counter
	/// </summary>
	public class FrameCounter
	{

		public long TotalFrames { get; private set; }
		public float TotalSeconds { get; private set; }
		public float AverageFramesPerSecond { get; private set; }
		public float CurrentFramesPerSecond { get; private set; }

		public const int MAX = 100;

		private Queue<float> sampleBuffer = new Queue<float>();

		public FrameCounter() { }

		/// <summary>
		///	calculate fps
		/// </summary>
		/// <param name="deltaTime"></param>
		/// <returns></returns>
		public bool Update(float deltaTime)
		{
			CurrentFramesPerSecond = 1.0f / deltaTime;
			sampleBuffer.Enqueue(CurrentFramesPerSecond);

			if (sampleBuffer.Count > MAX)
			{
				sampleBuffer.Dequeue();
				AverageFramesPerSecond = sampleBuffer.Average(i => i);
			}
			else
				AverageFramesPerSecond = CurrentFramesPerSecond;

			TotalFrames++;
			TotalSeconds += deltaTime;
			return true;
		}
	}
}
