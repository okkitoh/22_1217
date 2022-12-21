using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace _22_1217_ {
	public class ThreadedRipper {
		const int BUFSZ = (1024*1024)/2;
		FileStream? fHandle;
		Dictionary<string, int> frequency = new Dictionary<string, int>();
		
		public ThreadedRipper() {
			fHandle = null;
		}
		public ThreadedRipper(string pathname) {
			fHandle = new FileStream(pathname, FileMode.Open, FileAccess.Read, FileShare.Read);
		}

		public async Task<Dictionary<string, int>> GetWordFrequency() {
			return await Task.Run(() => {
				frequency.Clear();
				if(fHandle == null) {
					return frequency;
				}
					
				Encoding ASCII_ENCODE = new ASCIIEncoding();
				int head = 0;
				int fseek = 0;
				byte[] BUF = new byte[BUFSZ];

				while((fseek = fHandle.Read(BUF, 0, BUFSZ)) > 0) {
					string text = ASCII_ENCODE.GetString(BUF);
					int adjustedLen = fseek;
					if(fseek >= BUFSZ) {
						_getNextRToken(text, ref adjustedLen);
					}
					//ThreadPool.QueueUserWorkItem(RipperWork, text.Substring(0, adjustedLen)); #need a custom AutoResetEvent
					synchronousRipperWork(text.Substring(0, adjustedLen)); /* creating new buffer but this will be necessary for threads */
					head = head + adjustedLen;
					fHandle.Position = head;
					BUF = new byte[BUFSZ];
				}
				return frequency;
			});
		}


		private void synchronousRipperWork(string text) {
			int tseek = 0;
			string? token = _getNextToken(text, ref tseek);
			while (token != null) {
				tallyWord(token);
				token = _getNextToken(text,ref tseek);
			}
		}
		private void tallyWord(string token) {
			if(this.frequency.ContainsKey(token)) {
				this.frequency[token]+= 1;
			} else {
				this.frequency[token] = 1;
			}
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
		private string? _getToken(string buf, ref int startIndex, int direction) {
			if(startIndex > buf.Length || startIndex < 0) {
				return null;
			}

			int seek = (direction < 0 ? startIndex - 1 : startIndex);
			startIndex = (direction < 0 ? seek : startIndex);
			string? retVal = null;
			while(seek >= 0 && seek < buf.Length) {
				char c = buf[seek];
				if(!isAlphaNum(c)) {
					int peek = seek + direction;
					// peek doesn't land in EOF
					// not the first character examined
					// peek is alpha-numerical
					// forgot about hypens, follows same rule
					if((c == '\'' || c == '-') &&  (peek >= 0 && peek < buf.Length) && seek != startIndex && isAlphaNum(buf[peek])) {
						seek += direction; continue; // include in token and continue to next character
					} else if(seek == startIndex) {
						seek += direction;
						startIndex += direction; // advance token start mark, eating up symbol
						continue;
					} else {
						break; // signal token is ready to be cut out
					}
				}
				seek += direction;
			}
			if(direction > 0) {
				retVal = buf.Substring(startIndex, seek - startIndex);
			} else {
				retVal = buf.Substring(seek + 1, startIndex - seek);
			}
			startIndex = seek;
			return (retVal == null || retVal.Length == 0 ? null : retVal);
		}

		private string? _getNextToken(string buf, ref int startIndex) {
			return _getToken(buf, ref startIndex, 1);
		}
		private string? _getNextRToken(string buf, ref int startIndex) {
			return _getToken(buf, ref startIndex, -1);
		}
		
		public static bool isAlphaNum(char c) {
			return !(c < 48 || c > 57 && c < 65 || c > 90 && c < 97 || c > 122);
		}
	}
}
