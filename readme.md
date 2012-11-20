###SQL Restore Utility
##Comsec Solutions - comsechq.com

A simple utility to restore a directoy of backup files to a MS SQL Server.

Usage:

    sqlrest.exe -server [server] -src [source] -dest [destination]

  server:   The host name of the SQL server. Uses trusted authentication.
     src:   The source directory where the backup files are located
    dest:   The destination directory to restore the backup files to