'''
    Repurposed Slippi File renaming Program to populate Slippi Stats Database

    TODO: change all lines where file is renamed to an insert statement into the database. 


'''



from slippi import *
from os import walk, listdir, rename, path
import psycopg2
import uuid

conn = psycopg2.connect("dbname='SlippiStats' user='postgres' host='localhost' password='admin'")
cur = conn.cursor()

numFiles = 0

teamDict = {
    0: "Red",
    1: "Blue",
    2: "Green"
}

def connect_to_database():
    hostname='localhost'
    dbname = 'SlippiStats'
    username='postgres'
    password='admin'

    print("Connecting to the database:")


    try:

        conn = psycopg2.connect("dbname='SlippiStats' user='postgres' host='localhost' password='admin'")

        cur = conn.cursor()

        return True

    except:
        print("Could Not Connect to Database...")
        return False

def didWinGame(slippiGame, playerPort):
    # This function will accept a slippi game object, and a player port int that represents the port of the player that is to be checked for the win condition. 

    # build list of ports that were used in the current game

    #curPort = 0

    #portsUsed = []
   

    lastFrame = slippiGame.frames[-1]
    
    playerPortPostFrameStocks = lastFrame.ports[playerPort].leader.post.stocks

    return playerPortPostFrameStocks > 0

'''

    for player in lastFrame.ports:

        if player is not None:
            # add the current index to the list. 

            portsUsed.append(curPort)

        # increment curPort every time no matter if the port is used or not. 
        curPort += 1


    for port in portsUsed:

        

        if lastFrame.ports[playerPort] >= lastFrame.ports[port] and playerPort != port:
            pass
        else:
            return False

    return True
'''

def get_datetime(slippiFileName):
    return slippiFileName.split('_')[-1].split('.')[0]

