using _22_1217_;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ThreadedRipperUnitTests {
	[TestClass]
	public class TokenizeTests {
		[TestMethod]
		public void TestBench() {
			ThreadedRipper tr = new ThreadedRipper();
			PrivateObject po = new PrivateObject(tr);
			int startIndex = 0;
			string INPUT = "*'a we're O'hare 198a2       'ave 'snow' eves'     ";

			object[] args = new object[] { INPUT, startIndex };
			string? token = (string?)po.Invoke("_getNextToken", args);
			while(token != null) {
				if( token == null ) {
					Console.WriteLine("null");
				} else {
					Console.WriteLine( "HEAD " + args[1] + " -- " + token);
				}
				token = (string?)po.Invoke("_getNextToken", args);
			}
		}
	}
}