# Trekster-desktop 💰

**Trekster-desktop** is a simple personal finance application for Windows. It provides a straightforward way to manage daily finances through a dedicated desktop interface.

---

## ✨ Features
* **Income & Expense Tracking:** Log and categorize your financial transactions.
* **History View:** Access a clear, chronological list of all recorded entries.
* **Database Integration:** Persistent data storage using PostgreSQL.
* **Logging System:** Application logging implemented with **log4net**.
* **Code Quality Control:** Source code adheres to standards via **StyleCop**.
* **Automated Testing:** Core logic covered by unit tests using **xUnit**.

## 🛠 Tech Stack
* **Language:** C#
* **Framework:** .NET 8.0 (WPF)
* **Database:** PostgreSQL
* **Unit Testing:** xUnit
* **Logging:** log4net
* **Static Analysis:** StyleCop.Analyzers

## 📂 Project Structure
* **`/src`** — Contains the source code, including `Trekster` and `Trekster_app` subfolders.
* **`/docs`** — Project documentation and supplementary materials.
  * `SRS.md` — Software Requirements Specification with detailed project requirements.
  * `Trekster.pdf` — Application presentation and overview.
  * **`/img`** — Supporting assets, including ER diagrams, UML schemas, and UI wireframes.

## ⚙️ How to Run
1. Clone the repository.
2. Ensure you have **PostgreSQL** installed and running.
3. Configure your connection string in the project settings.
4. Open the solution in **Visual Studio 2022**.
5. Build the solution and press `F5` to run.
