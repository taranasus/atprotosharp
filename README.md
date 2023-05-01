# aTproto - Work in progress#

3rd Party C# library wrapper around the authenticated transfer protocol server API developed by https://github.com/bluesky-social/atproto

Understanding the protocol https://atproto.com/guides/overview

---

Currently it can do a few things like Login, Post, Get the timeline but it's not even close to wide-adoption use.

Here's a breakdown of the projects

- atporotsharp - This is the API wrapper that will eventually become a nuget package
- DevConsole - Improperly named C# console project I'm using to test the wrapper. On top of testing, its intended use is to act as a CLI for BSKY, allowing you to interact with the social network in that way.

If you want to have some fun, you can clone this repo, compile the DevConsole and once it's running type "help" and press enter to see all the things you can currently do with it
