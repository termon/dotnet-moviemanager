# DotNet Movie Manager Solution

This .NET project demonstrates concept of separation of concerns (SOC) and is a simple implementation of the Clean Architecture demonstrated via a movie management system.

## Core Project

The Core project contains all domain entities and has no dependencies.

Password hashing functionality added via the ```MovieManager.Core.Security.Hasher``` class. This is used in the Data project UserService to hash the user password before storing in database.

## Data Project

The Data project encapsulates all data related concerns.
```IUserService``` provides user management.
```IMovieService``` provides movie management.
```IMailService``` provides email sending functionality

The Services are the only element exposed from this project and consumers of this project simply need reference them to access their functionalty.

## Test Project

The Test project references the Core and Data projects and should implement unit tests to test the functionalty of the Data project services

## Web Project

Provides functionality to

1. User management (Login/Register/Forgotpassword/Update profile etc).
2. Movie management (CRUD operations)

## Implementations

The application has been implemented in

- [NextJS](https://github.com/termon/nextjs-moviemanager)
- [SpringBoot](https://github.com/termon/springboot-moviemanager)
- [Laravel](https://github.com/termon/laravel-moviemanager)
- [.NET](https://github.com/termon/dotnet-moviemanager)