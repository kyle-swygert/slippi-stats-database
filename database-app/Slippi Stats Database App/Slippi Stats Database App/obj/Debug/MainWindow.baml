   M S B A M L   `   `   `  ����T  PSlippi Stats Database App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:Slippi_Stats_Database_AppSlippi_Stats_Database_App  5   	   *    $Slippi_Stats_Database_App.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   9local'clr-namespace:Slippi_Stats_Database_App     5   	     � Title$  Slippi Stats Database��5   	   $	��700��6'   $
��1400��64   .��5	      � .+�5
   
   �� $
��Left=�6   $	��635��60   $��	10,25,0,0q�6=   $	��Top=�6P   $
��1374��6h   .@�5      �� $��Match Search��6   .��5      � ���	#FFE5E5E5.+�5      �� $��
Characters��6   $
��Left=�61   $��
10,391,0,0q�6L   $	��Top=�6`   �� 5      -   5         ��Name$ CharListBox��5       �� SelectionMode$ Single=�63   $
��Left=�6J   $	��140��6e   $��
10,417,0,0q�6r   $	��Top=�6�   $	��200��6�   6�   �� 5      $
��Tags��6   $
��Left=�6+   $��
10,200,0,0q�6F   $	��Top=�6Z    J� RenderTransformOrigin$ 0.821,4.984&�6r   �� 5      -   5        $ TagsListBox��5      $ Single=�63   $
��Left=�6J   $	��140��6e   $��
10,226,0,0q�6r   $	��Top=�6�   $	��200��6�   6�   �� 5      $��Dates��6   $
��Left=�6,   $��	10,10,0,0q�6G   $	��Top=�6Z   �� 5      -   5        $ DatesListBox��5      $ Multiple=�64   $
��Left=�6M   $	��140��6h   $��	10,36,0,0q�6u   $	��Top=�6�   $	��200��6�   6�   &   System.Windows.Controls.DataGrid  5      -   5          Name$ 	MatchGrid��5      $
��Left=�62   $	��370��6M   $��
215,36,0,0q�6Z   $	��Top=�6n   $	��934��6�      SelectionMode$ Single=�6�    � 5      $��
Team Color��6   $
��Left=�61   $��79��6L   $��1154,36,0,0q�6X   $	��Top=�6m   $	��100��6�    � 5      $��	Game Type��6   $
��Left=�60   $��79��6K   $��1259,36,0,0q�6W   $	��Top=�6l   $	��100��6�    � 5      $��Stage��6   $
��Left=�6,   $	��138��6G   $��1154,120,0,0q�6T   $	��Top=�6j   $	��205��6�   �� 5      -   5         ��Name$ NumMatchesLabel��5      $��# of Matches: ###��65   $
��Left=�6Q   $��	996,5,0,0q�6l   $	��Top=�6   $	��150��6�   �� 5      -   5         ��Name$ SearchButton��5      $��Search Matches��63   $
��Left=�6L   $��1208,468,0,0q�6g   $	��Top=�6}   $	��100��6�   $��61��6�   6�   �� 5      -   5         ��Name$ 	SinglesCB��5      $��Singles��62   $
��Left=�6D   $��1260,60,0,0q�6_   $	��Top=�6t   �� 5      -   5        $ TeamsCB��5      $��Teams��60   $
��Left=�6@   $��1260,80,0,0q�6[   $	��Top=�6p   �� 5      -	   5        $ FreeForAllCB��5      $��Free For All��65   $
��Left=�6L   $��1260,100,0,0q�6g   $	��Top=�6}   �� 5      -
   5        $ BattlefieldCB��5      $��Battlefield��66   $
