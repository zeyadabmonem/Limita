/* =========================================================
   Limita Digital Wallet — MVP Database Schema
   SQL Server
   Generated to match Limita.Data EF Core model 1:1
   ========================================================= */

IF DB_ID('LimitaDb') IS NULL
BEGIN
    CREATE DATABASE LimitaDb;
END
GO

USE LimitaDb;
GO

/* ---------------------------------------------------------
   Users
   --------------------------------------------------------- */
CREATE TABLE Users (
    Id              INT IDENTITY(1,1)   NOT NULL,
    FullName        NVARCHAR(150)       NOT NULL,
    Email           NVARCHAR(256)       NOT NULL,
    PhoneNumber     NVARCHAR(20)        NOT NULL,
    PasswordHash    NVARCHAR(255)       NOT NULL,
    ProfileImage    NVARCHAR(500)       NULL,
    Language        NVARCHAR(10)        NOT NULL DEFAULT ('en'),
    Currency        NVARCHAR(10)        NOT NULL DEFAULT ('USD'),
    IsActive        BIT                 NOT NULL DEFAULT (1),
    CreatedAt       DATETIME2           NOT NULL DEFAULT (SYSUTCDATETIME()),
    UpdatedAt       DATETIME2           NULL,
    CONSTRAINT PK_Users PRIMARY KEY (Id),
    CONSTRAINT UQ_Users_Email UNIQUE (Email),
    CONSTRAINT UQ_Users_PhoneNumber UNIQUE (PhoneNumber)
);
GO

/* ---------------------------------------------------------
   Accounts
   --------------------------------------------------------- */
CREATE TABLE Accounts (
    Id              INT IDENTITY(1,1)   NOT NULL,
    UserId          INT                 NOT NULL,
    Balance         DECIMAL(18,2)       NOT NULL DEFAULT (0),
    Currency        NVARCHAR(10)        NOT NULL,
    Status          NVARCHAR(20)        NOT NULL DEFAULT ('Active'), -- Active | Frozen | Closed
    CreatedAt       DATETIME2           NOT NULL DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT PK_Accounts PRIMARY KEY (Id),
    CONSTRAINT FK_Accounts_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    CONSTRAINT CK_Accounts_Balance_NonNegative CHECK (Balance >= 0),
    CONSTRAINT CK_Accounts_Status CHECK (Status IN ('Active','Frozen','Closed'))
);
CREATE INDEX IX_Accounts_UserId ON Accounts(UserId);
GO

/* ---------------------------------------------------------
   Cards
   --------------------------------------------------------- */
CREATE TABLE Cards (
    Id              INT IDENTITY(1,1)   NOT NULL,
    AccountId       INT                 NOT NULL,
    CardName        NVARCHAR(100)       NOT NULL,
    LastFourDigits  CHAR(4)             NOT NULL,
    Brand           NVARCHAR(20)        NOT NULL,
    ExpiryDate      DATE                NOT NULL,
    Status          NVARCHAR(20)        NOT NULL DEFAULT ('Active'), -- Active | Frozen | Expired
    CreatedAt       DATETIME2           NOT NULL DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT PK_Cards PRIMARY KEY (Id),
    CONSTRAINT FK_Cards_Accounts FOREIGN KEY (AccountId) REFERENCES Accounts(Id) ON DELETE CASCADE,
    CONSTRAINT CK_Cards_Status CHECK (Status IN ('Active','Frozen','Expired')),
    CONSTRAINT CK_Cards_LastFourDigits CHECK (LastFourDigits NOT LIKE '%[^0-9]%')
);
CREATE INDEX IX_Cards_AccountId ON Cards(AccountId);
GO

/* ---------------------------------------------------------
   Beneficiaries
   --------------------------------------------------------- */
CREATE TABLE Beneficiaries (
    Id                  INT IDENTITY(1,1)   NOT NULL,
    UserId              INT                 NOT NULL,
    Name                NVARCHAR(150)       NOT NULL,
    AccountIdentifier   NVARCHAR(100)       NOT NULL,
    CreatedAt           DATETIME2           NOT NULL DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT PK_Beneficiaries PRIMARY KEY (Id),
    CONSTRAINT FK_Beneficiaries_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_Beneficiaries_User_AccountIdentifier UNIQUE (UserId, AccountIdentifier)
);
GO

