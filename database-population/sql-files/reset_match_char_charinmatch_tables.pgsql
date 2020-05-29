
drop table character CASCADE;
drop table match CASCADE;
DROP table character_played_in_match CASCADE;


create table match (
    matchID varchar(50),
    stageName varchar(50),
    stageID integer,
    matchDate timestamp,
    gameType char(10),
    numOfFrames integer,
    fileName varchar,
    primary key(matchID)
);

create table character (
    charName varchar(50),
    charID varchar(50),
    color integer,
    didWin boolean,
    team char(5), -- the team that the player was on during a match. Will be null if the game was not a Teams match. 
    tag char(4), -- only need 4 since that is the absolute max number of chars that can be used in Melee
    portNum integer,
    cssID integer,
    primary key(charID)
    
);

-- this relation will relate all the characters that played in a specific match. 
create table character_played_in_match (

    matchID varchar(50),
    charID varchar(50),
    foreign key(charID) references character(charID),
    foreign key(matchID) references match(matchID),
    primary key(matchID, charID)

);
