##SQL Restore Utility

A simple utility to restore a directoy of backup files to a MS SQL Server.  Existing databases are dropped if they 
already exist.

####Usage:

    sqlrest.exe -server [server] -src [source] -dest [destination]

 * __server__ The host name of the SQL server. Uses trusted authentication.
 * __src__ The source directory where the backup files are located
 * __dest__ The destination directory to restore the data (.mdf) files to
 * __log-dest__ The destination directory to restore the log (.ldf) files to (optional: defaults to __dest__)

####Todo:

* Add support for non-trusted connections
* Add options for file de-duplication checks (via timestamp, size).  Currently, the latest, largest file is chosen  if duplicates exist.
* Add option to not overwrite existing databases

####Download:

Checkout the releases.