def insert_data_into_database(slippiFileName):

    


    conn = psycopg2.connect("dbname='SlippiStats' user='postgres' host='localhost' password='admin'")
    cur = conn.cursor()

    # NOTE: this function will run everytime that a file is to be inserted into the database. 


    # TODO: remove date-time generation from all other function for this function to work properly!!!!
    # TODO: TEST THIS FUNCTION!!!! HAS NOT BEEN TESTED AT ALL!!



    '''
    TODO: create cases in this function in the future to rename files based on the type of game being played. 
        ex) singles, doubles, free for all. 

    if is teams game:
        generate_doubles_game_name()

    else:

        if 2 players in game:
            generate_singles_game_name()

        elif more than 2 players in game:
            # either a 3 or 4 person free for all match. 
            generate_free_for_all_game_name()

    '''

    '''
    NEW FUNCTION ALGO: This function will NOT loop through the files, only generate name for single file. 
    open the file with slippi
        if not corrupt:
            get the date-time from the string, save for later
            check which function to rename with
            if teams == True:
                generate_doubles()
            else:
                if 2 players:
                    generate_singles()
                else >2 players:
                    generate_FFA()

            newFile = gameName + datetime + .slp # using the path.join() function
            rename(oldFile, newFile)


    '''

    # newFile = ''

    try:
        slippiGame = Game(slippiFileName)

        gameType = ""

        tempTime = slippiGame.metadata.date

        #print(f"temptime: {tempTime}")
        
        if slippiGame.start.is_teams == True:
            
            gameType = "Teams"

        else: # could be singles or FFA
            #print('non-teams game')
            curPlayerCount = 0
            for player in slippiGame.start.players:
                if player != None:
                    curPlayerCount += 1

            #print(f'players in game: {curPlayerCount}')
            if curPlayerCount == 2:
                
                gameType = "Singles"

            else:
                
                gameType = "FFA"

        # TODO: Get the date and time into a format that is able to be used as the time and date format for postgreSQL in the DB. 


        # generate unique string for the match
        matchID = str(uuid.uuid4())


        #print(f"Insert {slippiFileName} into the database.")
        #print(f"insert data: uuid: {matchID}, gametype: {gameType}, stage: {slippiGame.start.stage.__dict__['_name_']}, date: {date}, time: {time}")
        #print(f"stage stuff: {slippiGame.start.stage.__dict__}")

        stageName = str( slippiGame.start.stage.__dict__['_name_'])
        stageID = str( slippiGame.start.stage.__dict__['_value_'])

        fileName = slippiFileName.split('/')[-1]

        #print(f"INSERT NOT IMPLEMENTED YET!!!!!")
        
        '''
        match data to insert into the DB:

        unique matchID
        stage name
        stage ID value
        dateTime
        filename
        gametype: (singles, teams, FFA)


        '''
    

        matchInsert = f"INSERT INTO match(matchID, stageName, stageID, matchdate, gameType, filename) VALUES('{matchID}', '{stageName}', {stageID}, TIMESTAMP '{tempTime}', '{gameType}', '{fileName}');"

        #print(matchInsert)

        try: 

            #print("before match insert")

                    
            conn = psycopg2.connect("dbname='SlippiStats' user='postgres' host='localhost' password='admin'")
            cur = conn.cursor()

            # insert the match data into the database. 
            cur.execute(matchInsert)
            conn.commit()

            #print("after match insert")

        except Exception as e:
            print(e)
            print("Insert into match failed...")

       
        '''
        character data to insert into the DB:

        unique charID
        matchID same as the previously generated string so that the character can be associated with the specific match. 
        didWin Bool
        long char name: Captain_Falcon
        short char name: CF (if possible, is this even needed? probably not...)
        char ID value
        color
        tag (set to null if not used)

        '''

        #print(f"charaters in the current game:")

        curPort = 0

        for player in slippiGame.start.players:

            if player is not None:     

                #print(f"name: {player.character.__dict__['_name_']}, color: {player.costume}, tag: {player.tag}")

                charName = player.character.__dict__['_name_']
                #charID = player.character.__dict__['_value_']

                charID = str(uuid.uuid4()) # create a unique ID for each character that is in the database. 

                didWin = True # This is always set true for now, i think there is code in here from before to determine the winner of the match. 
                

                team = ""

                if slippiGame.start.is_teams == True:
                    team = teamDict[player.team]
                

                # TODO: translate the tags from full-width japanese characters into half-width regular english characters so that tags can be searched with regular keyboard inputs. 
                # Leave japanese tags in japanese characters. 

                charInsert = f"INSERT INTO character(charName, charID, color, didWin, matchID, team, tag) VALUES('{charName}', '{charID}', {player.costume}, {didWinGame(slippiGame, curPort)}, '{matchID}', '{team}', '{player.tag}');"

                try:
                    #print("before char insert")


                    conn = psycopg2.connect("dbname='SlippiStats' user='postgres' host='localhost' password='admin'")
                    cur = conn.cursor()


                    cur.execute(charInsert)
                    conn.commit()

                    #print("after char insert")

                except Exception as e:
                    print(e)
                    print("failed to insert into char...")

            curPort += 1


        cur.close()

    except:
        print(f'CORRUPTED: {slippiFileName}')
        

    return #newFile

