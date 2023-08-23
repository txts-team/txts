# Txts

*A faithful recreation of the txti.es service in ASP.NET*

## What is Txts?

Txts allows users to create markdown-based simple web pages. Most of the world still does not have internet, but many
websites from countries like the United States are big and complicated. This makes it hard for people with slow internet
to use these sites. It is even harder for those people to put their own thoughts on the internet. With Txts, anyone can
use any device to share their story.

## Need help?

Join our [Discord](https://discord.gg/Y5QfmF9uW3) for support.

## Security

Txts should be considered pre-release software. While we generally try to look out for security issues before pushing code, issues are bound to arise.

Simply put, you should use Txts **at your own risk**.

## Building and running

You'll need the .NET 7.0 SDK to build and run Txts.

**Testing locally:**
```bash
git clone https://github.com/sudokoko/txts.git
cd txts
dotnet run -c Release --project txts
```

**Publishing and running:**
```bash
git clone https://github.com/sudokoko/txts.git
cd txts
dotnet publish -c Release --project txts
cd ./txts/bin/Release/net7.0/publish
./txts
```
