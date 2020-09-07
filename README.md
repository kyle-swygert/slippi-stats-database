# Welcome to Slippi Stats!
Slippi Stats is a Database application to calculate and analyze stats from .slp files generated from [Project Slippi](https://github.com/project-slippi/project-slippi).

## Technologies used:
- [Python](https://www.python.org/): to populate the Database from the .slp files.
- [py-slippi](https://github.com/hohav/py-slippi): Python library used to parse .slp files to populate the database. 
- [C#](https://docs.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/): main language used for application. Chosen for usability and WPF for GUI design. 
- [IronPython](https://ironpython.net/): to execute the Python script to populate the Database.
- [Visual Studio](https://visualstudio.microsoft.com/): main development environment for code and WPF GUI designing.
- [PostgreSQL](https://www.postgresql.org/): SQL DBMS and SQL dialect used for database design.
- [Npgsql 3.2.7](https://www.npgsql.org/): used to connect C# application to Database for queries. 

## Match Search
- On the Match Search tab of the app, you can search for a specific match based on the Date the match was played, the Tag that was used in a match, the Characters that played in a match, the Game Type, Team Color, and even Stage played on in the match. 
- Currently you can only see where the match is located with the file path of the file, but with this you can open the information for the file with the Slippi Desktop App. Integration with the Slippi Desktop App is planned for near future implementation. 

## Character Vs. Character
- With the Char Vs. Char tab of the app, you can see how well one character does against another. The overall winrate for each character is shown, as well as the winrates for each character on the tournament legal stages. 

## Character Overall Stats
- The Char Overrall Stats tab displays all sorts of information about a selected character. Stats include win rates for tournament legal stages, matchup win rates against opponent characters, Tags used by this character, and the frequency of colors used by this character.

## Add your own .slp files to be analyzed
- Currently working on a way to package this application so that other users can download the app and insert their own .slp files to be stored and analyzed. 

## Future Plans
- A long running plan for this project is to be able to link this application with the Slippi Desktop App to be able to search for a match in Slippi Stats, then open the replay file seamlessly with the Slippi Desktop App for simple viewing of the file. 

## Thank you for checking out Slippi Stats!
- If you enjoyed learning about Slippi Stats, want to know more, or are interested in helping develop Slippi Stats, send me an email and we can talk more about it!


![Slippi Logo](slippi-logo.png)