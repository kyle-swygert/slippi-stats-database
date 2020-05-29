
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