'''

def generate_doubles_game_name(slippiFile):
    
    Team Naming Guide Idea: TeamGreen(Falco+(H E L P))-Vs-TeamBlue(CaptainFalcon+(S E L F))_datetime.slp
    TeamColor(char1(TAG)-char2(TAG))-Vs-TeamColor(char3(TAG)-char4(TAG))_datetime.slp
    
    TODO: test the execution of the doubles renaming function. stops executing when hitting (if player.team == firstTeamPlayers[0].team) [Nonetype object has no attribute team]

    TODO: check if any of the players in the game are CPUs. If there are any CPUs, add that in the filename. 

    TODO: improve the try except blocks in the program to further locate where the program is having difficulties executing. 

    

    # NOTE: assuming that all doubles games are full, meaning two teams of 2 players each. 

    newFile = ''
    firstTeamPlayers = []
    secondTeamPlayers = []

    firstTeamParts = []
    secondTeamParts = []

    # the team that the player is on is in the Player object of the game file. 

    # TODO: check that there are 4 players in this game. this teams game could be 2v1, which could cause an issue. it also might not cause an issue. 

    # add the first player to the lists. Compare all other players to this player for building the lists of players in the same team. 
    firstTeamPlayers.append(slippiFile.start.players[0])

    for player in slippiFile.start.players[1:]:

        if player != None:

            if player.team == firstTeamPlayers[0].team:
                firstTeamPlayers.append(player)
            else:
                secondTeamPlayers.append(player)

    # players have been separated by team, now build the list of strings for the team names. 

    # build the first team list of strings

    # remove the '.' between the team and the color strings. 
    [firstTeamParts.append(item) for item in str(firstTeamPlayers[0].team).split('.')]

    firstTeamParts.append('(')

    for player in firstTeamPlayers:

        if player.tag != '':
            # add only the tag of the player to the filename
            firstTeamParts.append(player.tag)

        else:
            # add only the character name to the filename

            [firstTeamParts.append(item.lower().capitalize()) for item in str(player.character).split('.')[-1].split('_')]

        firstTeamParts.append('-')

    firstTeamParts.pop()

    firstTeamParts.append(')')

    # build the second team list of names
    # remove the '.' between the team and the color strings. 
    [secondTeamParts.append(item) for item in str(secondTeamPlayers[0].team).split('.')]

    secondTeamParts.append('(')

    for player in secondTeamPlayers:

        if player.tag != '':
            # add only the tag of the player to the filename
            secondTeamParts.append(player.tag)

        else:
            # add only the character name to the filename

            [secondTeamParts.append(item.lower().capitalize()) for item in str(player.character).split('.')[-1].split('_')]

        secondTeamParts.append('-')

    secondTeamParts.pop()

    secondTeamParts.append(')')

    # append first team strings to file name
    for item in firstTeamParts:
        newFile += item

    # append '-Vs-' between the team names
    newFile += '-Vs-'

    for item in secondTeamParts:
        newFile += item

    # add the date-time to the filename. 
    #newFile += '_' + str(slippiFile.metadata.date).split('+')[0].replace(' ', '').replace('-', '').replace(':', '').split('.')[0]    

    #newFile += '.slp'

    #print('New Doubles match name: {}'.format(newFile))


    #print(firstTeamPlayers)
    #print('\n\n\n\n\n')
    #print(secondTeamPlayers)


    return newFile

def generate_free_for_all_game_name(slippiFile):
    
    iterate through all the players in the game
    build a list of strings of the character names and '-Vs-'
    Ex) Fox-Vs-Falco-Vs-Sheik_20191010143456.slp


    
    #print('implement the free-for-all function')

    newFile = ''

    # list used to build the new Filename
    newFileNameParts = []

    newFileNameParts.append('FFA_')


    #firstChar = False

    # iterate over players in the game
    for player in slippiFile.start.players:

        # append the formatted character name from the character object to the list
        [newFileNameParts.append(item.lower().capitalize()) for item in str(player.character).split('.')[-1].split('_')] if player != None else None

        # append the player tag to the list if they were using one
        #newFileNameParts.append('(' + player.tag + ')') if player != None and player.tag != '' else None

        # append the '-vs-' string to the list no matter what. 
        newFileNameParts.append('-Vs-') if player != None else None

        
    # remove the redundant '-vs-' string from the list. 
    newFileNameParts.pop()

    # Build the new Filename
    for item in newFileNameParts:
        newFile += item

    # appending the date and time that the game was played to reduce chance of overwriting files
                
    # Could I do list comprehension here to replace the multiple different characters with nothing?
    # NOTE: the previous format of <date>.slp is not allowed on the windows platform, so it is now replaced with _date.slp
    #newFile += '_' + str(slippiFile.metadata.date).split('+')[0].replace(' ', '').replace('-', '').replace(':', '').split('.')[0] #  + '_'

    #newFile += '.slp'

    #print(newFile)

    return newFile

def generate_singles_game_name(slippiFile):
    '''
    # this function will build a string based on the characters used in the game. 

