<b>Pre-requirements:</b>

ElasticSearch, easiest and quickest way is:
docker pull docker.elastic.co/elasticsearch/elasticsearch:6.5.4

docker run -p 9200:9200 -p 9300:9300 -e "discovery.type=single-node" docker.elastic.co/elasticsearch/elasticsearch:6.5.4

<b>Functional:</b>

Atm not using a config file, just for convenience of local debugging, console app is easily deployable with docker in the future.

The collector will first grab all the shows using paging, since this is cached we will fly through this very quickly and get no penalty
After which the long and slow process of getting the casts will begin, every 100 shows it will insert the shows in the data storage, this so you can have a selection of the data stored without having to need the full set, while still having a complete show.

or use one of the IntegrationTests, this however will not save the show.

For the api the url is /api/shows you can add ?page={page number you desire} if you want a different page then the very first, standard the size is 5 since there was no requirement, but can be easily changed.

Small number of test, due to time constraints 
