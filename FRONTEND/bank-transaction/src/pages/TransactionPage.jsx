import React, { useState } from "react";
import { Box, Button, Typography, Divider } from "@mui/material";
import FormTransaction from "../components/FormTransaction";
import TransactionDataTable from "../components/TransactionDataTable";

const TransactionPage = () => {
    const [isAddTransaction, setIsAddTransaction] = useState(true);
    const apiUrl = import.meta.env.REACT_APP_API_URL;
    console.log(apiUrl);

    return (
        <Box
        sx={{
            width: "100%",
            maxWidth: "800px",
            margin: "20px auto",
            padding: "20px",
            boxShadow: 3,
            borderRadius: 2,
            backgroundColor: "#fff",
        }}
        >
        <Box
            sx={{
            display: "flex",
            justifyContent: "space-between",
            marginBottom: 2,
            }}
        >
            <Button
                variant="contained"
                color="primary"
                onClick={() => setIsAddTransaction(true)}
            >
                Add New Transaction
            </Button>
            <Button
                variant="outlined"
                color="secondary"
                onClick={() => setIsAddTransaction(false)}
            >
                Show Transaction History
            </Button>
        </Box>

        <Divider sx={{ marginY: 2 }} />

        <Box
            sx={{
            padding: 2,
            textAlign: "center",
            backgroundColor: "#f9f9f9",
            borderRadius: 1,
            }}
        >
            {isAddTransaction ? (
                <>
                    <Typography variant="h6" color="textSecondary">
                        Add New Transaction
                    </Typography>
                    <Typography variant="body2" color="textSecondary">
                        Here you can add a new transaction using the form below.
                    </Typography>
                    <FormTransaction/>
                </>
            ) : (
                <>
                    <Typography variant="h6" color="textSecondary">
                        Transaction History
                    </Typography>
                    <Typography variant="body2" color="textSecondary">
                        Here you can view your transaction history.
                    </Typography>
                    <TransactionDataTable/>
                </>
            )}
        </Box>
        </Box>
    );
};

export default TransactionPage;
