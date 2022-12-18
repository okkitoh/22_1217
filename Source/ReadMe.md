December 17, 2022
The air is cold and nice.

A word frequency counter
Sample text, War & Peace by Tolstoy


Module Specifications:
> At the core, the module is a tokenizer deciding which combination of characters have meaning. The goal is
  to design it in a way that an arbitrarily large file could be read
> How to load massive files and take big chunks from text?
	I. Chunk size should be configurable
	II. Assumption that I/O is lamentably slow and space is limited
	III. Having an expected format for the chunk could make processing simpler later on down the line.
		Can we format the chunk without significant thread safety issues or performance hit?

	II. Constraints
		For the moment, tokens are defined as:
		- Pure ASCII
		- All non alphanumeric symbols converted to space(32) unless
		- If midtoken and encountered symbol \' and proceeded by alphanumeric, dont convert \' to space(32)
		- Tokens are delimited by symbols
	III.  Keep ordered dictionary <string, int> behind mutex lock to store frequency counts 
		
