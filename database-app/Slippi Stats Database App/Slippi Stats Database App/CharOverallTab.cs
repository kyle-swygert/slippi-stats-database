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

        public class MatchUpWin
        {
            public string charName { get; set; }
            public int numTimesPlayedAgainst { get; set; }
            public double winRate { get; set; }
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
            // NOTE: The tags item is a ListBox currently, change to a DataGrid in the near future. 

        }

        private void addMatchupWinRateColumns()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "Opponent";
            col1.Binding = new Binding("charName");
            col1.Width = 50;
            OverallStageWinDataGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "Num Matched Played Against Opponent";
            col2.Binding = new Binding("numTimesPlayedAgainst");
            col2.Width = 50;
            OverallStageWinDataGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "Win Rate";
            col3.Binding = new Binding("winRate");
            col3.Width = 50;
            OverallStageWinDataGrid.Columns.Add(col3);
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

        private void CalculateOverallCharStatsButtonClicked()
        {
            //query the database to calculate the stats for the selected character in the program. 

        }


    }
}