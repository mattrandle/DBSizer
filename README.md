DBSizer
=======

SQL Server database size estimator is an application that reads schema information from a SQL Server database and creates a spreadsheet that estimates the physical size of the database given a number of rows per table. The database size is calculated by summing the estimated size of each table and index in the database.  

The output is loosely based on the spreadsheet created by Reuben Sultana in this blog post - http://sqlserverdiaries.com/blog/index.php/2011/05/estimating-the-size-of-an-sql-server-database/  

For columns with variable length datatypes, the application can calculate an average fill length from existing data.  

I tested it against SQL 2005 and 2008 but it should work with 2008 R2 and 2012.  

It handles all Sql Server datatypes, including custom, binary, Sql variant, and UDT’s

