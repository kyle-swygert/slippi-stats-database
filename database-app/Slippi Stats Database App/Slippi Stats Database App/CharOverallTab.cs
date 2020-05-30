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
using System.Drawing;


namespace Slippi_Stats_Database_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public class MatchUpWin
        {
            public string charName { get; set; }
            public int numTimesPlayedAgainst { get; set; }
            public double winRate { get; set; }
        }

        public class UsesObj
        {
            public string item { get; set; }
            public int itemInt { get; set; }
            public int uses { get; set; }
        }


        // NOTE: All the functionality of the Char Overall Tab will be placed in this file for the partial class



        /*
         
            Queries for Overall Character Stats:

-- list the winrates of the selected character on each tournament legal stage. 
select stagename, (sum( case when didwin=true then 1 else 0 end )::float /  count(*)::float) * 100 as winrates
from tourneysingleschars
where charname='CAPTAIN_FALCON'
group by stagename;

-- list the tags used by a character and the number of times the tag was used. 
select tag, count(*) as uses
from tourneysingleschars
where charname='CAPTAIN_FALCON'
group by tag
order by uses desc;

-- list the colors used by a character and the number of times the color was used. Use the charID number and the color number to display the stock icon in the all as a future feature. 
select color, count(*) as uses
from tourneysingleschars
where charname='CAPTAIN_FALCON'
group by color
order by uses desc;

-- calculate the winrates against all characters in the database, Don't count ditto matches. NOTE: This is still a work in progress, not yet working properly...
select charname, count(*) as totalgames, sum( case when didwin=true then 1 else 0 end)::float / count(*) * 100 as winrate
from tourneysingleschars
where charname not in ('CAPTAIN_FALCON')
group by charname;


select distinct(charname), sum( case when didwin=true then 1 else 0 end)::float / count(*) * 100 as winrate
from tourneysingleschars as t1
where charname = any (select charname 
					from tourneysingleschars as t2 
					where t1.matchid=t2.matchid
					and charname='CAPTAIN_FALCON')
group by charname





             
             
             */


        private void initCharOverallTab()
        {
            // TODO: initailize all the UI items for this tab in this method. 

            addMatchupWinRateColumns();

            addStageWinRateColumns();

            initOverallCharComboBox();

            initColorsUsedDataGrid();

            addTagsUsedColumns();

        }

        private void initColorsUsedDataGrid()
        {
            // TODO: Populate the datagrid with the stock icon image as a test to begin with. 

            // for final version, query the database to display the icon of the color and the number if times that it was used in all the matches. 



            // add columns to the data grid

            //DataGridTextColumn col1 = new DataGridTextColumn();
            //col1.Header = "Color Used";
            ////col1.Binding = new Binding("stageName");
            //col1.Width = 200;
            //ColorsUsedDataGrid.Columns.Add(col1);

            DataGridColumn col1 = new DataGridTextColumn();
            col1.Header = "Color Used";
            //col1.Binding = new Binding("stageName");
            col1.Width = 200;
            
            ColorsUsedDataGrid.Columns.Add(col1);

            // add all the icons to the data grid




        }

        private void initOverallCharComboBox()
        {
            string characterQuery = "select distinct(charname) from character order by charname asc;";

            // execute query and populate the comboboxes

            execQuery(characterQuery, addOverallCharComboBoxItem);
        }

        private void addOverallCharComboBoxItem(NpgsqlDataReader reader)
        {

            CharOverallComboBox.Items.Add(reader.GetString(0).TrimEnd());

            
        }

        private void addTagsUsedColumns()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "Tag";
            col1.Binding = new Binding("tag");
            col1.Width = 50;
            OverallTagUsageGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "Uses";
            col2.Binding = new Binding("uses");
            col2.Width = 50;
            OverallTagUsageGrid.Columns.Add(col2);
        }

        private void addMatchupWinRateColumns()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "Opponent";
            col1.Binding = new Binding("charName");
            col1.Width = 50;
            OverallMatchUpWinDataGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "Num Matched Played Against Opponent";
            col2.Binding = new Binding("numTimesPlayedAgainst");
            col2.Width = 50;
            OverallMatchUpWinDataGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "Win Rate";
            col3.Binding = new Binding("winRate");
            col3.Width = 50;
            OverallMatchUpWinDataGrid.Columns.Add(col3);
        }

        private void addStageWinRateColumns()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "Stage Name";
            col1.Binding = new Binding("stageName");
            col1.Width = 50;
            OverallStageWinDataGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "Num Matched Played on Stage";
            col2.Binding = new Binding("numTimesPlayedOnStage");
            col2.Width = 50;
            OverallStageWinDataGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "Win Rate";
            col3.Binding = new Binding("winRate");
            col3.Width = 50;
            OverallStageWinDataGrid.Columns.Add(col3);
        }

        private void addStageWinRateGridRow(NpgsqlDataReader reader)
        {
            OverallStageWinDataGrid.Items.Add(new StageWin()
            {
                stageName = reader.GetString(0),
                numTimesPlayedOnStage = reader.GetInt32(1),
                winRate = reader.GetDouble(2)
            });
            

        }

        private void addTagUsageGridRow(NpgsqlDataReader reader)
        {
            OverallTagUsageGrid.Items.Add(new UsesObj()
            {
                item = reader.GetString(0),
                uses = reader.GetInt32(1)
            });


        }

        private void addColorUsageGridRow(NpgsqlDataReader reader)
        {
            OverallTagUsageGrid.Items.Add(new UsesObj()
            {
                itemInt = reader.GetInt32(0),
                uses = reader.GetInt32(1)
            });


        }

        private void ResetCharOverallGUIItems()
        {
            // Reset all the items that are on the Char Overall tab. 
            // call this function when querying the database for another character's stats. 




        }

        private void CalculateOverallCharStatsButtonClicked()
        {
            //query the database to calculate the stats for the selected character in the program. 


            // query for stage win rates. 

            string stageWinRates = $@"
select stagename, count(*), (sum( case when didwin=true then 1 else 0 end )::float /  count(*)::float) * 100 as winrates
from tourneysingleschars
where charname='{CharOverallComboBox.SelectedItem.ToString()}'
group by stagename;
";


            execQuery(stageWinRates, addStageWinRateGridRow);

            // query for the tags used

            string tagUses = $@"
select tag, count(*) as uses
from tourneysingleschars
where charname='{CharOverallComboBox.SelectedItem.ToString()}'
group by tag
order by uses desc;
";
            execQuery(tagUses, addTagUsageGridRow);

            // query for the colors used. 

            string colorUses = $@"
select color, count(*) as uses
from tourneysingleschars
where charname='{CharOverallComboBox.SelectedItem.ToString()}'
group by color
order by uses desc;
";


            // query for the matchup win rates. NOTE: This query has not been solved yet, finish this in the near future!!!!




        }


    }
}