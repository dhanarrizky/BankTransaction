-- add new transaction
CREATE PROCEDURE ProcessTransaction
    @AccountNumber VARCHAR(200),
    @TransactionType VARCHAR(15),
    @Amount DECIMAL(19, 4)
AS
BEGIN
    BEGIN TRANSACTION;

    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Account WHERE AccountNumber = @AccountNumber)
        BEGIN
            RAISERROR('Account Not Found', 16, 1);
            RETURN -1;
        END

        IF @TransactionType NOT IN ('Deposit', 'Withdrawal')
        BEGIN
            RAISERROR('Invalid transaction type.', 16, 1);
            RETURN -1;
        END

        IF @TransactionType = 'Deposit'
        BEGIN
            UPDATE Account
            SET Balance = Balance + @Amount
            WHERE AccountNumber = @AccountNumber;
        END
        ELSE IF @TransactionType = 'Withdrawal'
        BEGIN
            IF (SELECT Balance FROM Account WHERE AccountNumber = @AccountNumber) < @Amount
            BEGIN
                RAISERROR('Insufficient balance.', 16, 1);
                RETURN -1;
            END

            UPDATE Account
            SET Balance = Balance - @Amount
            WHERE AccountNumber = @AccountNumber;
        END

        INSERT INTO [Transaction] (AccountNumber, TransactionType, Amount)
        VALUES (@AccountNumber, @TransactionType, @Amount);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
    
        ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000);
        DECLARE @ErrorSeverity INT;
        DECLARE @ErrorState INT;

        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR('Transaction failed: %s', @ErrorSeverity, @ErrorState, @ErrorMessage);
        RETURN -1;
    END CATCH
END;

GO;

-- get transaction Account Detail by accountNumber
CREATE PROCEDURE GetAccountDetails
    @AccountNumber VARCHAR(200)
AS
BEGIN
    BEGIN TRY
        SELECT AccountNumber, Balance
        FROM Account WHERE AccountNumber = @AccountNumber;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000);
        DECLARE @ErrorSeverity INT;
        DECLARE @ErrorState INT;

        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR('Transaction failed: %s', @ErrorSeverity, @ErrorState, @ErrorMessage);
    END CATCH
END;



GO;

-- get transaction history by accountNumber
CREATE PROCEDURE GetTransactionHistoryByAccountNumber
    @AccountNumber VARCHAR(200)
AS
BEGIN
    BEGIN TRY
        SELECT *
        FROM [Transaction] WHERE AccountNumber = @AccountNumber;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000);
        DECLARE @ErrorSeverity INT;
        DECLARE @ErrorState INT;

        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR('Transaction failed: %s', @ErrorSeverity, @ErrorState, @ErrorMessage);
    END CATCH
END;
