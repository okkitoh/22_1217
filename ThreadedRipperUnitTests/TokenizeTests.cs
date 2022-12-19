using _22_1217_;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ThreadedRipperUnitTests {
	[TestClass]
	public class TokenizeTests {
		[TestMethod]
		public void TestBench() {
			ThreadedRipper tr = new ThreadedRipper();
			PrivateObject po = new PrivateObject(tr);
			List<string> EXPECTED = new List<string>() { "a", "we're", "O'hare", "198a2", "lil", "ave", "snow", "eves" };
			List<string> ACTUAL = new List<string>();

			int startIndex = 0;
			string INPUT = "*'a we're O'hare 198a2   lil,'     'ave 'snow' eves'     '";
			object[] args = new object[] { INPUT, startIndex };

			string? token = (string?)po.Invoke("_getNextToken", args);
			while(token != null) {
				ACTUAL.Add(token);
				token = (string?)po.Invoke("_getNextToken", args);
			}
			CollectionAssert.AreEqual(EXPECTED, ACTUAL);
		}
	}
}