/* ---------------------------------------------------------
   Bills
   --------------------------------------------------------- */
CREATE TABLE Bills (
    Id              INT IDENTITY(1,1)   NOT NULL,
    UserId          INT                 NOT NULL,
    ProviderName    NVARCHAR(150)       NOT NULL,
    BillNumber      NVARCHAR(100)       NOT NULL,
    Amount          DECIMAL(18,2)       NOT NULL,
    DueDate         DATE                NOT NULL,
    Status          NVARCHAR(20)        NOT NULL DEFAULT ('Unpaid'), -- Unpaid | Paid | Overdue
    CreatedAt       DATETIME2           NOT NULL DEFAULT (SYSUTCDATETIME()),
    PaidAt          DATETIME2           NULL,
    CONSTRAINT PK_Bills PRIMARY KEY (Id),
    CONSTRAINT FK_Bills_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    CONSTRAINT CK_Bills_Status CHECK (Status IN ('Unpaid','Paid','Overdue')),
    CONSTRAINT CK_Bills_Amount_Positive CHECK (Amount > 0)
);
CREATE INDEX IX_Bills_UserId ON Bills(UserId);
GO

/* ---------------------------------------------------------
   Transactions
   NOTE: FKs to Users/Accounts use NO ACTION (Restrict) to avoid
   multiple cascade paths conflicting with Accounts -> Users cascade.
   Deleting a user must go through the service layer, not a raw cascade.
   --------------------------------------------------------- */
CREATE TABLE Transactions (
    Id              INT IDENTITY(1,1)   NOT NULL,
    UserId          INT                 NOT NULL,
    AccountId       INT                 NOT NULL,
    BeneficiaryId   INT                 NULL,
    BillId          INT                 NULL,
    Type            NVARCHAR(20)        NOT NULL, -- Transfer | BillPayment | Deposit
    Amount          DECIMAL(18,2)       NOT NULL,
    Currency        NVARCHAR(10)        NOT NULL,
    Status          NVARCHAR(20)        NOT NULL DEFAULT ('Pending'), -- Pending | Completed | Failed
    Reference       NVARCHAR(50)        NOT NULL,
    Note            NVARCHAR(500)       NULL,
    CreatedAt       DATETIME2           NOT NULL DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT PK_Transactions PRIMARY KEY (Id),
    CONSTRAINT FK_Transactions_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE NO ACTION,
    CONSTRAINT FK_Transactions_Accounts FOREIGN KEY (AccountId) REFERENCES Accounts(Id) ON DELETE NO ACTION,
    CONSTRAINT FK_Transactions_Beneficiaries FOREIGN KEY (BeneficiaryId) REFERENCES Beneficiaries(Id) ON DELETE NO ACTION,
    CONSTRAINT FK_Transactions_Bills FOREIGN KEY (BillId) REFERENCES Bills(Id) ON DELETE NO ACTION,
    CONSTRAINT UQ_Transactions_Reference UNIQUE (Reference),
    CONSTRAINT CK_Transactions_Type CHECK (Type IN ('Transfer','BillPayment','Deposit')),
    CONSTRAINT CK_Transactions_Status CHECK (Status IN ('Pending','Completed','Failed')),
    CONSTRAINT CK_Transactions_Amount_Positive CHECK (Amount > 0)
);
CREATE INDEX IX_Transactions_UserId_CreatedAt ON Transactions(UserId, CreatedAt);
CREATE INDEX IX_Transactions_AccountId ON Transactions(AccountId);
GO

/* ---------------------------------------------------------
   Notifications
   --------------------------------------------------------- */
CREATE TABLE Notifications (
    Id              INT IDENTITY(1,1)   NOT NULL,
    UserId          INT                 NOT NULL,
    Title           NVARCHAR(150)       NOT NULL,
    Message         NVARCHAR(500)       NOT NULL,
    IsRead          BIT                 NOT NULL DEFAULT (0),
    CreatedAt       DATETIME2           NOT NULL DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT PK_Notifications PRIMARY KEY (Id),
    CONSTRAINT FK_Notifications_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);
CREATE INDEX IX_Notifications_UserId_IsRead ON Notifications(UserId, IsRead);
GO
