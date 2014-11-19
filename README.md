Blockchain Parser
=================

A .NET bitcoin's blockchain parser that reads raw blocks from the **blk?????.dat** files. 

Goals
-----
There are lots of articles, websites and blogs on the internet that display very interesting analysis about bitcoin usage, trends, events an all kind of things based on information available in the blockchain however there are few tools that allow us perform those analysis.

BlockchainParser can parse the blockchain to provide an object model for each block, With that info we are able to do what we need.

+ Tested with .NET  _YES_
+ Tested with Mono  _NO_

How to use?
-----------
With nuget :
> **NO NUGET PACKAGE YET** 

Go on the [nuget website](https://) for more information.

Example
--------

The simplest scenario:

```c#

	public class SillyBlockchainProcessor : Parser
	{
    	private static DateTime First1970 = new DateTime(1970,1,1);
    	private long blockNumber;

    	protected override void ProcessBlock(Block block)
    	{
    	    Console.WriteLine("Processing Blk# {0} mined on {1}" 
    	            , blockNumber++ 
    	            , First1970.AddSeconds(block.TimeStamp));
    	}
	}

```

The previous piece of code shows how to display the block's timestamp for each parsed block.


Blockchain to SQL
-----------------

It is a complete example that shows how to export the whole blockchain into an MS SQL Server Database. Doing this we can execute queries against the database in order to get all kind of valuable information.

![Logo](http://i.imgur.com/lxD2Pc4.png)

Exporting the blockchain can takes several hours, in my case it took 25 hours long for a ~15GB blockchain. The parser is very fast, the sql inserts are not. 

Please, if you find a way to improve the general performance then you can help a lot.


Documentation
-------------
To understand the internal blockchain data structure take a look at the excellent post [How to Parse the Bitcoin BlockChain](http://codesuppository.blogspot.com.ar/2014/01/how-to-parse-bitcoin-blockchain.html) by John Ratcliff's [Code Suppository](http://codesuppository.blogspot.com.ar/) blog 


Development
-----------
BlockchainParser is been developed by [Lucas Ontivero](http://geeks.ms/blogs/lontivero) ([@lontivero](http://twitter.com/lontivero)). 
You are welcome to contribute code. You can send code both as a patch or a GitHub pull request. 

Build Status
------------

### Version 0.0.1
* Initial version

Bitcoin Tip Jar
---------------
If this code helps you, consider helping: 1MV8GizXD2Mbsz2T4LTfqmZV9zpBTA6s2h

 

