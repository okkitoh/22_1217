using _22_1217_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace _22_1217_ {
	class MainProgramme {
		static void Main(string[] args) {

			string path = Directory.GetCurrentDirectory();
			path = path.Substring(0, path.IndexOf("Source\\")) + "Resources\\WarAndPeace.txt";
			ThreadedRipper tr = new ThreadedRipper(path);
			ThreadedRipper tr2 = new ThreadedRipper(path);

			Task<Dictionary<string, int>> task = tr.GetWordFrequency();
			Task<Dictionary<string, int>> task2 = tr2.GetWordFrequency();

			Task.WhenAll(task, task2);
			
			
			Dictionary<string, int> dict = task.Result;
			int wc = dict.AsParallel().Aggregate(0, (acc, kv) => {
				return acc + kv.Value;
			});
			Console.WriteLine("word count: " + wc);
			
			Dictionary<string, int> dict2 = task.Result;
			wc = dict2.AsParallel().Aggregate(0, (acc, kv) => {
				return acc + kv.Value;
			});
			Console.WriteLine("word count: " + wc);
		}
	}
}