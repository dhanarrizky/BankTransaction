import { useState } from "react";
import { Grid2, Paper, TextField, Button, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from "@mui/material";
import TransactionServices from "../services/TransactionServices";

const TransactionDataTable = () => {
  const [accountNumber, setAccountNumber] = useState("");
  const [transactions, setTransactions] = useState([]);
  const [columns, setColumns] = useState([]);
  const [showTable, setShowTable] = useState(false);

  const fetchTransactionsHistory = async () => {
    try {
      let newCol = [];
      const data = await TransactionServices.getTransactions(accountNumber);
      const dataArr = data?.data;
      setTransactions(dataArr);
      for(let key in data?.data[0]) {
        newCol.push(key);
      };
      setColumns(newCol);
    } catch (error) {
      console.error("Error fetching transactions:", error);
    } finally {
      console.error("finnally");
    }
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    if (accountNumber) {
      setShowTable(true);
      fetchTransactionsHistory()
    }
  };
  
  const handleReset = () => {
    setShowTable(false);
    setTransactions([])
    setColumns([])
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
            {/* <Box fullWidth sx={{ display: 'flex', flexDirection: 'column', gap: 1.5, marginTop: '1.5rem', bgcolor:'blue' }}>
            </Box> */}
              <Button
                type="submit"
                variant="contained"
                color="primary"
                fullWidth
                sx={{
                  marginTop:'1rem'
                }}
                >
                Show Transaction History
              </Button>
              <Button
                type="button"
                variant="contained"
                fullWidth
                onClick={() => handleReset()}
                sx={{
                  backgroundColor: 'grey.500',
                  '&:hover': {
                    backgroundColor: 'grey.700',
                  },
                  marginTop:'1rem'
                }}
              >
                Reset
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
                  {columns.map((col, index) => (
                    <TableCell key={index}>{col}</TableCell>
                  ))}
                </TableRow>
              </TableHead>
              <TableBody>
                {transactions.map((transaction, index) => (
                  <TableRow key={index}>
                    {columns.map((col, colIndex) => (
                      <TableCell key={colIndex}>{transaction[col]}</TableCell>
                    ))}
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
