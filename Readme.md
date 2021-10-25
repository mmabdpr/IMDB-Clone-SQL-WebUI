# IMDB Clone

## Overview

This project mimics the main functionalities of IMDB website. The goal of this project is to learn and practice SQL and database design.

## Usage (Web UI)

1. install dotnet core 3.1 runtime (or sdk)
2. windows only: run P0.UI.exe
   linux, mac, windows:
       in terminal: dotnet P0.UI.dll
3. open browser and open url printed on console (default: http://localhost:5000)

## Notes (Web UI)

- Compiled & ready-to-run version of the project is located at "Publish" directory
- All data files are located at "Data/DB" directory. There is a back-up just in case. Do not edit it.
- A set of mock & completely random dataset have been provided. Check the formatting of each file and change it (if you want) very carefully.
- UI: You can double click on each table row to see full detail of any record
- UI: Search bar & sorting features of tables are processed at client side. Input textboxes below the tables are meant for satisfying the     goals of the project according to the given doc.
- To walk through & debug the code VisualStudio 2019 is the recommended IDE.
- The web framework: Blazor
- The UI framework: Radzen

## Usage (SQL Scripts)

Run the files in the following order: (ignore any error)

1. drop.sql
2. tables.sql
3. views.sql
4. functions.sql
5. triggers.sql
6. insert.sql

There is a set of prepopulated sql insert query files in the "pre" directory
howerver, if you want to regenerate the data do the 7th step otherwise skip it

7. random_data_gen.py (new sql files will be replaced in "pre" directory)
8. pre/post.sql (be patient :))
9. pre/**.sql (order doesn't matter)

Then run each query from "queries.sql" file to see the results.
(some of the results could be empty due to lack of data which satisfies the constraints)

Feel free to add data or run custom query in the console.
  