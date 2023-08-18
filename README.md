# ProductSubscription

This is a C# Web API project for managing users, products, and subscriptions.

## Table of Contents

- [Introduction](#introduction)
    - [User Class](#user-class)
    - [Product Class](#product-class) 
- [API Endpoints](#api-endpoints)
    - [Users](#users)
    - [Products](#products)
- [Getting Started](#getting-started)
- [Running Unit Tests](#running-unit-tests)

## Introduction

This project provides a set of C# Web APIs that allow users to manage their products, subscribe to other users, and interact with the system. It can be run using a Swagger and has a set of Unit Tests to cover all the APIs.

Two classes were created for this:

### User Class

The `User` class represents a user in the system. It has the following properties:
- `Id` is a unique identifier for the user.
- `Name` is the name of the user.
- `ListSubscribedUsers` is a list of user IDs representing users that this user has subscribed to.
- `ListFollowers` is a list of user IDs representing users that are following this user.

### Product Class

The `Product` class represents a product and is associated with a user. It has the following properties:
- `Id` is a unique identifier for the product.
- `Name` is the name of the product.
- `CreatorUserId` is the ID of the user who created the product.
- `Price` is the price of the product.

## API Endpoints

### Users

- `GET /users/getAllUsers`: Retrieve a list of users.
- `GET /users/getUser/{userId}`: Retrieve details of a specific user.
- `GET /users/getAllSubscribedUsers/{userId}`: Retrieve the list of subscribed users.
- `GET /users/getAllFollowers/{userId}`: Retrieve the list of followers.
- `POST /users/createUser`: Create a new user.
- `DELETE /users/deleteUser/{userId}`: Delete a user.
- `PUT /users/subscribeUser/{userId}/{subscribedUserId}`: Subscribe to a user.
- `PUT /users/unsubscribeUser/{userId}/{subscribedUserId}`: Unsubscribe to a user.

### Products

- `GET /products/getAllProducts`: Retrieve a list of products.
- `GET /products/getProductById/{productId}`: Retrieve details of a specific product.
- `GET /products/getProductsFromUser/{userId}`: Retrieve the list of products of a certain user.
- `GET /products/getAllProductsFromSubscribedUsers/{userId}`: Retrieve the list of products of all subscribed users.
- `POST /products/createProduct`: Create a new product.
- `DELETE /products/deleteProduct/{productId}`: Delete a product.
- `PUT /products/updatePrice/{productId}`: Update the product price.

## Getting Started

1. Clone the repository.
2. Navigate to the project directory: `cd .\ProductSubscription\`.
3. Run the command `dotnet restore` to restore the project dependencies.
4. Start the API server: `dotnet run`.
5. Go to `http://localhost:5146/swagger` and execute the available endpoints.

## Running Unit Tests

1. Navigate to the project directory: `cd .\ProductSubscription.Tests\`.
2. Run the command `dotnet test`