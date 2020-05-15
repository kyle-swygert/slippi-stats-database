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
            addColumns();
            populateCharacterListBox();
            populateDatesListBox();
            populateTagsListBox();
        }


        // NOTE: Can calculate match duration from numofframes
        // ex) 17284 frames @ 60 fps is 4:48 (only calculate the minutes and seconds

        private string testDBConnStr = "Host = localhost; Username = postgres; Database = SlippiTest; password = admin";

        private string slippiStatsConnStr = "Host = localhost; Username = postgres; Database = SlippiStats; password = admin";





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

        private void addColumns()
        {
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


        }

        private void addGridRow(NpgsqlDataReader reader)
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

            // add the dates to the query

            // add the tags to the query

            // add the characters to the query

            // add all the checkboxes to the query. 




            string fullQuery = $"select matchid, stagename, stageid, matchdate, gametype, numofframes, filename from (match natural join character_played_in_match natural join character) charinmatch {searchCond} group by matchid;";



            return null;
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
            string tagQuery = "select distinct(tag) from character;";

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


            execQuery(allMatch, addGridRow);

            NumMatchesLabel.Content = "# of Matches: " + MatchGrid.Items.Count;

        }

        private void MatchGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // a new row in the match grid was selected. 

            // display basic information below the grid. 


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

            if (dates.Count > 1)
            {

                foreach (string date in dates)
                {

                    allDates += $" date(matchdate)='{date}' or ";


                }

                // remove the final ' or ', or the last 4 characters from the string. 

                allDates = allDates.Substring(0, allDates.Length - 4);


                // insert into a where parens block. 
                allDates = $" where ( {allDates} ) ";

            } else if (dates.Count == 1) 
            {
                // there is only a single date, build the part of the query with a single query. no need to add the ' or ' sections. 

                allDates = $" where ( date(matchdate)='{dates[0]}' ) ";



            } else if (dates.Count == 0)
            {
                // Dont add the 'where ()' part to the string at all. the query will not search by the dates.
                // nothing should be added to the string here. 


            }



            // if there is at least one date, insert the previous string into ' where ( {dates} )



            string updatedTagsQuery = $" select distinct(tag) from(match natural join character_played_in_match natural join character) charinmatch {allDates} ;";
            
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



            // gather all the dates that are selected. 

            // build the string of all selected dates.




            // gather all the tags that are selected. 

            // build the string of all the selected tags. 



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
    }
}
