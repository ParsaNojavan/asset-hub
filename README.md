<p align="center">
  <img src="https://raw.githubusercontent.com/ParsaNojavan/asset-hub/main/frontend/asset-client/public/logo.png" width="180" />
</p>

<h1 align="center">Asset Hub</h1>

<p align="center">
A modern platform for managing, organizing and accessing digital assets in one centralized hub.
</p>

<p align="center">
Asset Hub is built with a modern full‑stack architecture combining <b>Next.js</b>, <b>.NET</b>, and <b>Microsoft SQL Server</b>, designed to provide a fast, scalable and maintainable asset management system.
</p>

---

## 🚀 Overview

Asset Hub is a full‑stack application designed to simplify the way teams manage digital resources and assets.  
It provides a clean UI, a scalable backend, and an architecture that can grow with larger systems.

The project focuses on:

- clean architecture
- scalable backend services
- modern frontend development
- asynchronous communication between services
- a polished UI with dark/light/system theme support

---

## ✨ Features

- 📁 Centralized asset management
- 🌗 Light / Dark / System theme support
- ⚡ Fast and responsive UI built with Next.js
- 🔐 Secure backend with .NET
- 🗄️ Reliable data storage using Microsoft SQL Server
- 📨 Event‑driven communication with RabbitMQ
- 🎨 Modern UI using Tailwind CSS
- 🧩 Modular and scalable architecture

---

## 🧱 Tech Stack

<p>

<img alt="NextJs" src="https://img.shields.io/badge/Next-black?style=for-the-badge&logo=next.js&logoColor=white" />
<img alt="TypeScript" src="https://img.shields.io/badge/typescript-%23007ACC.svg?style=for-the-badge&logo=typescript&logoColor=white" />
<img alt="Tailwind" src="https://img.shields.io/badge/tailwindcss-%2338B2AC.svg?style=for-the-badge&logo=tailwind-css&logoColor=white" />
<img alt="NodeJs" src="https://img.shields.io/badge/node.js-6DA55F?style=for-the-badge&logo=node.js&logoColor=white" />
<img alt="NPM" src="https://img.shields.io/badge/NPM-%23CB3837.svg?style=for-the-badge&logo=npm&logoColor=white" />

<br/>

<img alt=".NET" src="https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white" />
<img alt="C#" src="https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white" />
<img alt="Microsoft SQL Server" src="https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white" />
<img alt="RabbitMQ" src="https://img.shields.io/badge/rabbitmq-FF6600?style=for-the-badge&logo=rabbitmq&logoColor=white" />

</p>

---

## 📂 Project Structure

```
asset-hub
│
├── backend
│   ├── API (.NET)
│   ├── Services
│   └── Messaging / RabbitMQ
│
├── frontend
│   └── asset-client (Next.js + Tailwind)
│
└── database
    └── SQL Server scripts
```

---

## 🛠️ Getting Started

### 1️⃣ Clone the repository

```bash
git clone https://github.com/ParsaNojavan/asset-hub.git
cd asset-hub
```

---

### 2️⃣ Run the frontend

```bash
cd frontend/asset-client
npm install
npm run dev
```

Frontend will start on:

```
http://localhost:3000
```

---

### 3️⃣ Run the backend

Navigate to the backend project and run:

```bash
dotnet run
```

Make sure SQL Server and RabbitMQ are running and properly configured.

---

## 🌗 Theme System

Asset Hub includes a **three‑state theme system**:

- Light
- Dark
- System

The theme preference is saved in `localStorage` and automatically applied on page load, ensuring a smooth user experience without UI flickering.

---

## 📌 Future Improvements

- Asset versioning
- File preview support
- Role‑based access control
- Advanced search and filtering
- Cloud storage integration
- Activity logs and auditing

---

## 🤝 Contributing

Contributions, issues and feature requests are welcome.

If you'd like to improve the project, feel free to fork the repository and submit a pull request.

---

## 📄 License

This project is licensed under the MIT License.

---

<p align="center">
Built with ❤️ using modern web technologies
</p>
