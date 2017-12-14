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



Rules about synchronization:

1) Any schema changes invalidates all data.
2) Joins are only 1 to many.
3) Each row must have a version number which is the last time that row was edited. (Note: This data isn't passed to the client.)
4) Each row must be uniquely identifiable. If the row cannot be identified, versioning must be disabled and the unique identifier can be a incrementing int64.
5) When rows are modified, only the version number of that specific row must be updated.
6) Primary Keys cannot be modified, they must be deleted and a new row added. 
7) When syncing a query, only rows that have been changed (including checking the row version of all joined table) must be validated. 
For all of these rows, the server must send DefineRow or UndefineRow, they cannot be filtered because the server 
does not know if the data used to be in the client's set, but no longer is.
8) When deleting a row, either change the schema version so all data is invalidated,
or invalidate all foreign keys that point to this row, except a permanent list 
of the deleted key must be kept so updates to the table will reflect the change.
9) The first table specified in the JOIN clause must be the left most table of the JOIN. And all other tables in the SELECT must 
have some kind of join to the main table. Chaining is permitted.

