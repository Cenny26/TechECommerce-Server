# TechECommerce-Server

**TechECommerce (Technology E-Commerce)** is a backend application for an e-commerce platform focusing on technological products. This project is currently in development and has been in progress for about a month.

## Project Overview

TechECommerce aims to provide a robust backend for managing and selling tech products. It is built using **C#** and leverages the latest **.NET 8.0** framework. Adhering to **SOLID principles** and **design patterns**, the project emphasizes clean and maintainable code. Throughout the development process, notes and TODOs are included for easier future revisions.

> [!IMPORTANT]
> **Since the project is currently in the development stage, it is inevitable that there will be developments and modifications in the project over time!**

## Features & Technology Stack

- **Onion Architecture**: Followed the principles of onion architecture, promoting modular and maintainable code.
- **Entity Framework Core**: Entity Framework Core was released as an ORM tool. Used for database operations, ensuring efficient data handling.
- **PostgreSQL**: PostgreSQL was preferred as the database server, and various behavior db modeling (**TPH**) was used where necessary.
- **Repository Pattern**: A repository pattern according to SOLID principles is integrated as much as possible. Implemented both [`ReadRepository`](https://github.com/Cenny26/TechECommerce-Server/blob/master/src/Infrastructure/TechECommerceServer.Persistence/Concretes/Repositories/ReadRepository.cs) and [`WriteRepository`](https://github.com/Cenny26/TechECommerce-Server/blob/master/src/Infrastructure/TechECommerceServer.Persistence/Concretes/Repositories/WriteRepository.cs) to support CQRS and Mediator patterns, enhancing data manipulation capabilities.
- **Redis (*for cache*)**: Employed Redis for caching data, enhancing performance by reducing database loads ([especially for listing all products](https://github.com/Cenny26/TechECommerce-Server/blob/master/src/Infrastructure/TechECommerceServer.Infrastructure/Services/Cache/RedisCacheService.cs)).
- **Storage Services With Solid Infrastructure**: Using the benefits of object-oriented programming, a storage service with a solid infrastructure was created. As a result, all services had storage services with the same infrastructure, but working in a unique way. In the mentioned project, it is possible to store only [`locally`](https://github.com/Cenny26/TechECommerce-Server/blob/master/src/Infrastructure/TechECommerceServer.Infrastructure/Services/Storage/Local/LocalStorage.cs) and via [`Azure Portal (cloud)`](https://github.com/Cenny26/TechECommerce-Server/blob/master/src/Infrastructure/TechECommerceServer.Infrastructure/Services/Storage/Azure/AzureStorage.cs).
- **SEO Compatibility**: Appropriate algorithms and procedures are written to ensure SEO compliance for files (images and others) of products.
- **Microsoft identity platform**: Microsoft Identity is used for authentication process.
- **JSON Web Token**: Integrated JWT [(token handler)](https://github.com/Cenny26/TechECommerce-Server/blob/master/src/Infrastructure/TechECommerceServer.Infrastructure/Services/Token/TokenHandler.cs) for secure authentication and authorization processes, ensuring API security.
- **OAuth Login**: It is possible to access the application through external providers such as [`Google`](https://github.com/Cenny26/TechECommerce-Server/blob/master/src/Core/TechECommerceServer.Application/Features/Commands/AppUser/GoogleLogInAppUser) and [`Facebook`](https://github.com/Cenny26/TechECommerce-Server/tree/master/src/Core/TechECommerceServer.Application/Features/Commands/AppUser/FacebookLogInAppUser).
- **Logging**: Serilog library is used to log actions in according service, controllers (`PostgreSQL` (db-server) sink, `Seq` sink).
- **Base Structures**: Incorporated [base](https://github.com/Cenny26/TechECommerce-Server/tree/master/src/Core/TechECommerceServer.Application/Bases) structures such as [`BaseHandler`], [`BaseException`], and [`BaseRule`] for consistent and efficient code organization.
- **Custom Exception Handling**: Developed [`custom exception handlers`](https://github.com/Cenny26/TechECommerce-Server/blob/master/src/Core/TechECommerceServer.Application/Exceptions/ExceptionMiddleware.cs) for improved error management and user experience (for any client-side application).
- **Validation Rules**: Defined validation rules for each entity and structure, ensuring data consistency and integrity.

## Contribution

> [!NOTE]
> Contributions are welcome! Feel free to open issues or pull requests for any enhancements, bug fixes, or suggestions.
