using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

// below using statements added for the project. 
using Npgsql;

namespace Slippi_Stats_Database_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public class Match
        {
            public string matchID { set; get; }
            public string stageName { set; get; }
            public int stageID { set; get; }
            public DateTime matchDate { set; get; }
            public string gameType { set; get; }
            public int numOfFrames { set; get; }
            public string fileName { set; get; }
        }

        public class GameCharacter
        {
            public string charName { set; get; }
            public string charID { set; get; }
            public int colorInt { set; get; }
            public bool didWin { set; get; }
            public string teamColor { set; get; }
            public string tag { set; get; }
            public int portNumber { set; get; }
        }

        // NOTE: Initialize the app on startup inside this method. 
        public MainWindow()
        {
            InitializeComponent();
            addMatchColumns();
            addCharColumns();
            populateCharacterListBox();
            populateDatesListBox();
            populateTagsListBox();
            initCheckBoxes();
        }


        // NOTE: Can calculate match duration from numofframes
        // ex) 17284 frames @ 60 fps is 4:48 (only calculate the minutes and seconds

        private string testDBConnStr = "Host = localhost; Username = postgres; Database = SlippiTest; password = admin";

        private string slippiStatsConnStr = "Host = localhost; Username = postgres; Database = SlippiStats; password = admin";

        private List<CheckBox> teamsCheckBoxes;
        private List<CheckBox> stagesCheckBoxes;
        private List<CheckBox> gameTypeCheckBoxes;

        private Dictionary<string, string> stockIcons;


        // TODO: Find stock icons (maybe from Slippi Desktop app) and populate the Dict with the names of chars and filenames for the icons. 
        private void initStockIcons()
        {
            stockIcons = new Dictionary<string, string>();

            // populate the dictionary with character names and stock icon images to be used in the character listbox. 

        }


        private void initCheckBoxes()
        {
            // add all the check boxes to their own separate lists. 

            teamsCheckBoxes = new List<CheckBox>();

            teamsCheckBoxes.Add(TeamBlueCB);
            teamsCheckBoxes.Add(TeamGreenCB);
            teamsCheckBoxes.Add(TeamRedCB);


            stagesCheckBoxes = new List<CheckBox>();

            stagesCheckBoxes.Add(BattlefieldCB);
            stagesCheckBoxes.Add(FoDCB);
            stagesCheckBoxes.Add(FinalDestCB);
            stagesCheckBoxes.Add(PokemonStadiumCB);
            stagesCheckBoxes.Add(YoshiStoryCB);
            stagesCheckBoxes.Add(DreamlandCB);


            gameTypeCheckBoxes = new List<CheckBox>();

            gameTypeCheckBoxes.Add(SinglesCB);
            gameTypeCheckBoxes.Add(TeamsCB);
            gameTypeCheckBoxes.Add(FreeForAllCB);

        }

        private void execQuery(string sqlstr, Action<NpgsqlDataReader> myr)
        {

            // NOTE: Change the line below when using the real DB, rather than the test DB. 
            using (var connection = new NpgsqlConnection(slippiStatsConnStr))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sqlstr;
                    try
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            myr(reader);
                        }
                    }
                    catch (NpgsqlException ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                        System.Windows.MessageBox.Show("SQL Error - " + ex.Message.ToString());
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        private void addMatchColumns()
        {

            // Add columns for Match Grid

            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "Stage Name";
            col1.Binding = new Binding("stageName");
            col1.Width = 200;
            MatchGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "Match ID";
            col2.Binding = new Binding("matchID");
            col2.Width = 200;
            MatchGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "File Name";
            col3.Binding = new Binding("fileName");
            col3.Width = 200;
            MatchGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = "Match Date";
            col4.Binding = new Binding("matchDate");
            col4.Width = 200;
            MatchGrid.Columns.Add(col4);

            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Header = "Game Type";
            col5.Binding = new Binding("gameType");
            col5.Width = 200;
            MatchGrid.Columns.Add(col5);


        }

        private void addCharColumns()
        {

            // Add columns for CharInfoGrid

            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "Character Name";
            col1.Binding = new Binding("charName");
            col1.Width = 150;
            CharInfoGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "Tag";
            col2.Binding = new Binding("tag");
            col2.Width = 50;
            CharInfoGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "Team";
            col3.Binding = new Binding("teamColor");
            col3.Width = 50;
            CharInfoGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = "Won Match";
            col4.Binding = new Binding("didWin");
            col4.Width = 50;
            CharInfoGrid.Columns.Add(col4);

            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Header = "Port #";
            col5.Binding = new Binding("portNumber");
            col5.Width = 50;
            CharInfoGrid.Columns.Add(col5);



        }

        private void addMatchGridRow(NpgsqlDataReader reader)
        {

            MatchGrid.Items.Add(new Match()
            {
                matchID = reader.GetString(0).TrimEnd(),
                stageName = reader.GetString(1).TrimEnd(),
                stageID = reader.GetInt32(2),
                matchDate = reader.GetDateTime(3),
                gameType = reader.GetString(4),
                numOfFrames = reader.GetInt32(5),
                fileName = reader.GetString(6).TrimEnd()

            });


        }

        private void addCharInfoGridRow(NpgsqlDataReader reader)
        {

            CharInfoGrid.Items.Add(new GameCharacter()
            {
                charID = reader.GetString(0),
                charName = reader.GetString(1),
                colorInt = reader.GetInt32(2),
                didWin = reader.GetBoolean(3),
                teamColor = reader.GetString(4),
                tag = reader.GetString(5),
                portNumber = reader.GetInt32(6)
            });


        }


        private string GenerateMatchQuery()
        {
            // This method will generate a query based on all the selected items on the GUI of the app to display the proper data to the screen. 

            // reference the GenerateBusinessQuery() from Yelp DB for help.

            /*
             
             NOTE: This is the base query to use to populate the app with the correct data for a match object. 
             
            select matchid, stagename, stageid, matchdate, gametype, numofframes, filename
            from (match natural join character_played_in_match natural join character) charinmatch
            group by matchid
             
            select matchid, stagename, stageid, matchdate, gametype, numofframes, filename
            from (match natural join character_played_in_match natural join character) charinmatch

            where {add conditions for searching here}

            group by matchid
             
             */


            string searchCond = "";

            List<string> searchSections = new List<string>();

            string allDates = GenerateDatesStr();
            string allChars = GenerateCharsStr();
            string allTags = GenerateTagsStr();

            // add all the search sections into the list
            searchSections.Add(GenerateCharsStr());
            searchSections.Add(GenerateDatesStr());
            searchSections.Add(GenerateTagsStr());
            searchSections.Add(GenerateStagesStr());
            searchSections.Add(GenerateTeamsStr());
            searchSections.Add(GenerateGameTypesStr());


            // build the search condition from the list. 
            foreach( string section in searchSections)
            {

                // only add section if it is not empty. 
                if (section.Length > 0)
                {

                    searchCond += $" {section} and ";

                }

            }



            // check if the serachCond is empty or not. 
            // if empty, dont put in a where block. Don't do anything actually...
            // if not empty, remove final ' and ' and  put in a where block. 
            
            if (searchCond.Length > 0)
            {

                searchCond = $" where ( {searchCond.Substring(0, searchCond.Length - 5)} ) ";

            }







            // add the dates to the query

            // add the tags to the query

            // add the characters to the query

            // add all the checkboxes to the query. 

            

            string fullQuery = $"select matchid, stagename, stageid, matchdate, gametype, numofframes, filename from (match natural join character_played_in_match natural join character) charinmatch {searchCond} group by matchid;";

           

            return fullQuery;
        }

        private void populateCharacterListBox()
        {
            string charQuery = "select distinct(charname) from character;";

            execQuery(charQuery, addCharacter);

        }


        private void addCharacter(NpgsqlDataReader reader)
        {

            // only add the string to the listbox. 
            CharListBox.Items.Add(reader.GetString(0).TrimEnd());

        }

        private void populateTagsListBox()
        {
            string tagQuery = "select distinct(tag) from character order by tag asc;";

            execQuery(tagQuery, addTag);

        }

        private void addTag(NpgsqlDataReader reader)
        {
            // only add the string to the listbox. 
            TagsListBox.Items.Add(reader.GetString(0));

        }

        private void populateDatesListBox()
        {
            string dateQuery = "select distinct(date(matchdate)) from match order by date(matchdate) ;";

            execQuery(dateQuery, addDate);

        }

        private void addDate(NpgsqlDataReader reader)
        {
            // only add the string to the listbox. 
            DatesListBox.Items.Add(reader.GetDateTime(0));


        }

        private string GenerateDatesStr()
        {

            int dateCount = 0;

            string allDates = "";

            List<string> dates = new List<string>();

            // gather dates in a list

            foreach (DateTime singleDate in DatesListBox.SelectedItems)
            {

                // this line splits the string representing the date by the space character and gets the first substring that represents the date only. 
                dates.Add(singleDate.Date.ToString().Split(' ')[0]);

                dateCount += 1;
            }


            // create a string where there is an ' or ' inserted between the dates

            //if (dates.Count > 1)
            //{

                foreach (string date in dates)
                {

                    allDates += $" date(matchdate)='{date}' or ";


                }

                // remove the final ' or ', or the last 4 characters from the string. 

            if (allDates.Length > 0)
            {

                allDates = $" ( {allDates.Substring(0, allDates.Length - 4)} ) ";

            }


            // insert into a where parens block. 
            //allDates = $" where ( {allDates} ) ";

            //}
            //else if (dates.Count == 1)
            //{
            //    // there is only a single date, build the part of the query with a single query. no need to add the ' or ' sections. 

            //    allDates = $" where ( date(matchdate)='{dates[0]}' ) ";



            //}
            //else if (dates.Count == 0)
            //{
            //    // Dont add the 'where ()' part to the string at all. the query will not search by the dates.
            //    // nothing should be added to the string here. 


            //}

            return allDates;


        }

        private string GenerateTagsStr()
        {

            int tagCount = 0;

            string allTags = "";

            List<string> tags = new List<string>();

            // gather dates in a list

            foreach (string singleTag in TagsListBox.SelectedItems)
            {

                // this line splits the string representing the date by the space character and gets the first substring that represents the date only. 
                tags.Add(singleTag);

                tagCount += 1;
            }


            // create a string where there is an ' or ' inserted between the dates

            //if (tags.Count > 1)
            //{

                foreach (string tag in tags)
                {

                    allTags += $" tag='{tag}' or ";
                    
                }

            // remove the final ' or ', or the last 4 characters from the string. 
            if (allTags.Length > 0)
            {

                allTags = $" ( {allTags.Substring(0, allTags.Length - 4)} ) ";

            }



            // insert into a where parens block. 
            //allTags = $" where ( {allTags} ) ";

            //}
            //else if (tags.Count == 1)
            //{
            //    // there is only a single date, build the part of the query with a single query. no need to add the ' or ' sections. 

            //    allTags = $" where ( tag='{tags[0]}' ) ";



            //}
            //else if (tags.Count == 0)
            //{
            //    // Dont add the 'where ()' part to the string at all. the query will not search by the tags.
            //    // nothing should be added to the string here. 


            //}

            return allTags;


        }

        private string GenerateCharsStr()
        {

            int charCount = 0;

            string allChars = "";

            List<string> chars = new List<string>();

            // gather chars in a list

            foreach (string singleChar in CharListBox.SelectedItems)
            {

                // this line splits the string representing the date by the space character and gets the first substring that represents the date only. 
                chars.Add(singleChar);

                charCount += 1;
            }


                foreach (string character in chars)
                {

                    allChars += $" charname='{character}' or ";
                    
                }

            // remove the final ' or ', or the last 4 characters from the string. 
            if (allChars.Length > 0)
            {

                allChars = $" ( {allChars.Substring(0, allChars.Length - 4)} ) ";

            }


            return allChars;


        }

        // TODO: implement the below methods. 
        // all these methods should return the section of the where clause inside a set of ( parens ) to make processing simpler for other methods. 

        private string GenerateStagesStr()
        {

            string allStages = "";
                
            foreach (CheckBox stage in stagesCheckBoxes)
            {

                if (stage.IsChecked == true)
                {
                    // use the content of the checkbox after some manipulation in the query. 
                    allStages += $" stagename='{stage.Content.ToString().Replace("'", string.Empty).Replace(" ", "_").ToUpper()}' or ";

                }

            }

            if (allStages.Length > 0)
            {
                // remove the final ' or ' block and put into parens. 
                allStages = $" ( {allStages.Substring(0, allStages.Length - 4)} ) ";
            }

            return allStages;
        }

        private string GenerateGameTypesStr()
        {
            string allGameTypes = "";

            foreach (CheckBox gameType in gameTypeCheckBoxes)
            {

                if (gameType.IsChecked == true)
                {
                    // use the content of the checkbox after some manipulation in the query. 
                    //allGameTypes += $" stagename='{gameType.Content.ToString().Replace("'", string.Empty).Replace(" ", "_").ToUpper()}' or ";

                    if (gameType.Content.ToString() == "Singles" || gameType.Content.ToString() == "Teams")
                    {

                        allGameTypes += $" gametype='{gameType.Content.ToString()}' or ";

                    } else
                    {

                        allGameTypes += " gametype='FFA' or ";

                    }

                }

            }

            if (allGameTypes.Length > 0)
            {
                // remove the final ' or ' block and put into parens. 
                allGameTypes = $" ( {allGameTypes.Substring(0, allGameTypes.Length - 4)} ) ";
            }

            return allGameTypes;
        }

        private string GenerateTeamsStr()
        {
            string allTeams = "";

            foreach (CheckBox team in teamsCheckBoxes)
            {

                if (team.IsChecked == true)
                {
                    // use the content of the checkbox after some manipulation in the query. 
                    //allTeams += $" stagename='{stage.Content.ToString().Replace("'", string.Empty).Replace(" ", "_").ToUpper()}' or ";

                    allTeams += $" team='{team.Content.ToString().Split(' ')[0]}' or ";

                }

            }

            if (allTeams.Length > 0)
            {
                // remove the final ' or ' block and put into parens. 
                allTeams = $" ( {allTeams.Substring(0, allTeams.Length - 4)} ) ";
            }

            return allTeams;
        }
        

        // NOTE: all the below methods are for the UI Click actions in the GUI of the application. 

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // add files to database button was just clicked. 

            // use an OpenFileDialog to select a directory

            // use the selected directory to then call the python code to populate the database recursively through the directory tree. 

            // NOTE: While the database population is occuring, disable the main window, popup a small window that says "Database Population in progress" with a loading bar of some sort. add a cancel button to the popup
            // when population has finished or been cancelled, re-enable the main window and remove the popup window. 

            Console.Write("Add Files to DB Button clicked.");



        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // query the database with the values of items on the GUI of the app. 


            /*
             
             NOTE: This is the base query to use to populate the app with the correct data for a match object. 
             
            select matchid, stagename, stageid, matchdate, gametype, numofframes, filename
            from (match natural join character_played_in_match natural join character) charinmatch
            group by matchid
             
             
             */



            // NOTE: This is a simple query used for TESTING PURPOSES ONLY!!! Remove after testing. 

            string allMatch = "select matchid, stagename, stageid, matchdate, gametype, numofframes, filename from(match natural join character_played_in_match natural join character) charinmatch group by matchid;";

            string fullQuery = GenerateMatchQuery();


            MatchGrid.Items.Clear();


            execQuery(fullQuery, addMatchGridRow);

            NumMatchesLabel.Content = "# of Matches: " + MatchGrid.Items.Count;

        }

        private void MatchGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // a new row in the match grid was selected. 

            // display basic information below the grid. 

            CharInfoGrid.Items.Clear();

            // query the database for the characters that played in the current selected match. 

            Match currSelection = (sender as DataGrid).SelectedItem as Match;

            if (currSelection != null)
            {

                string charsInMatchQuery = $" select * from (character natural join character_played_in_match) charmatch where matchid='{currSelection.matchID}';";

                execQuery(charsInMatchQuery, addCharInfoGridRow);

            }



        }

        private void DatesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // when the dates that are selected changes, update the tag listbox and the character listbox accordingly. 

            /*
             
             select distinct(tag) 
            from (match natural join character_played_in_match natural join character) charinmatch
            where ( date(matchdate)='2020-02-06' or date(matchdate)='2020-02-27' );
             
             */


            // clear the character listbox and the tag listbox

            CharListBox.Items.Clear();
            TagsListBox.Items.Clear();


            string allDates = GenerateDatesStr();

            
            if (allDates.Length > 0)
            {

                allDates = $" where {allDates} ";

            }


            string updatedTagsQuery = $" select distinct(tag) from(match natural join character_played_in_match natural join character) charinmatch {allDates} order by tag asc ;";
            
            execQuery(updatedTagsQuery, addTag);


            /*

           select distinct(charname) 
          from (match natural join character_played_in_match natural join character) charinmatch
          where ( date(matchdate)='2020-02-06' or date(matchdate)='2020-02-27' );

           */


            string updatedCharsQuery = $" select distinct(charname) from(match natural join character_played_in_match natural join character) charinmatch {allDates} ;";

            execQuery(updatedCharsQuery, addCharacter);


        }

        private void TagsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // when the tags that are selected is changed, update the character listbox accordingly. 
            // the characters in the charListBox must have the date and the tags with them 

            CharListBox.Items.Clear();

            string allDates = GenerateDatesStr();
            string allTags = GenerateTagsStr();

            string conditions = "";


            if (allDates.Length == 0 && allTags.Length == 0)
            {
                // both the chars and tags are empty, add nothing. 


            } else if (allDates.Length > 0 && allTags.Length == 0)
            {
                // only chars
                conditions = $" where {allDates} ";

            } else if (allDates.Length == 0 && allTags.Length > 0)
            {
                // only tags
                conditions = $" where {allTags} ";

            } else
            {
                // include both chars and tags. 
                // NOTE: should these be connected with the or or the and operator????
                conditions = $" where ( {allTags} and {allDates} ) ";

            }



            string updatedCharsQuery = $" select distinct(charname) from(match natural join character_played_in_match natural join character) charinmatch {conditions} ;";

            execQuery(updatedCharsQuery, addCharacter);
            
            // NOTE: remmeber to check for a count of zero for both the dates and the tags. 



            // build the query to be run on the database. 

        }

        private void CharListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // when the character listbox selections are changed, nothing is to be updated. 


        }

        private void ClearDatesButton_Click(object sender, RoutedEventArgs e)
        {
            DatesListBox.SelectedItems.Clear();
        }

        private void ClearTagsButton_Click(object sender, RoutedEventArgs e)
        {
            TagsListBox.SelectedItems.Clear();
        }

        private void ClearCharsButton_Click(object sender, RoutedEventArgs e)
        {
            CharListBox.SelectedItems.Clear();
        }

        private void ClearAllSelectionsButton_Click(object sender, RoutedEventArgs e)
        {
            // clear all the selections that are on the screen.

            foreach (CheckBox stage in stagesCheckBoxes)
            {
                stage.IsChecked = false;
            }

            foreach (CheckBox gametype in gameTypeCheckBoxes)
            {
                gametype.IsChecked = false;
            }

            foreach (CheckBox team in teamsCheckBoxes)
            {
                team.IsChecked = false;
            }

            DatesListBox.SelectedItems.Clear();
            TagsListBox.SelectedItems.Clear();
            CharListBox.SelectedItems.Clear();

            CharInfoGrid.Items.Clear();

            // TODO: reset the radio buttons to their initial position when executed????? I might just want to keep those the same actually. 


        }

        private void CharInfoGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
