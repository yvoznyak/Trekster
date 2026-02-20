--Створення бази даних
CREATE DATABASE "trekster_db";

--Створення таблиці Рахунки (Accounts)
CREATE TABLE accounts (
	id SERIAL PRIMARY KEY,
	name VARCHAR (100) UNIQUE NOT NULL
);

--Створення таблиці Валюти (Currencies)
CREATE TABLE currencies (
	id SERIAL PRIMARY KEY,
	name VARCHAR (100) UNIQUE NOT NULL
);

--Створення таблиці Початкові баланси (Start Balances)
CREATE TABLE startBalances (
	id SERIAL PRIMARY KEY,
	idAccount INT NOT NULL,
	idCurrency INT NOT NULL,
	FOREIGN KEY (idAccount)
      REFERENCES accounts (id),
	FOREIGN KEY (idCurrency)
      REFERENCES currencies (id),
	sum REAL NOT NULL
);

--Створення домену для типу категорії
CREATE DOMAIN categoryType AS INT
CHECK(
	VALUE IN (-1, 1)
);

--Створення таблиці Категорії (Categories)
CREATE TABLE categories (
	id SERIAL PRIMARY KEY,
	name VARCHAR (100) UNIQUE NOT NULL,
	type categoryType NOT NULL
);

--Створення таблиці Транзакції (Transactions)
CREATE TABLE transactions (
	id SERIAL PRIMARY KEY,
	date TIMESTAMP NOT NULL,
	idAccount INT NOT NULL,
	idCurrency INT NOT NULL,
	idCategory INT NOT NULL,
	FOREIGN KEY (idAccount)
      REFERENCES accounts (id),
	FOREIGN KEY (idCurrency)
      REFERENCES currencies (id),
	FOREIGN KEY (idCategory)
      REFERENCES categories (id),
	sum REAL NOT NULL,
	note VARCHAR (100)
);