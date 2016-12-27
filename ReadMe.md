# FieldedText library

This library can be used to:

* Parse Fielded Text files
* Generate Fielded Text files
* Create Meta Files for Fielded Text files

## What is a Fielded Text file?

A fielded text file is a file that contains a tables of values.  Think CSV file.  However it can also be a file that has information in Fixed Width fields.

The structure of the data in the file is specified by a Meta file.  This is a small XML file which holds all the layout, structure and format of the data within the file.  With this file, a FieldedText reader can read the table(s) of values within a file without having to take into account how these values are stored in the file.  Likewise for writing tables of values.

## Under what licence is this code made available?

This FieldedText library has been released into the Public Domain.  You are free to use it however you like without any restrictions.

## Where can I find out more?

The FieldedText standard is described and documented at:  
[http://www.fieldedtext.org](http://www.fieldedtext.org)

Information about this library can be found at:  
[http://www.xilytix.com/fieldedtext/csharplibrary/index.html](http://www.xilytix.com/fieldedtext/csharplibrary/index.html)

Code examples can be viewed at:  
[http://www.xilytix.com/fieldedtext/csharplibrary/examples/](http://www.xilytix.com/fieldedtext/csharplibrary/examples/)