��Left=�6L   $��1154,145,0,0q�6g   $	��Top=�6}   �� 5      -   5        $ DreamlandCB��5      $��Dream Land N64��64   $
��Left=�6M   $��1154,165,0,0q�6h   $	��Top=�6~   �� 5      -   5        $ FinalDestCB��5      $��Final Destination��64   $
��Left=�6P   $��1154,183,0,0q�6k   $	��Top=�6�   �� 5      -   5        $ FoDCB��5      $��Fountain of Dreams��6.   $
��Left=�6K   $��1154,203,0,0q�6f   $	��Top=�6|   �� 5       -   5        $ PokemonStadiumCB��5       $��Pokemon Stadium��69   $
��Left=�6S   $��1154,223,0,0q�6n   $	��Top=�6�   �� 5!      -   5        $ YoshiStoryCB��5!      $��Yoshi's Story��65   $
��Left=�6M   $��1154,243,0,0q�6h   $	��Top=�6~   �� 5"      -   5        $ 
TeamBlueCB��5"      $��	Blue Team��63   $
��Left=�6G   $��1154,60,0,0q�6b   $	��Top=�6w   �� 5#      -   5        $ 	TeamRedCB��5#      $��Red Team��62   $
��Left=�6E   $��1154,80,0,0q�6`   $	��Top=�6u   �� 5$      -   5        $ TeamGreenCB��5$      $��
Green Team��64   $
��Left=�6I   $��1154,100,0,0q�6d   $	��Top=�6z   �� 5%      -   5        $ ClearDatesButton��5%      $��Clear Date Selections��67   $
��Left=�6W   $��
10,176,0,0q�6r   $	��Top=�6�   $	��200��6�   6�   �� 5&      -   5        $ ClearTagsButton��5&      $��Clear Tag Selections��66   $
��Left=�6U   $��
10,366,0,0q�6p   $	��Top=�6�   $	��200��6�   6�   �� 5'      -   5        $ ClearCharsButton��5'      $ ��Clear Character Selections��67   $
��Left=�6\   $��
10,557,0,0q�6w   $	��Top=�6�   $	��200��6�   6�   �� 5(      -   5        $ ClearAllSelectionsButton��5(      $��Clear All Selections��6?   $
��Left=�6^   $	��Top=�6y   $	��205��6�   $��
1154,8,0,0q�6�   6�   �� 5)      $*��$Selected Match Character Information��6   $
��Left=�6K   $��215,411,0,0q�6f   $	��Top=�6{     5*      -   5        $ CharInfoGrid��5*      $
��Left=�65   $	��120��6P   $��215,437,0,0q�6]   $	��Top=�6r   $	��934��6�   6�   �� 5+      $��Match Search View��6   $��20(�68   $
��Bold"�6F   $
��Left=�6X   $��
215,-1,0,0q�6s   $	��Top=�6�   $	��211��6�   5,      5-      �� 5.      $��Char Vs. Char��6   .��5/      � ���	#FFE5E5E5.+�50      �� $	��Vs.��6   $
��Bold"�6*   $��20(�6<   $	��Top=�6J   $��666,0,667,0q�6b   �� 51      $��Character Matchup Stats: ��6   $��20(�6@   $
��Bold"�6N   $
��Left=�6`   $	��Top=�6{   �� 52      -   5        	 ��Name$	 Char1ComboBox��52      $
��Left=�66   $��
511,10,0,0q�6Q   $	��Top=�6e   $	��150��6}   �� 53      -   5        $	 Char2ComboBox��53      $
��Left=�66   $��
706,10,0,0q�6Q   $	��Top=�6e   $	��150��6}   �� 54      -   5        $��Calculate Matchup Stats��54      $
��Left=�6?   $��
1154,8,0,0q�6[   $	��Top=�6o   $	��205��6�   6�   �� 55      -   5        $  CharMatchupNumMatchesLabel��55      $��# of Matches: ###��6@   $
��Left=�6\   $��	996,5,0,0q�6w   $	��Top=�6�   $	��148��6�   �� 56      -   5        $ Character1Label��56      $
��Bold"�65   $��20(�6G   $��Character 1��6U   $
��Left=�6k   $	��Top=�6�   $��
155,76,0,0q�6�   $	��300��6�   �� 57      -   5        $ Character2Label��57      $
��Bold"�65   $��20(�6G   $��Character 2��6U   $
��Left=�6k   $��1017,76,0,0q�6�   $	��Top=�6�   $	��300��6�   �� 58      ���	#FFF4F4F5$
��Left=�61   $	��262��6L   $��
10,326,0,0q�6Y   
���   �$	��Top=�6|   $
��1349��6�     59      -   5        $ Char1StageWinsGrid��59      $
��Left=�6;   $	��200��6V   $��
10,121,0,0q�6c   $	��Top=�6w   $	��250��6�     5:      -   5        $ Char2StageWinsGrid��5:      $
��Left=�6;   $	��200��6V   $��1108,121,0,0q�6c   $	��Top=�6y   $	��250��6�   6�   5;      5<      �� 5=      $��Char Overall Stats��6   $
��Left=�62   $��20��6M   $	��Top=�6Y   .��5>      � ���	#FFE5E5E5.+�5?      �� $��Character Overall Stats: ��6   $��20(�6@   $
��Bold"�6N   $
��Left=�6`   $	��Top=�6{   �� 5@      -    5        $	 CharOverallComboBox��5@      $
��Left=�6<   $��
305,10,0,0q�6W   $	��Top=�6k   $	��150��6�   �� 5A      -!   5        $% CalculateOverallCharStatsButton��5A      $��Calculate Character Stats��6F   $
��Left=�6j   $��
1154,8,0,0q�6�   $	��Top=�6�   $	��205��6�   6�   �� 5B      $
��Tags��6   $
��Left=�6+   $��	10,42,0,0q�6F   $	��Top=�6Y   �� 5C      $��Colors��6   $
��Left=�6-   $	��Top=�6H   $��
11,330,0,0q�6`   �� 5D      -"   5        $  CharOverallWinPercentLabel��5D      $��Character: Win% PickRate%��6@   $��20(�6d   $
��Bold"�6r   $
��Left=�6�   $��	470,0,0,0q�6�   $	��Top=�6�   $	��679��6�     5E      -#   5        $ OverallMatchUpWinDataGrid��5E      $
��Left=�6B   $	��500��6]   $��
305,73,0,0q�6j   $	��Top=�6~   $	��250��6�     5F      -$   5        $ OverallStageWinDataGrid��5F      $
��Left=�6@   $	��499��6[   $��
619,73,0,0q�6h   $	��Top=�6|   $	��250��6�   �� 5G      $��Match Up Win %��6   $
��Left=�65   $��
305,42,0,0q�6P   $	��Top=�6d   �� 5H      $��Stage Win %��6   $
��Left=�62   $��
619,42,0,0q�6M   $	��Top=�6a   �� 5I      -%   5        $ TournamentLegalStagesCB��5I      $"��Tournament Legal Stages Only��6@   $
��Left=�6g   $��1154,32,0,0q�6�   $	��Top=�6�     5J      -&   5        $ ColorsUsedDataGrid��5J      $��Right=�6;   $	��185��6W   $��0,362,1142,0q�6d   $	��Top=�6z   $	��216��6�     5K      -'   5        $ OverallTagUsageGrid��5K      $	��251��6<   $��9,77,1142,0q�6I   $	��Top=�6^   5L      5M      5N      ]� 5O   
   $
��Left=�6   $��20��6/   ��.  $	��Top=�6Q   $
��1394��6i   $��
0,0,-0.4,0q�6v   .+�5P      o� $
��1394��6   $��	0,0,0,0.6q�6    .@�5Q      m� $
��File��6   $��20��6)   $��40��65   .@�5R      m� -(   5        $��Add Files to Database��5R      $
��Left=�6>   $��20��6Y   $	��200��6e   6q   5S      m� 5T      $
��Help��6   $��20��6)   $��40��65   m� 5U      $
��Info��6   $��19��6)   $��40��65   5V      5W      5Y      5Z      