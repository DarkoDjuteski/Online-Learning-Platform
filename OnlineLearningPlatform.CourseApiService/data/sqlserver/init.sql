-- SQL Server init script

-- Create the OnlineLearningPlatformDB database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = N'OnlineLearningPlatformDB')
BEGIN
  CREATE DATABASE OnlineLearningPlatformDB;
END;
GO

USE OnlineLearningPlatformDB;
GO

CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY,
    Username NVARCHAR(50) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL
);

CREATE TABLE Course (
    CourseId INT PRIMARY KEY IDENTITY,
    Title NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL
);

CREATE TABLE Basket (
    BasketId INT PRIMARY KEY IDENTITY,
    UserId INT NOT NULL,
    CourseId INT NOT NULL,
    Quantity INT NOT NULL,
    FOREIGN KEY (CourseId) REFERENCES Course(CourseId),
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

CREATE TABLE Payment (
    PaymentId INT PRIMARY KEY IDENTITY,
    UserId INT NOT NULL,
    Amount DECIMAL(10, 2) NOT NULL,
    PaymentDate DATETIME NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);


CREATE TABLE CustomerBasket
(
    Id INT PRIMARY KEY IDENTITY,
    BuyerId NVARCHAR(50) NOT NULL,
    TotalItemCount INT NOT NULL DEFAULT 0
);

CREATE TABLE BasketItem
(
    Id INT PRIMARY KEY IDENTITY,
    ProductId INT NOT NULL,
    UnitPrice DECIMAL(18, 2) NOT NULL,
    OldUnitPrice DECIMAL(18, 2) NOT NULL,
    Quantity INT NOT NULL,
    CustomerBasketId INT NOT NULL,
    FOREIGN KEY (CustomerBasketId) REFERENCES CustomerBasket(Id)
);
