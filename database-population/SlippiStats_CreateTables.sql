create table match (
    matchID char(20),
    stageName char(50),
    stageID integer,
    date date,
    time time,
    gameType char(10),
    fileName varchar,
    primary key(matchID)
);


create table character (
    charName char(50),
    charID integer,
    color integer,
    didWin boolean,
    matchID char(20),
    team integer, -- the team that the player was on during a match. Will be null if the game was not a Teams match. 
    tag char(4), -- only need 4 since that is the absolute max number of chars that can be used in Melee
    primary key(charID, matchID),
    foreign key(matchID) references match(matchID)


);