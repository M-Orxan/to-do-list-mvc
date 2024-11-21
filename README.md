# ToDoList

The **ToDoList** application is a simple yet powerful task management tool designed to help users organize and prioritize their daily activities. It provides a clean, intuitive interface for creating, updating, deleting, and managing tasks effectively. 

---

## Technologies Used

- **ASP.NET Core MVC**: Framework used to build the web application.
- **Entity Framework Core**: ORM for database operations.
- **MSSQL**: Database used for storing tasks and related data.

---

## Features

- **Task Management**: Create, update, delete, and mark tasks as completed to keep track of progress.
- **Task Details**: Each task includes a title, description, deadline, and notification date.
- **Deadline Filters**: Easily filter tasks by due dates, including the option to choose an "until" date.
- **Search Functionality**: Quickly search and find specific tasks.
- **Email Notifications**: Set notification dates for tasks to receive timely email reminders to ensure no deadlines are missed.
- **User-Friendly Design**: A clean and intuitive interface for seamless navigation and task management.

---

## Setup Instructions

Follow these steps to set up and run the ToDoList application locally:

1. **Clone the Repository**  
   Clone this repository to your local machine using the following command:
   ```bash
   git clone https://github.com/M-Orxan/to-do-list-mvc.git
   ```

2. **Install Dependencies**  
   Navigate to the project directory and restore the required dependencies:
   ```bash
   cd ToDoList
   dotnet restore
   ```

3. **Set up the Database**  
   Ensure you have MSSQL installed and running. Update your connection string in the `appsettings.json` file to match your database setup.

4. **Apply Migrations**  
   Apply the database migrations to set up the necessary tables:
   ```bash
   dotnet ef database update
   ```

5. **Run the Application**  
   Start the application using:
   ```bash
   dotnet run
   ```
   You can now open your browser and navigate to `http://localhost:5000` to view the ToDoList application.

---

## Contributing

We welcome contributions! To contribute to the development of this project, please follow these steps:

1. Fork the repository.
2. Create a new branch for your changes.
3. Make your changes and test them.
4. Commit your changes and push them to your forked repository.
5. Submit a pull request with a detailed explanation of your changes.
