CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `AspNetRoles` (
    `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Name` varchar(256) CHARACTER SET utf8mb4 NULL,
    `NormalizedName` varchar(256) CHARACTER SET utf8mb4 NULL,
    `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AspNetRoles` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `AspNetUsers` (
    `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `FirstName` longtext CHARACTER SET utf8mb4 NULL,
    `LastName` longtext CHARACTER SET utf8mb4 NULL,
    `CreatedAt` datetime(6) NOT NULL,
    `UpdatedAt` datetime(6) NOT NULL,
    `DeletedAt` datetime(6) NOT NULL,
    `UserName` varchar(256) CHARACTER SET utf8mb4 NULL,
    `NormalizedUserName` varchar(256) CHARACTER SET utf8mb4 NULL,
    `Email` varchar(256) CHARACTER SET utf8mb4 NULL,
    `NormalizedEmail` varchar(256) CHARACTER SET utf8mb4 NULL,
    `EmailConfirmed` tinyint(1) NOT NULL,
    `PasswordHash` longtext CHARACTER SET utf8mb4 NULL,
    `SecurityStamp` longtext CHARACTER SET utf8mb4 NULL,
    `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 NULL,
    `PhoneNumber` longtext CHARACTER SET utf8mb4 NULL,
    `PhoneNumberConfirmed` tinyint(1) NOT NULL,
    `TwoFactorEnabled` tinyint(1) NOT NULL,
    `LockoutEnd` datetime(6) NULL,
    `LockoutEnabled` tinyint(1) NOT NULL,
    `AccessFailedCount` int NOT NULL,
    CONSTRAINT `PK_AspNetUsers` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `CategoryForDish` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Name` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `CreatedAt` datetime(6) NULL,
    `UpdatedAt` datetime(6) NOT NULL,
    `DeletedAt` datetime(6) NULL,
    CONSTRAINT `PK_CategoryForDish` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `CategoryForIngredient` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Priority` int NOT NULL,
    `Name` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `CreatedAt` datetime(6) NULL,
    `UpdatedAt` datetime(6) NOT NULL,
    `DeletedAt` datetime(6) NULL,
    CONSTRAINT `PK_CategoryForIngredient` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Plan` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Week` int NOT NULL,
    `Year` int NOT NULL,
    `Active` tinyint(1) NOT NULL,
    CONSTRAINT `PK_Plan` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Unit` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Name` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `CreatedAt` datetime(6) NULL,
    `UpdatedAt` datetime(6) NOT NULL,
    `DeletedAt` datetime(6) NULL,
    CONSTRAINT `PK_Unit` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `AspNetRoleClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `RoleId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ClaimType` longtext CHARACTER SET utf8mb4 NULL,
    `ClaimValue` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AspNetRoleClaims` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `AspNetUserClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ClaimType` longtext CHARACTER SET utf8mb4 NULL,
    `ClaimValue` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AspNetUserClaims` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `AspNetUserLogins` (
    `LoginProvider` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ProviderKey` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ProviderDisplayName` longtext CHARACTER SET utf8mb4 NULL,
    `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_AspNetUserLogins` PRIMARY KEY (`LoginProvider`, `ProviderKey`),
    CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `AspNetUserRoles` (
    `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `RoleId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_AspNetUserRoles` PRIMARY KEY (`UserId`, `RoleId`),
    CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `AspNetUserTokens` (
    `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `LoginProvider` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Name` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Value` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AspNetUserTokens` PRIMARY KEY (`UserId`, `LoginProvider`, `Name`),
    CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Dish` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Description` longtext CHARACTER SET utf8mb4 NULL,
    `Procedure` longtext CHARACTER SET utf8mb4 NULL,
    `Comment` longtext CHARACTER SET utf8mb4 NULL,
    `CategoryForDishID` char(36) COLLATE ascii_general_ci NULL,
    `Name` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `CreatedAt` datetime(6) NULL,
    `UpdatedAt` datetime(6) NOT NULL,
    `DeletedAt` datetime(6) NULL,
    CONSTRAINT `PK_Dish` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Dish_CategoryForDish_CategoryForDishID` FOREIGN KEY (`CategoryForDishID`) REFERENCES `CategoryForDish` (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Ingredient` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `CategoryForIngredientID` char(36) COLLATE ascii_general_ci NULL,
    `Name` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `CreatedAt` datetime(6) NULL,
    `UpdatedAt` datetime(6) NOT NULL,
    `DeletedAt` datetime(6) NULL,
    CONSTRAINT `PK_Ingredient` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Ingredient_CategoryForIngredient_CategoryForIngredientID` FOREIGN KEY (`CategoryForIngredientID`) REFERENCES `CategoryForIngredient` (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `DishPlan` (
    `DishID` char(36) COLLATE ascii_general_ci NOT NULL,
    `PlanID` char(36) COLLATE ascii_general_ci NOT NULL,
    `DayOfWeek` int NOT NULL,
    `Comment` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_DishPlan` PRIMARY KEY (`DishID`, `PlanID`),
    CONSTRAINT `FK_DishPlan_Dish_DishID` FOREIGN KEY (`DishID`) REFERENCES `Dish` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_DishPlan_Plan_PlanID` FOREIGN KEY (`PlanID`) REFERENCES `Plan` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `DishIngredient` (
    `DishID` char(36) COLLATE ascii_general_ci NOT NULL,
    `IngredientID` char(36) COLLATE ascii_general_ci NOT NULL,
    `Amount` double NOT NULL,
    `Unit` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_DishIngredient` PRIMARY KEY (`IngredientID`, `DishID`),
    CONSTRAINT `FK_DishIngredient_Dish_DishID` FOREIGN KEY (`DishID`) REFERENCES `Dish` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_DishIngredient_Ingredient_IngredientID` FOREIGN KEY (`IngredientID`) REFERENCES `Ingredient` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_AspNetRoleClaims_RoleId` ON `AspNetRoleClaims` (`RoleId`);

CREATE UNIQUE INDEX `RoleNameIndex` ON `AspNetRoles` (`NormalizedName`);

CREATE INDEX `IX_AspNetUserClaims_UserId` ON `AspNetUserClaims` (`UserId`);

CREATE INDEX `IX_AspNetUserLogins_UserId` ON `AspNetUserLogins` (`UserId`);

CREATE INDEX `IX_AspNetUserRoles_RoleId` ON `AspNetUserRoles` (`RoleId`);

CREATE INDEX `EmailIndex` ON `AspNetUsers` (`NormalizedEmail`);

CREATE UNIQUE INDEX `UserNameIndex` ON `AspNetUsers` (`NormalizedUserName`);

CREATE UNIQUE INDEX `IX_CategoryForDish_Name` ON `CategoryForDish` (`Name`);

CREATE UNIQUE INDEX `IX_CategoryForIngredient_Name` ON `CategoryForIngredient` (`Name`);

CREATE INDEX `IX_Dish_CategoryForDishID` ON `Dish` (`CategoryForDishID`);

CREATE UNIQUE INDEX `IX_Dish_Name` ON `Dish` (`Name`);

CREATE INDEX `IX_DishIngredient_DishID` ON `DishIngredient` (`DishID`);

CREATE INDEX `IX_DishPlan_PlanID` ON `DishPlan` (`PlanID`);

CREATE INDEX `IX_Ingredient_CategoryForIngredientID` ON `Ingredient` (`CategoryForIngredientID`);

CREATE UNIQUE INDEX `IX_Ingredient_Name` ON `Ingredient` (`Name`);

CREATE UNIQUE INDEX `IX_Unit_Name` ON `Unit` (`Name`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20211231163941_Identity+Foodplan', '6.0.1');

COMMIT;

