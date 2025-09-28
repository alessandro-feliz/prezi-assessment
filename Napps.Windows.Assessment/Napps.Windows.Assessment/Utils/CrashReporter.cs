using System;

namespace Napps.Windows.Assessment.Utils
{
	public interface ICrashReporter
	{
		void ReportException(Exception exception);
	}

	public class CrashReporter : ICrashReporter
	{
		public void ReportException(Exception exception)
		{
			// reporting exception to Crash reporting service endpoint
		}
	}
}
