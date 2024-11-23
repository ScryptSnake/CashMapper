## CashMapper:  A .NET personal finance application. :moneybag:
CashMapper is a .NET application that helps a user have a solid
view of their financial landscape. It relies on categorizing transaction data
to compare against budgets, income, and expenses. 
___
### :notebook:Project Overview
This repository contains 3 projects.
- **CashMapper (Class Library):** The application backend that performs database operations
    and parsing. A SQLite database is used for this application. Parsing is used
    to read transaction data from bank account data (.csv, .xls).

- **CashMapperWebApi (ASP.NET Core Web API):** A RESTful API that allows access to backend operations.
- **ConsoleDemoApp (Console App)**: Used for doing manual testing of the application.
- **TestProject (xUnit)**: Contains xUnit integration tests of the backend. 

###
___
