
-- get all the distinct dates for games that are in the database
select distinct(DATE(matchdate)) as date
from match
order by date asc;

-- count the number of times that a character was played on a specific date
select distinct(charname), count(*) as uses
from (match natural join character) bigboi 
where DATE(matchdate)='2019-09-14'
group by charname
order by uses desc;


-- count the number of matches played on each date
select DATE(matchdate), count(*) as games
from match 
group by DATE(matchdate)
order by date(matchdate) asc;

-- trying to calculate win pickrate for each character. 
select charname, (((count(*)::float ) / ((select count(*) from match)::float) ) )::float * 100.00 as pickrate
from (match natural join character)
group by charname
order by pickrate desc;




select distinct(match.*)  from 
(
select matchid from 
(select charinmatch.matchid
from character_played_in_match as charinmatch, match as m, character as mychar
where mychar.charid = charinmatch.charid
 and mychar.charname in ('CAPTAIN_FALCON', 'MARTH')
group by charinmatch.matchid
having (count(charinmatch.charid) >= (select count(charid) from character ))
) as subresult 
natural join match natural join character_played_in_match natural join character
where gametype='Singles'
group by matchid
having count( distinct( charname)) = 2
)  as goblin 
natural join match natural join character_played_in_match natural join character