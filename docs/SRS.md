# Trekster

Trekster is a simple and user-friendly personal finance tracker designed to optimize and streamline personal fund management.

# Requirements

## Functional Requirements

The application defines only one type of user (**User**).  
The user can:
- View the ratio of expenses to total income and balance to total income.
- Add transactions (expense or income).
- View their accounts and the balance of each.
- Add new accounts.
- View transaction history.
- Edit transaction history.
- View monthly income reports.
- View monthly expense reports.
- Configure accounts and categories (deleting and editing).
- Add new categories.

### Description of Main Windows (Wireframes)

#### 1. Home Page

<p align="center">
	<img src="https://github.com/yvoznyak/Trekster/blob/main/docs/img/ui/main.jpg" alt="Home Page"/>
</p>

The start window that opens upon application launch.  
The top section shows the total balance across all accounts, summarized separately by different currencies (e.g., UAH, USD, EUR, USDT).  
The center of the window displays the share of total expenses relative to total income and the share of the remaining balance relative to total income as percentages.  
The left sidebar contains the navigation menu with the following items: Home, Accounts, History, Income, Expenses, Settings.  
At the bottom center, there is an "Add Transaction" button. Clicking it allows the user to add a new transaction.  
In the new window, the user selects the transaction type (expense or income via select box), the account (dropdown list), the category (dropdown list), and enters the amount (input):

<p align="center">
	<img src="https://github.com/yvoznyak/Trekster/blob/main/docs/img/ui/add%20trans.jpg" alt="Add Transaction"/>
</p>

#### 2. Accounts

<p align="center">
	<img src="https://github.com/yvoznyak/Trekster/blob/main/docs/img/ui/accounts.jpg" alt="Accounts"/>
</p>

The window opened by selecting the "Accounts" menu item.  
This page displays all accounts with balances grouped by currency in the format: <<Account Name: Amount + Currency>>.  
The left sidebar for navigation remains accessible.  
At the bottom, there is a button to add a new account.  
In the new window, the user enters the account name (input), selects currencies (checkbox), and enters initial amounts (input). A "Save" button at the bottom records the new account:

<p align="center">
	<img src="https://github.com/yvoznyak/Trekster/blob/main/docs/img/ui/add%20account.jpg" alt="Add Account"/>
</p>

#### 3. History

<p align="center">
	<img src="https://github.com/yvoznyak/Trekster/blob/main/docs/img/ui/history.jpg" alt="History"/>
</p>

The window opened by selecting the "History" menu item.  
This page displays all transactions in chronological order in the format: <<Date, Account Name, Category, Amount, Currency>>.  
Each transaction block is color-coded: blue for income and red for expense.  
Each transaction block also contains an "Edit" button on the right, which opens a new window for modifying transaction details:

<p align="center">
	<img src="https://github.com/yvoznyak/Trekster/blob/main/docs/img/ui/edit%20trans%201.jpg" alt="Edit Transaction"/>
</p>

The user can modify the account (dropdown list), category (dropdown list), and amount (input), and then save or delete the transaction using the buttons at the bottom.

#### 4. Income

<p align="center">
	<img src="https://github.com/yvoznyak/Trekster/blob/main/docs/img/ui/profit.jpg" alt="Income"/>
</p>

The window opened by selecting the "Income" menu item.  
This page displays total income by category with summarized balances by currency for the current month in the format: <<Category: Amount + Currency>>.  
A block at the bottom shows the summary for all currencies.

#### 5. Expenses

<p align="center">
	<img src="https://github.com/yvoznyak/Trekster/blob/main/docs/img/ui/expense.jpg" alt="Expenses"/>
</p>

The window opened by selecting the "Expenses" menu item.  
This page displays total expenses by category with summarized balances by currency for the current month in the format: <<Category: Amount + Currency>>.  
A block at the bottom shows the summary for all currencies.

#### 6. Settings

<p align="center">
	<img src="https://github.com/yvoznyak/Trekster/blob/main/docs/img/ui/settings%20main.jpg" alt="Settings"/>
</p>

The window opened by selecting the "Settings" menu item.  
This page displays two blocks/items: Accounts and Categories, which redirect the user to the corresponding configuration pages.

#### 7. Account Settings

<p align="center">
	<img src="https://github.com/yvoznyak/Trekster/blob/main/docs/img/ui/settings%20acc.jpg" alt="Account Settings"/>
</p>

The window opened by selecting the "Accounts" item within the Settings window.  
This page displays account blocks sequentially. On the right side, each block contains an "Edit" button that opens a new window to modify account details:

<p align="center">
	<img src="https://github.com/yvoznyak/Trekster/blob/main/docs/img/ui/edit%20acc.jpg" alt="Edit Account Details"/>
</p>

The user can change the account name (input) and the balance.  
Two buttons at the bottom—"Save" and "Delete"—execute the respective operations.

#### 8. Category Settings

<p align="center">
	<img src="https://github.com/yvoznyak/Trekster/blob/main/docs/img/ui/settings%20cat.jpg" alt="Category Settings"/>
</p>

The window opened by selecting the "Categories" item within the Settings window.  
This page displays category blocks sequentially. On the right side, each block contains an "Edit" button that opens a new window to modify category details:

<p align="center">
	<img src="https://github.com/yvoznyak/Trekster/blob/main/docs/img/ui/settings%20cat%201.jpg" alt="Edit Category Details"/>
</p>

The user can change the category name (input).  
"Save" and "Delete" buttons at the bottom execute the respective operations.  

Additionally, there is an "Add Category" button at the bottom of the main Categories window, which allows the user to add a new category in a popup window:

<p align="center">
	<img src="https://github.com/yvoznyak/Trekster/blob/main/docs/img/ui/add%20cat.jpg" alt="Add New Category"/>
</p>

The user enters the category name and saves or cancels the action.

## Use Case Diagram

<p align="center">
	<img src="https://github.com/yvoznyak/Trekster.desktop/blob/main/docs/img/Use%20case%20diagram.jpg" alt="Use case diagram"/>
</p>

## Non-functional Requirements
- **OS:** Windows 10
- **Interface Language:** Ukrainian
- **Design:** Static (Non-Responsive Design)
- **Connectivity:** The program does not require an Internet connection
