# Job Search Aggregator

A full-stack application that allows users to search for jobs using the **Adzuna** and **Remotive** APIs. The backend is built with **.NET**, and the frontend is a **React + TypeScript** app. Users can filter results by keywords, location, salary, and posting date.

---

## Features

- Search for jobs using keywords
- Filter by location
- Filter by salary range
- Filter by how recently jobs were posted
- Combine results from multiple job platforms (Adzuna, Remotive)

---

## Tech Stack

- **Backend:** ASP.NET Core (.NET 7/8)  
- **Frontend:** React + TypeScript + Vite  
- **APIs Used:** [Adzuna](https://developer.adzuna.com/) and [Remotive](https://remotive.com/api-documentation)  

---

## Project Structure

```text
JobSearchProject/
├── backend/                  # .NET backend project
│   ├── Program.cs
│   ├── Models/
│   ├── Services/
│   └── Controllers/
├── job-search-frontend/      # React frontend
├── .gitignore
└── LICENSE
```
---

## Getting Started

Prerequisites

.NET SDK (v 10)
Node.js (version 20.19+ or 22.12+)
npm (comes with Node.js)

---

## Usage

(Coming soon: Open the frontend in your browser (http://localhost:5173))

Use the search bar to enter keywords and optional location

Apply filters like salary range and posting date

View combined results from Adzuna and Remotive

---

## Contributing

Fork the repository

Create a branch: git checkout -b feature-name

Make changes and commit: git commit -m "Add feature"

Push to your branch: git push origin feature-name

Create a Pull Request

---

## License

This project is licensed under the MIT License
