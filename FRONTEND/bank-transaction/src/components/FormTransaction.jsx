import { Button, Grid2, MenuItem, Paper, Select, TextField } from "@mui/material";
import { useEffect, useState } from "react";
import TransactionServices from "../services/TransactionServices";


const FormTransaction = () => {
  const [formState, setFormState] = useState({
    accountNumber: "",
    transactionType: "Deposit",
    amount: "",
  });
  
  useEffect(() => {
    console.log('form state : ', formState);
  },[formState])
  
  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormState((prevState) => ({
      ...prevState,
      [name]: value,
    }));
  };

  const fetchNewTransactions = async () => {
    try {
      const data = await TransactionServices.createTransaction(formState);
      console.log("data : ", data)
    } catch (error) {
      console.error("Error fetching transactions:", error);
    } finally {
      console.error("Finally");
    }
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    fetchNewTransactions();
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
              value={formState.accountNumber}
              onChange={handleChange}
              required
              />
            <Select
              name="transactionType"
              fullWidth
              required
              value={formState.transactionType}
              onChange={handleChange}
              >
              <MenuItem value="Deposit">Deposit</MenuItem>
              <MenuItem value="Withdrawal">Withdrawal</MenuItem>
            </Select>
            <TextField
              label="Amount"
              name="amount"
              fullWidth
              margin="normal"
              type="number"
              value={formState.amount}
              onChange={handleChange}
              required
              />
            <Button type="submit" variant="contained" color="primary" fullWidth>
              Submit
            </Button>
          </form>
        </Paper>
      </Grid2>
    </Grid2>
  );
}

export default FormTransaction;