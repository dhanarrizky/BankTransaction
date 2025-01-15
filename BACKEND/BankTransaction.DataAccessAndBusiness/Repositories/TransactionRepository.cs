using System.Data;
using BankTransaction.DataAccessAndBusiness.IRepositories;
using Microsoft.Data.SqlClient;

namespace BankTransaction.DataAccessAndBusiness.Repositories;
public class TransactionRepository : ITransactionRepository {
    private readonly SqlDatabaseService _sqlDatabaseService;

        public TransactionRepository(SqlDatabaseService sqlDatabaseService) {
            _sqlDatabaseService = sqlDatabaseService;
        }

        // LazyInitializer database

        private static string CreateBaseQueryCreateTable(string tableName, string query) {
            return $@"
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tableName}')
                BEGIN
                    {query}
                END";
        }

        public async Task<string> InitalizeDatabasesAsync() {
            try {
                string createDatabaseQuery = @"
                    IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'bank_transaction')
                    BEGIN
                        CREATE DATABASE bank_transaction;
                    END";

                string tableNameAccount = "Account";
                string tableQueryAccount = @"
                    IF OBJECT_ID('Account', 'U') IS NOT NULL
                        DROP TABLE Account;

                    CREATE TABLE Account (
                        AccountNumber VARCHAR(200) PRIMARY KEY,
                        Balance DECIMAL(38, 4)
                    );

                    CREATE UNIQUE INDEX idx_AccountNumber
                        ON Account (AccountNumber);
                    ";


                string createTableQueryAccount = CreateBaseQueryCreateTable(tableNameAccount, tableQueryAccount);

                string tableNameTransaction = "Transaction";
                string tableQueryTransaction = @"
                    IF OBJECT_ID('Transaction', 'U') IS NOT NULL
                        DROP TABLE Account;

                    CREATE TABLE [Transaction] (
                        Id BIGINT IDENTITY(1,1) UNIQUE,
                        TransactionID VARCHAR(200),
                        AccountNumber VARCHAR(200) NOT NULL,
                        TransactionType VARCHAR(15) CHECK (TransactionType IN ('Deposit','Withdrawal')) NOT NULL,
                        Amount DECIMAL(19, 4) CHECK (Amount > 0),
                        TransactionDate DATETIME DEFAULT GETDATE(),
                        FOREIGN KEY (AccountNumber) REFERENCES Account(AccountNumber) 
                            ON DELETE CASCADE 
                            ON UPDATE CASCADE
                    );

                    CREATE NONCLUSTERED INDEX idx_trx_AccountNumber
                        ON [Transaction] (AccountNumber);

                    CREATE UNIQUE INDEX idx_trx_TransactionID
                        ON [Transaction] (TransactionID);
                        ";

                string createTableQueryTransaction = CreateBaseQueryCreateTable(tableNameTransaction, tableQueryTransaction);

                string removeTriger = @"
                    IF OBJECT_ID('TriggerName', 'TR') IS NOT NULL
                        DROP TRIGGER TriggerName;
                    ";

                string createTriggerQuery = @"
                    CREATE TRIGGER transaction_id_gen
                    ON [Transaction]
                    AFTER INSERT
                    AS
                    BEGIN
                        SET NOCOUNT ON;

                        UPDATE t
                        SET t.TransactionID = CONCAT('T000', FORMAT(i.Id, '0000000000'))
                        FROM [Transaction] t
                        INNER JOIN inserted i ON t.Id = i.Id;
                    END;
                    ";

                await _sqlDatabaseService.ExecuteQueryAsync(createDatabaseQuery);
                await _sqlDatabaseService.ExecuteQueryAsync(createTableQueryAccount);
                await _sqlDatabaseService.ExecuteQueryAsync(createTableQueryTransaction);
                await _sqlDatabaseService.ExecuteQueryAsync(removeTriger);
                await _sqlDatabaseService.ExecuteQueryAsync(createTriggerQuery);

                return "Database and tables initialized successfully.";
            } catch (Exception ex) {
                throw new InvalidOperationException($"Initialization failed: {ex.Message}", ex);
            }

        }
        
        public async Task<string> InitializedDummyDataDatabaseAsync() {
            try {
                string addAccountDummyData = @"
                    INSERT INTO Account (AccountNumber, Balance)
                        VALUES 
                        ('ACC001', 5000.00),
                        ('ACC002', 10000.00),
                        ('ACC003', 15000.00),
                        ('ACC004', 20000.00);
                ";

                string addTransactionDummyData = @"
                    INSERT INTO [Transaction] (AccountNumber, TransactionType, Amount)
                        VALUES 
                        ('ACC001', 'Deposit', 1000.00);
                ";
                
                await _sqlDatabaseService.ExecuteQueryAsync(addAccountDummyData);
                await _sqlDatabaseService.ExecuteQueryAsync(addTransactionDummyData);

                return "DummyData initialized successfully.";
            } catch (Exception ex) {
                throw new InvalidOperationException($"Dummy Data Initialization failed: {ex.Message}", ex);
            }
        }

        public async Task<string> InitializedSPDataDatabaseAsync() {
            try {
                string createSpProcessTransaction = @"
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
                ";

                await _sqlDatabaseService.ExecuteQueryAsync(createSpProcessTransaction);

                return "Store Procedure initialized successfully.";
            } catch (Exception ex) {
                throw new InvalidOperationException($"Store Procedure Initialization failed: {ex.Message}", ex);
            }
        }
        

        // database opration

        public async Task<DataTable> GetAccountDetails(string accountNumber) {
            string query = "SELECT * FROM [Account] WHERE AccountNumber = @AccountNumber";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@AccountNumber", System.Data.SqlDbType.VarChar) 
                {
                    Value = accountNumber
                }
            };
            return await _sqlDatabaseService.ExecuteQueryWithParamsAsync(query,parameters);
        }

        public async Task<DataTable> GetTransationHistoryByAccountNumber(string accountNumber) {
            string query = @"SELECT
                                TransactionID,
                                AccountNumber,
                                TransactionType,
                                Amount,
                                TransactionDate
                            FROM [Transaction] WHERE AccountNumber = @AccountNumber";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@AccountNumber", System.Data.SqlDbType.VarChar) 
                {
                    Value = accountNumber
                }
            };
            return await _sqlDatabaseService.ExecuteQueryWithParamsAsync(query,parameters);
        }
        
        public async Task<DataTable> AddNewTransaction(string accountNumber, string transactionType, decimal amount) {
            string SpName = @"ProcessTransaction";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@AccountNumber", System.Data.SqlDbType.VarChar) {Value = accountNumber},
                new SqlParameter("@TransactionType", System.Data.SqlDbType.VarChar) {Value = transactionType},
                new SqlParameter("@Amount", System.Data.SqlDbType.VarChar) {Value = amount}
            };
            return await _sqlDatabaseService.ExecuteQuerySPWithParamsAsync(SpName,parameters);
        }
}