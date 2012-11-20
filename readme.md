##SQL Restore Utility

A simple utility to restore a directoy of backup files to a MS SQL Server.  Existing databases are dropped if they 
already exist.

####Usage:

    sqlrest.exe -server [server] -src [source] -dest [destination]

 * __server__ The host name of the SQL server. Uses trusted authentication.
 * __src__ The source directory where the backup files are located
 * __dest__ The destination directory to restore the backup files to

####Todo:

* Add support for non-trusted connections
* Add options for file de-duplication checks (via timestamp, size).  Currently, the latest, largest file is chosen 
  if duplicates exist.
* Add option to not overwrite existing databases

####Download:

Get the precompiled [version 0.1 here] [1].

[1]: https://github.com/downloads/comsechq/sql-restore/sqlrest.zip