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
    public partial class MainWindow : Window
    {

        public class StageWin
        {
            public string stageName { get; set; }
            public double winRate { get; set; }
            public int numTimesPlayedOnStage { get; set; }
        }



        // TODO: Move all of the methods for the Char Vs. Char Tab into this file. 

        private void initCharMatchupTab()
        {
            // this method will initialize all the UI items for this tab. 

            initComboBoxes();

            addStageWinsColumns();

            Char1StageWinsGrid.IsReadOnly = true;
            Char2StageWinsGrid.IsReadOnly = true;

        }


        private void initComboBoxes()
        {
            // populate the combo boxes with all the names of the characters that are in the database

            // create query

            string characterQuery = "select distinct(charname) from character order by charname asc;";

            // execute query and populate the comboboxes

            execQuery(characterQuery, addComboBoxItem);




        }

        private void addStageWinsColumns()
        {

            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "Stage Name";
            col1.Binding = new Binding("stageName");
            col1.Width = 150;
            Char1StageWinsGrid.Columns.Add(col1);
            

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "Win Rate";
            col2.Binding = new Binding("winRate");
            col2.Width = 50;
            Char1StageWinsGrid.Columns.Add(col2);
            

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "Stage Name";
            col3.Binding = new Binding("stageName");
            col3.Width = 150;
            Char2StageWinsGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = "Win Rate";
            col4.Binding = new Binding("winRate");
            col4.Width = 50;
            Char2StageWinsGrid.Columns.Add(col4);

        }

        private void addComboBoxItem(NpgsqlDataReader reader)
        {

            Char1ComboBox.Items.Add(reader.GetString(0).TrimEnd());

            Char2ComboBox.Items.Add(reader.GetString(0).TrimEnd());

        }

        private void addWinRateInformation(NpgsqlDataReader reader)
        {

            // TODO: Check that there is data for the matchup that was selected. IF there is NOT matchup data, do not process this method. 

            // TODO: Check that the win rate percent is being set properly. If the winrate does not have 5 decimal places in it, display as many digits as there are in the number. 


            // check that the reader has any data in it. 

            if (reader.HasRows == false)
            {

                return;

            }

            // if there is no data, then there is no match history between these selected characters. 

            // if there are results, check which listBox to add the current item into. 

            if (reader.GetString(0).TrimEnd() == Char1ComboBox.SelectedItem.ToString())
            {
                // change the label contents

                //Character1Label.Content = reader.GetString(0).TrimEnd().Split('_').ToList<string>();

                // build the proper character name from the all caps and underscore version of the name. 

                List<string> partsOfName = reader.GetString(0).TrimEnd().Split('_').ToList<string>();

                foreach (string part in partsOfName)
                {

                    Character1Label.Content += part[0].ToString() + part.Substring(1).ToLower() + " ";

                }

                double char1WinRate = reader.GetDouble(3);

                if (char1WinRate > 50.0)
                {
                    // character 1 has a higher win rate, set the text color to green
                    Character1Label.Foreground = Brushes.Green;

                }
                else
                {
                    // character 2 has lower winrate, set text color to Red
                    Character1Label.Foreground = Brushes.Red;

                }

                // TODO: Check if the winrate is a whole number or not. If the winrate is a double, then round to 2 decimal places. If the winrate is a whole number, jsut display all the digits. 

                Character1Label.Content += ": " + reader.GetDouble(3).ToString().Substring(0, 5) + "%";

                // change the number of matches to the proper number. 


                CharMatchupNumMatchesLabel.Content = $"# of Matches: {reader.GetInt32(1).ToString()}";

            }
            else if (reader.GetString(0).TrimEnd() == Char2ComboBox.SelectedItem.ToString())
            {
                List<string> partsOfName = reader.GetString(0).TrimEnd().Split('_').ToList<string>();

                foreach (string part in partsOfName)
                {

                    Character2Label.Content += part[0].ToString() + part.Substring(1).ToLower() + " ";

                }

                double char2WinRate = reader.GetDouble(3);

                if (char2WinRate > 50.0)
                {
                    // character 1 has a higher win rate, set the text color to green
                    Character2Label.Foreground = Brushes.Green;

                }
                else
                {
                    // character 2 has lower winrate, set text color to Red
                    Character2Label.Foreground = Brushes.Red;

                }


                Character2Label.Content += ": " + reader.GetDouble(3).ToString().Substring(0, 5) + "%";


            }


        }

        private void addStageWinRateItem(NpgsqlDataReader reader)
        {
            // check that the reader has any data in it. 

            if (reader.HasRows == false)
            {

                return;

            }

            // if there is no data, then there is no match history between these selected characters. 

            // if there are results, check which listBox to add the current item into. 

            if (reader.GetString(0).TrimEnd() == Char1ComboBox.SelectedItem.ToString())
            {
                // add the current item to the char1listbox

                // add the stagename and the win percent rounded to 2 decimal places. 

                Char1StageWinsGrid.Items.Add(new StageWin()
                {
                    stageName = reader.GetString(1).TrimEnd(),
                    winRate = reader.GetDouble(4)
                });



            } else if (reader.GetString(0).TrimEnd() == Char2ComboBox.SelectedItem.ToString())
            {

                // add the current item to the char2listbox

                // add the stagename and the win percent rounded to 2 decimal places. 

                Char2StageWinsGrid.Items.Add(new StageWin()
                {
                    stageName = reader.GetString(1).TrimEnd(),
                    winRate = reader.GetDouble(4)
                });


            }



        }

        private void CalculateStatsButtonClicked()
        {
            // this method will query the database and populate the UI items with the proper data for the selected characters.

            // KNOWN: The selected characters are 2 different characters. 
            // Still need to check if the characters have a matchup history with each other. 


            string charWinRatesQuery = $@"

select charname, 
count(*) as totalGames,
sum( case when didwin=true then 1 else 0 end) as wins,
sum( case when didwin = true then 1 else 0 end) * 100 / count(*)::float as winrate

from

(select * from 
(select matchid
from tourneysingleschars
where charname in ('{Char1ComboBox.SelectedItem.ToString()}', '{Char2ComboBox.SelectedItem.ToString()}') 
group by matchid
having count( distinct( charname)) = 2 and count(distinct(didwin)) = 2) as bothcharmatches
natural join tourneysingleschars) as bothchardata

group by charname;
";


            string charStageWinRatesQuery = $@"

select charname, stagename, 
count(*) as totalGames,
sum( case when didwin=true then 1 else 0 end) as wins,
sum( case when didwin = true then 1 else 0 end) * 100 / count(*)::float as winrate

from

(select * from 
(select matchid
from tourneysingleschars
where charname in ('{Char1ComboBox.SelectedItem.ToString()}', '{Char2ComboBox.SelectedItem.ToString()}') 
group by matchid
having count( distinct( charname)) = 2 and count(distinct(didwin)) = 2) as bothcharmatches
natural join tourneysingleschars) as bothchardata

group by charname, stagename

order by stagename;
";


            // query for overall win percent first. 

            // check that there is a result of the query. 
            // if there is a result, change the two character labels and add their win percentages to the labels. also populate the number of matches label. 


            // clear the UI items before they are repopulated.

            Character1Label.Content = "";
            Character2Label.Content = "";

            Char1StageWinsGrid.Items.Clear();
            Char2StageWinsGrid.Items.Clear();


            execQuery(charWinRatesQuery, addWinRateInformation);

            // query for the stage win rates. 

            execQuery(charStageWinRatesQuery, addStageWinRateItem);

            // populate the two listBoxes as well. 



        }

    }
}
