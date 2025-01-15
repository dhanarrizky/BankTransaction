import React, { useState } from "react";
import { Grid2, Paper, TextField, Button, Typography, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from "@mui/material";

const TransactionDataTable = () => {
  const [accountNumber, setAccountNumber] = useState("");
  const [transactions, setTransactions] = useState([]);
  const [showTable, setShowTable] = useState(false);

  const handleSubmit = (e) => {
    e.preventDefault();
    if (accountNumber) {
      const dummyTransactions = [
        { transactionId: "T001", transactionType: "Deposit", amount: 1000 },
        { transactionId: "T002", transactionType: "Withdrawal", amount: 500 },
      ];
      setTransactions(dummyTransactions);
      setShowTable(true);
    }
  };

  return (
    <Grid2 container spacing={2} justifyContent="center" style={{ marginTop: "20px" }}>
      <Grid2 item xs={12} md={6}>
        <Paper elevation={3} style={{ padding: "20px" }}>
          <h2>Transaction Form</h2>
          <form onSubmit={handleSubmit}>
            <TextField
              label="Account Number"
              name="accountNumber"
              fullWidth
              margin="normal"
              required
              value={accountNumber}
              onChange={(e) => setAccountNumber(e.target.value)}
            />
            <Button type="submit" variant="contained" color="primary" fullWidth>
              Show Transaction History
            </Button>
          </form>
        </Paper>
      </Grid2>

      {showTable && (
        <Grid2 item xs={12} md={8} style={{ marginTop: "20px" }}>
          <h2>Transaction History</h2>
          <TableContainer component={Paper}>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell>Transaction ID</TableCell>
                  <TableCell>Transaction Type</TableCell>
                  <TableCell>Amount</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {transactions.map((transaction) => (
                  <TableRow key={transaction.transactionId}>
                    <TableCell>{transaction.transactionId}</TableCell>
                    <TableCell>{transaction.transactionType}</TableCell>
                    <TableCell>{transaction.amount}</TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        </Grid2>
      )}
    </Grid2>
  );
};

export default TransactionDataTable;
