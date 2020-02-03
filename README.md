# HackerNewsCLI

This is a command line program that gets hacker news posts and returns them in this string JSON Format

```
[
  {
    "Title": "Tips from Poland on Old-School Zero Waste (2019)",
    "Uri": "https://culture.pl/en/article/tips-from-poland-on-old-school-zero-waste",
    "Author": "ericdanielski",
    "Points": "60",
    "comments": "42",
    "rank": "1"
  }
]
```
**Dependencies:**
- Newtonsoft.Json: Makes formatting JSON a lot easier https://www.newtonsoft.com/json
- .Net Core 3.1: I wrote it in this because I just wanted to get better at it https://dotnet.microsoft.com/download
- Visual Studio Code: Its a good editor and makes it easier to deal with dependencies https://code.visualstudio.com/download

**How to Run:**
- Make sure you have the dependencies above installed
- Clone the repo into a local folder on your PC
- Open the folder with Visual Studio Code
- If it says "Required assets to build and debug are missing. Add them?", click **Yes**
- Open the command line termminal. Terminal -> New Terminal
- Make sure you're in the HackerNewsConsole directory in the terminal
- Run the program using the command ```dotnet run --posts 10``` and that will give you the first 10 posts on hacker news in the console.

-To Run tests, go to the HackerNewsConsoleTests directory in the terminal and run ```dotnet tests```
