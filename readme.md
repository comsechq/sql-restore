## SQL Restore Utility

A command line utility to restore each .bak file present in a directory to a MS SQL Server.
Existing databases are dropped if they already exist.

#### Usage:

    sqlrest.exe restore [server] --src [source] --mdfPath [mdf] --ldfPath [ldf]

 * __server__ The host name of the SQL server. Uses trusted authentication
 * __source__ The source directory where the backup files are located
 * __mdf__ The destination directory to restore the data (.mdf) files to
 * __ldf__ The destination directory to restore the log (.ldf) files to

#### Todo:

* Add support for non-trusted connections
* Add options for file de-duplication checks (via timestamp, size). Currently, the most recent (and largest file) is chosen if duplicates exist.
* Add option to not overwrite existing databases

#### Download:

Checkout the [latest releases](https://github.com/comsechq/sql-restore/releases).

#### License

This project is licensed under the terms of the [MIT license](https://github.com/comsechq/sql-prune/blob/master/LICENSE.txt). 

By submitting a pull request for this project, you agree to license your contribution under the MIT license to this project.
