## :dollar: CashMapper:  A .NET personal finance application. :moneybag:
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
___

### Features
Several types of financial items can be managed within CashMapper. Some of these items
are categorized, others simply help keep track of financial information. These include:

- **Transactions**: A master list of transactional data usually from a bank account transaction
   history or a credit card (a 'source'). Transactions are assigned a *category* to be tracked and
   then compared against assumptions (*Expense Items, Budget Items, Income Profiles*).

   Transactions are meant to represent any event that modifies a user's wealth: be it positively or negatively.
   These can be considered a master ledger of 'wealth activity' and should capture income events,
   payments, expenses, etc. 
   They are not directly tied to any type of account, but can be tracked by a 'source' property.

- **Account Profiles**: Represents some type of financial account, such as a savings account or bank account.
    These are used for tracking *Cash Flow Items*.

- **Cash Flow Items**: A recording of a bank account's balance at a specific instance in time. Associated
    with a given *Account Profile*. This data is not categorized by a *category*.

- **Income Profiles**: Represents any source of income. Ex: "Day Job".
     Income profiles are assigned a single category to track them in transactions. 

- **Income Items**: An item tied to a specific *Income Profile* that contains a value (positive or negative).
    The sum of these items' value for a given *Income Profile* should result to the profile's NET income. 
    Items could mirror a user's paystub breakdown. 
    For example: "STATE TAXES" may be an item. "WAGES" may be another item. Income Items serve as a way
    for a user to have an overview of how their .NET income is computed for a given profile.

- **Expense Items**: An anticipated expense, defined with a monthly-value. 
    This type of item is not confined to any profile, but is defined with a *category* 
    to be tracked in *Transactions*.


   
