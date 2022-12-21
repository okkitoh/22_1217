﻿using _22_1217_;
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
			Task<Dictionary<string, int>> task = tr.GetWordFrequency();
			task.Wait();
			Dictionary<string, int> dict = task.Result;
			int wc = dict.AsParallel().Aggregate(0, (acc, kv) => {
				return acc + kv.Value;
			});
			//dict.AsParallel().ForAll((kv) => Console.WriteLine(kv.Key + ": " + kv.Value));
			Console.Write("word count: " + wc);
		}
	}
}