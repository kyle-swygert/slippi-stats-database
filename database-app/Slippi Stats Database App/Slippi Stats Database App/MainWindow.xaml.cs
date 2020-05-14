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
        public MainWindow()
        {
            InitializeComponent();
        }

        // NOTE: Can calculate match duration from numofframes
        // ex) 17284 frames @ 60 fps is 4:48 (only calculate the minutes and seconds

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // add files to database button was just clicked. 

            // use an OpenFileDialog to select a directory

            // use the selected directory to then call the python code to populate the database recursively through the directory tree. 

            // NOTE: While the database population is occuring, disable the main window, popup a small window that says "Database Population in progress" with a loading bar of some sort. add a cancel button to the popup
            // when population has finished or been cancelled, re-enable the main window and remove the popup window. 

            Console.Write("Add Files to DB Button clicked.");



        }
    }
}
