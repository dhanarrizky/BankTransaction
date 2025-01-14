-- must have
--  ● TransactionID (unik)
--  ● AccountNumber (nomor rekening nasabah)
--  ● TransactionType (Deposit atau Withdrawal)
--  ● Amount(jumlah transaksi)
--  ● TransactionDate (tanggal transaksi)


create database BankTransaction;
go

use BankTransaction;
go

create table Account (
    AccountNumber varchar(255) primary key,
    Balance decimal(38, 4)
)
go

create table Transaction_Type (
    Id int primary key identity(1,1),
    name varchar(15) not null
)
go

-- create table Transaction (
--     TransactionID int primary key identity(1,1),
--     AccountNumber varchar(255) not null,
--     TransactionType varchar(15) check (TransactionType in ('Deposit','Withdrawal')) not null,
--     Amount decimal(19, 4) check (Amount > 0),
--     TransactionDate datetime default getdate(),
--     foreign key (AccountNumber) references Account(AccountNumber) on delete cascade on update cascade
-- )
drop table [Transaction]
create table [Transaction] (
    Id bigint identity(1,1) unique,
    TransactionID varchar(200),
    AccountNumber varchar(255) not null,
    TransactionType varchar(15) check (TransactionType in ('Deposit','Withdrawal')) not null,
    Amount decimal(19, 4) check (Amount > 0),
    TransactionDate datetime default getdate(),
    foreign key (AccountNumber) references Account(AccountNumber) on delete cascade on update cascade
)
go

-- createing triger for TransactionID Generator
create trigger transaction_id_gen
on [Transaction]
after insert
as
begin
    update t
    set t.TransactionID = concat('T000', format(i.Id,'0000000000'))
    from [Transaction] t 
    inner join inserted i on t.Id = i.Id;
end