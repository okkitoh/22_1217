using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace _22_1217_ {
	public class ThreadedRipper {
		
		int seekPosition = 0;
		int length = 0;
		int BUFSZ = 524288;

		public ThreadedRipper() {
		}

		public ThreadedRipper(string s) {

			//ThreadPool.QueueUserWorkItem(RipTextThread);
		}

		public ThreadedRipper(FileStream file) {

		}


		static void RipTextThread(object? state) {
			// Each thread begins tokenizing the chunk of bytes it receives and updating a shared resource with tally
		}


		private string _chunkToWordBoundary(string input, int BUFSZ) {
			// TODO: 
			// Read from input or file handle at _(seek_position)
			// Get BUFSZ amount of bytes
			// If chunked text length is less than BUFSZ
			//     signal EOF
			//	   ret chunk
			// If chunked text length is equal to BUFSZ
			//     trim last token off end
			//	   update startIndex by adjusted size
			//	   ret chunk
			return "";
		}

		/* FUNCTION: _getNextToken(string, int *)
		 * Tokens are defined as alphanumeric and only containing a \' character if it functions as an apostrophe
		 * apostrophe rules:
		 * if prefixed and suffixed by alphanumerics, treat \' as alphanumeric i.e. O'hare we're eve's
		 * if prefixed or suffixed by symbol, treat \' as symbol i.e. 'snow' 'ave chaos'
		 * RETURN: string?
		 *     null - NO MORE TOKENS
		 *     string - extracted token
		 * POSTCONDITION:
		 *     startIndex is updated with the index where processing left off
		 */
		private string? _getNextToken(string buf, ref int startIndex) {
			// TODO: cleanup
			if(startIndex >= buf.Length) {
				return null;
			}
			int seek = startIndex;
			while(seek < buf.Length) {
				char c = buf[seek];
				if(!isAlphaNum(c)) {
					if(c == '\'') { // only want a lookahead when ' is encountered
						if(seek + 1 >= buf.Length) {
							string retVal = buf.Substring(startIndex, seek - startIndex);
							startIndex = ++seek;
							return retVal; // end processing
						} else if (!isAlphaNum(buf[seek + 1])) {
							string retVal = buf.Substring(startIndex, seek - startIndex);
							startIndex = ++seek;
							return retVal;
						} else {
							if(seek == startIndex) {
								startIndex = ++seek;	
							} else {
								seek++;
							}
						}
					} else if(seek == startIndex) {
						startIndex = ++seek;
					} else {
						string retVal = buf.Substring(startIndex, seek - startIndex);
						startIndex = ++seek;
						return retVal;
					}
				} else {
					seek++;
				}
				
			}
			if(startIndex == seek || seek > startIndex) {
				return null;
			}
			string val = buf.Substring(startIndex, seek - startIndex);
			startIndex = seek;
			return val;
		}
		
		public static bool isAlphaNum(char c) {
			return !(c < 48 || c > 57 && c < 65 || c > 90 && c < 97 || c > 122);
		}
	}
}
