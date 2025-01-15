import { Button, Grid2, Paper, TextField } from "@mui/material";

const FormTransaction = () => {
      return (
        <Grid2 container spacing={2} justifyContent="center" style={{ marginTop: "20px" }}>
          <Grid2 item xs={12} md={6}>
            <Paper elevation={3} style={{ padding: "20px" }}>
              <h2>Transaction Form</h2>
              <form>
                <TextField
                  label="Account Number"
                  name="accountNumber"
                  fullWidth
                  margin="normal"
                  required
                />
                <TextField
                  label="Transaction Type (Deposit/Withdrawal)"
                  name="transactionType"
                  fullWidth
                  margin="normal"
                  required
                />
                <TextField
                  label="Amount"
                  name="amount"
                  fullWidth
                  margin="normal"
                  type="number"
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