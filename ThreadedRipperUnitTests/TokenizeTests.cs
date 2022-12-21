using _22_1217_;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ThreadedRipperUnitTests {
	[TestClass]
	public class TokenizeTests {
		[TestMethod]
		public void SingleCheckForwards() {
			ThreadedRipper tr = new ThreadedRipper();
			PrivateObject po = new PrivateObject(tr);
			List<string> EXPECTED = new List<string>() {"we're"};
			List<string> ACTUAL = new List<string>();
			
			string INPUT = "we're";
			int startIndex = 0;
			object[] args = new object[] { INPUT, startIndex };

			string? token = (string?)po.Invoke("_getNextToken", args);
			while(token != null) {
				ACTUAL.Add(token);
				token = (string?)po.Invoke("_getNextToken", args);
			}
			CollectionAssert.AreEqual(EXPECTED, ACTUAL);
		}

		[TestMethod]
		public void ComprehensiveCheckForwards() {
			ThreadedRipper tr = new ThreadedRipper();
			PrivateObject po = new PrivateObject(tr);
			List<string> EXPECTED = new List<string>() { "a", "we're", "O'hare", "198a2", "lil", "ave", "snow", "eves" };
			List<string> ACTUAL = new List<string>();

			
			string INPUT = "*'a we're O'hare 198a2   lil,'     'ave 'snow' eves'     '";
			int startIndex = 0;
			object[] args = new object[] { INPUT, startIndex };

			string? token = (string?)po.Invoke("_getNextToken", args);
			while(token != null) {
				ACTUAL.Add(token);
				token = (string?)po.Invoke("_getNextToken", args);
			}
			CollectionAssert.AreEqual(EXPECTED, ACTUAL);
		}

		[TestMethod]
		public void SingleCheckBackwards() {
			ThreadedRipper tr = new ThreadedRipper();
			PrivateObject po = new PrivateObject(tr);
			List<string> EXPECTED = new List<string>() {"we're"};
			List<string> ACTUAL = new List<string>();
			
			string INPUT = "we're";
			int startIndex = INPUT.Length;
			object[] args = new object[] { INPUT, startIndex };

			string? token = (string?)po.Invoke("_getNextRToken", args);
			while(token != null) {
				ACTUAL.Add(token);
				token = (string?)po.Invoke("_getNextRToken", args);
			}
			CollectionAssert.AreEqual(EXPECTED, ACTUAL);
		}

		[TestMethod]
		public void ComprehensiveCheckBackwards() {
			ThreadedRipper tr = new ThreadedRipper();
			PrivateObject po = new PrivateObject(tr);
			List<string> EXPECTED = new List<string>() { "eves", "snow", "ave", "lil", "198a2", "O'hare", "we're",  "a" };
			List<string> ACTUAL = new List<string>();

			string INPUT = "*'a we're O'hare 198a2   lil,'     'ave 'snow' eves'     '";
			int startIndex = INPUT.Length;
			object[] args = new object[] { INPUT, startIndex };

			string? token = (string?)po.Invoke("_getNextRToken", args);
			while(token != null) {
				ACTUAL.Add(token);
				token = (string?)po.Invoke("_getNextRToken", args);
			}
			CollectionAssert.AreEqual(EXPECTED, ACTUAL, "\nACTUAL: {\n\t" + string.Join(",\n\t", ACTUAL.ToArray()) + "\n}\n");
		}
	}
}