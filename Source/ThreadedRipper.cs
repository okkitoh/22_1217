using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace _22_1217_ {
	public class ThreadedRipper {
		
		int seekPosition = 0;
		int length = 0;
		int BUFSZ = 52428;

		public ThreadedRipper() {
		}

		public ThreadedRipper(string pathname) {
			byte[] BUF = new byte[BUFSZ];
			FileStream fileStream = new FileStream(pathname, FileMode.Open, FileAccess.Read, FileShare.Read);
			fileStream.Read(BUF, seekPosition, BUFSZ);
			
			string chunk = System.Text.Encoding.ASCII.GetString(BUF);
			int chunkSeek = 0;
			string? token = _getNextToken(chunk, ref chunkSeek);
			Dictionary<string, int> frequency = new Dictionary<string, int>();
			while(token != null) {
				if(frequency.ContainsKey(token)) {
					frequency[token] = frequency[token] + 1;
				} else {
					frequency.Add(token, 1);
				}
				token = _getNextToken(chunk, ref chunkSeek);
			}


			frequency.AsParallel().ForAll(entry => Console.WriteLine(entry.Key + " - " + entry.Value));
			//ThreadPool.QueueUserWorkItem(RipTextThread);
		}

		static void RipTextThread(object? state) {
			// Each thread begins tokenizing the chunk of bytes it receives and updating a shared frequency count object 
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
			if(startIndex >= buf.Length) {
				return null;
			}
			int seek = startIndex;
			string? retVal = null;
			while(seek < buf.Length) {
				char c = buf[seek];
				if(!isAlphaNum(c)) {
					int peek = seek + 1;
					// peek doesn't land in EOF
					// not the first character examined
					// peek is alpha-numerical
					if(c == '\'' &&  peek < buf.Length && seek != startIndex && isAlphaNum(buf[peek])) {
						seek++; continue; // include in token and continue to next character

					} else if(seek == startIndex) {
						startIndex = ++seek; // advance token start mark, eating up symbol
						continue;
					} else {
						break; // signal token is ready to be cut out
					}

				}
				seek++;
			}

			retVal = buf.Substring(startIndex, seek - startIndex);
			startIndex = seek;
			if(retVal.Length == 0) {
				return null;
			}
			return retVal;
		}
		
		public static bool isAlphaNum(char c) {
			return !(c < 48 || c > 57 && c < 65 || c > 90 && c < 97 || c > 122);
		}
	}
}
