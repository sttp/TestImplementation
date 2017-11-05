A note about synchronization for the publisher.

For datasets that are small enough to be stored in memory, 
periodically query the main source of the data and check which field have changed, 
This is made easy with the default implementation included in Sttp.Core.

For datasets that are enormous, there should be some kind of ETL or versionID in place, 
apply patches when this number changes. 

If no versioning is included, keeping a local copy of the primary key of every row, 
and a hashsum of the contents of the row can help identify when a row changes.

If you don't expect to have to normally synchronize the entire dataset, turning off versioning 
in sttp might be the best solution.