'''

    newFile = ''

    # list used to build the new Filename
    newFileNameParts = []

    firstChar = False

    # iterate over players in the game
    for player in slippiFile.start.players:


        # NOTE: this CPU player check is causing the program to execute improperly. figure out why and how to append the 'CPU' string to the list of strings. I removed the statements entirely to get rid of the issue. 

        # append the formatted character name from the character object to the list
        [newFileNameParts.append(item.lower().capitalize()) for item in str(player.character).split('.')[-1].split('_')] if player != None else None

        # append the player tag to the list if they were using one
        newFileNameParts.append('(' + player.tag + ')') if player != None and player.tag != '' else None

        # append '-Vs-' string to list if this is the first player being added to the list
        if newFileNameParts != [] and firstChar == False:
            firstChar = True
            newFileNameParts.append('-Vs-')


    # Build the new Filename
    for item in newFileNameParts:
        newFile += item

    # appending the date and time that the game was played to reduce chance of overwriting files
                
    # Could I do list comprehension here to replace the multiple different characters with nothing?
    # NOTE: the previous format of <date>.slp is not allowed on the windows platform, so it is now replaced with _date.slp
    #newFile += '_' + str(slippiFile.metadata.date).split('+')[0].replace(' ', '').replace('-', '').replace(':', '').split('.')[0] #  + '_'

    #newFile += '.slp'

    #print(newFile)

    return newFile

    '''

def insert_files_from_folder_into_database(folder):
    '''
    this function accepts the name of a folder as a string and will rename all the .slp files in the directory and sub-directories. 
    '''

    global numFiles


    #print("Connect to the database using the conn global variable")
    #print("NOT YET IMPLEMENTED!!!!")

    connect_to_database()



    #print('folder that contains all the slippi files: {}'.format(folder))

    # check that the folder exists in the system. 
    if path.exists(folder) == False:
        #print(f'{folder} does not exist in the system')
        return

    #print(f'{folder} exists, renaming all files in the folder...')

    for root, dirs, files in walk(folder):
        # root represents the current directory that is being processed
        # dirs represents the sub-directories in the currently processing directory
        # files represents the files inside the root directory

        # TODO: For each directory and sub-directory in the main directory, create a thread to rename that specific directory. 
        '''
        NOTE: Thiw might mean that the way that the program works may need to be changed. if there is a directory in a directory, don't rename the directory recursively, ONLY rename the .slp files in the directory. 

        make a list of threads, where each thread is a function execution of the insert_files_from_folder_into_database() function. Only execute 4 threads at a time. when one thread is complete, add another thread to the currently running queue. 


        '''


        # I want to store the file back into the directory that it came from in the directories, and I believe that I will need the root string to do that. 
        #print(root + " " + str(files))

        # TODO: in the future, reorganize the code so that the path.join() method is only called once in this funtion. will make this code cleaner in my opinion. 

        #print(f"Processing Directory {root}: ")

        for curr in files:

            # check if the file ends in .slp
            # check that the game is not a teams or FFA
            # if only a 2 player game, send the SlippiGame file to the insert_data_into_database() function. 

            if curr.endswith('.slp') == True:

                currFilePath = path.join(root, curr)

                insert_data_into_database(currFilePath)

                
                numFiles += 1

                if numFiles % 150 == 0:
                    print(f"files processed: {numFiles}")

                '''
                if newFileNameWhole != '':
                    newFilePath = path.join(root, newFileNameWhole)
                    rename(currFilePath, newFilePath)
                else:
                    #print('something went wrong...')
                    pass
                '''

            else:
                print(f'WRONG FORMAT: {curr}')
                pass


# NOTE: this is where the program starts executing when run as a command line program. 
if __name__ == "__main__":

    directory = path.join(path.dirname(path.realpath(__file__)), 'slp')

    print(f"Directory being processed: {directory}")

    # "D:\Project Slippi Replays\All WSU Slippi Replays from Google Drive"

    # /mnt/d/Project Slippi Replays/All WSU Slippi Replays from Google Drive

    #insert_files_from_folder_into_database(directory)

    insert_files_from_folder_into_database("/mnt/d/Project Slippi Replays/All WSU Slippi Replays from Google Drive")

    #print("All files in the directory have been renamed.")
