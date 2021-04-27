using Core.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Logging
{
	public interface ILogger
	{
		void LogException(Exception ex);
		void LogInfo(string msg = "");

		string FilePath { get; }
	}

	public class Logger : ILogger
	{
		private readonly string _filePath;

		public Logger(string folderPath)
		{
            string fileName = $"{DateTime.Today.ToDateNumber().ToString()}.txt";
			this._filePath = Path.Combine(folderPath, fileName);

			System.IO.Directory.CreateDirectory(folderPath);

		}

		

		public string FilePath => _filePath;

		public void LogException(Exception ex)
		{
			try
			{
				using (StreamWriter sw = File.AppendText(FilePath))
				{
					sw.WriteLine(String.Format("{0} {1}", DateTime.Now, ex.ToString()));
				}
			}
			catch (Exception)
			{

			
			}
			
		}

		public void LogInfo(string msg = "")
		{
			try
			{
				using (StreamWriter sw = File.AppendText(FilePath))
				{
					sw.WriteLine(String.Format("{0} {1}", DateTime.Now, msg));
				}
			}
			catch (Exception)
			{


			}
		}



	}
}
