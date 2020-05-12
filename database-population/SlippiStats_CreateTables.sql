-- try to figure out some way that the smash.gg attendee checkin data can be used in this database as well. that would be really cool!!!!






create table game (
    gameID char(20), -- each game inserted will have a unique gameID. 
    isTeams boolean,
    datePlayed Date,
    timePlayed time(0),
    -- the port attributes below will represent the character that was played in that port for that current game. 
    port1char char(50),
    port2char char(50),
    port3char char(50),
    port4char char(50),
    primary key(gameID),
    foreign key(port1char) references character(characterName),
    foreign key(port2char) references character(characterName),
    foreign key(port3char) references character(characterName),
    foreign key(port4char) references character(characterName)

);
/*
create table teamsGame (


);

-- table that contains all the 2 player teams that have played matches together. this might not be necessary for the first version of this app. 
create table team (
    member1 char(50),
    member2 char(50),
    teamName char(50), -- this could be used from the team name that is registered in smash.gg
    teamColor char(5), -- blue, red, or green
    primary key(member1, member2),
    foreign key(member1) references character(characterName),
    foreign key(member1) references character(characterName)
);

create table singlesGame (


);

create table freeforallGame (


);

*/

create table character (
    characterName char(50),
    shortName char(5), -- abbreviation of the character name
    numTimesPlayed Integer, -- will be incremented on insert to the table
    numTimesWon Integer, -- will be incremented on insert to the table
    winRate float, -- will be calculated on insert to the table
    colorUsed char(20), -- this attr might need to be included in a separate table. I want to capture the color of the skin that the player used for a specific match. 


    primary key(characterName)

);

create table stage (
    stageName char(50),
    shortName char(5), -- abbreviation of the stage name. 
    numTimesPlayed Integer,

    primary key(stageName)
);

/*
-- this will represent the 4 character tag that a player can use in a match
create table tag (
    tagText char(4),
    playerName char(50),



);


-- this table could represent the actual human player that played the game. could include their real name, tags used, characters used, and possible other pieces of information as well. 
create table player (
    playerName char(50),



);

*/