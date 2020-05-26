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

            initMatchSearchTab();

            initCharMatchupTab();

            initCharOverallTab();

        }


        // NOTE: Can calculate match duration from numofframes
        // ex) 17284 frames @ 60 fps is 4:48 (only calculate the minutes and seconds

        private string testDBConnStr = "Host = localhost; Username = postgres; Database = SlippiTest; password = admin";

        private string slippiStatsConnStr = "Host = localhost; Username = postgres; Database = SlippiStats; password = admin";


        private Dictionary<string, string> stockIcons;


        // TODO: Find stock icons (maybe from Slippi Desktop app) and populate the Dict with the names of chars and filenames for the icons. 
        private void initStockIcons()
        {
            stockIcons = new Dictionary<string, string>();

            // populate the dictionary with character names and stock icon images to be used in the character listbox. 

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


            string testDivWRemainder = @"
select matchid, stagename, stageid, matchdate, gametype, numofframes, filename from 
(select charinmatch.matchid
from character_played_in_match as charinmatch, match as m, character as mychar
where mychar.charid = charinmatch.charid
 and mychar.charname in ('CAPTAIN_FALCON', 'MARTH')
group by charinmatch.matchid
having (count(charinmatch.charid) >= (select count(charid) from character ))
) as subresult 
natural join match natural join character_played_in_match natural join character
where gametype='Singles'
group by matchid, stagename, stageid, matchdate, gametype, numofframes, filename
having count( distinct( charname)) = 2
";




            // TODO: This has been edited for testing, change back when finished testing!!!
            execQuery(fullQuery, addMatchGridRow);


            //execQuery(testDivWRemainder, addMatchGridRow);


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
            //TagsListBox.SelectedItems.Clear();
            TagsListBox.SelectedIndex = -1;
        }

        private void ClearCharsButton_Click(object sender, RoutedEventArgs e)
        {
            //CharListBox.SelectedItems.Clear();
            CharListBox.SelectedIndex = -1;
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
            TagsListBox.SelectedIndex = -1;
            CharListBox.SelectedIndex = -1;

            CharInfoGrid.Items.Clear();

            
        }

        private void CharInfoGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // calculate matchup stats button clicked. 


            if (Char1ComboBox.SelectedItem.ToString() == Char2ComboBox.SelectedItem.ToString())
            {
                // the user is trying to find stats for a ditto, which will not be calculated properly. 

                // TODO: Create a popup window saying to choose 2 different characters to compute stats for. 



            } else
            {
                // execute the query and calculate the proper stats for the selected characters

                CalculateStatsButtonClicked();

            }




        }

        private void Char2StageWinsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
