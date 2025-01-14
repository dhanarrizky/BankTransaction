-- Menambahkan data dummy ke tabel Account
INSERT INTO Account (AccountNumber, Balance)
VALUES 
('ACC001', 5000.00),
('ACC002', 10000.00),
('ACC003', 15000.00),
('ACC004', 20000.00);

-- Menambahkan data dummy ke tabel Transaction_Type
INSERT INTO Transaction_Type (name)
VALUES 
('Deposit'),
('Withdrawal');

-- Menambahkan data dummy ke tabel Transaction (akan digenerate TransactionID menggunakan trigger)
INSERT INTO [Transaction] (AccountNumber, TransactionType, Amount)
VALUES 
('ACC001', 'Deposit', 1000.00),
('ACC002', 'Withdrawal', 500.00),
('ACC003', 'Deposit', 2000.00),
('ACC004', 'Withdrawal', 1500.00),
('ACC001', 'Deposit', 2500.00);
