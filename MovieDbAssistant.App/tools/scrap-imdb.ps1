# movie-db-scrapper-windows-64bit-intel-1.1.1.exe %*
# example:
# cls ; .\scrap-imdb.ps1 "Conjuring : Sous l'emprise du Diable" "release_date=2019-01-01,2019-12-31&title_type=feature,tv_series,tv_miniseries,video,tv_special,tv_movie,tv_episode&count=10,country_of_origin=FR"

param([string]$title="",[string]$year="",[string]$country="",[string]$ext="",[int]$count=10) 

cls
echo title=$title
echo year=$year
echo country=$country
echo count=$count
echo ext=$ext

#pause
#del ./test.json
#ex: ./movie-db-scrapper-windows-64bit-intel-1.1.1.exe imdb test.json "Le marchand de sable" "release_date=2022-01-01,2022-12-31&title_type=feature,tv_series,tv_miniseries,video,tv_special,tv_movie,tv_episode&user-rating=6.2,6.2&country_of_origin=FR&count=10"
#./movie-db-scrapper-windows-64bit-intel-1.1.1.exe imdb test.json $title $filters

./movie-db-scrapper-windows-64bit-intel-1.1.1.exe imdb test.json $title "a=b&release_date=$year-01-01,$year-12-31&count=$count&country_of_origin=$country$ext"
#./movie-db-scrapper-windows-64bit-intel-1.1.1.exe imdb test.json $title "title_type=feature,tv_series,tv_miniseries,video,tv_special,tv_movie,tv_episode&count=10&country_of_origin=$country$ext"
code ./test.json
