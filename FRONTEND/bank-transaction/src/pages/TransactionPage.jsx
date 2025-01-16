import { useState } from "react";
import { Box, Button, Typography, Divider, CircularProgress } from "@mui/material";
import FormTransaction from "../components/FormTransaction";
import TransactionDataTable from "../components/TransactionDataTable";
import AlertInfoWarningError from "../components/AlertInfoWarningError";

const TransactionPage = () => {
    const [isAddTransaction, setIsAddTransaction] = useState(true);
    const [isLoading, setIsLoading] = useState(false)
    const [alert, setAlert] =useState({severity:null, message:null})
    
    const LoadingOverlay = () => {
        return (
            <>
                <Box
                    sx={{
                        opacity: 0.7,
                        position: "absolute",
                        top: 0,
                        left: 0,
                        right: 0,
                        bottom: 0,
                        zIndex: 3,
                        backgroundColor: "rgba(255, 255, 255, 0.7)",
                    }}
                    ></Box>
                <Box
                    display="flex"
                    flexDirection="column"
                    alignItems="center"
                    justifyContent="center"
                    minHeight="100vh"
                    sx={{
                        position: "absolute",
                        top: 0,
                        left: 0,
                        right: 0,
                        bottom: 0,
                        zIndex: 4,
                    }}
                >
                    <CircularProgress size={100} sx={{ animation: "spin 1.5s linear infinite" }} />
                
                    <Typography 
                    variant="h6" 
                    mt={2} 
                    sx={{
                        animation: "fadeIn 2s ease-in-out infinite"
                    }}
                    >
                    Loading Transactions...
                    </Typography>
            
                    <style>{`
                    @keyframes fadeIn {
                        0% {
                        opacity: 0;
                        }
                        50% {
                        opacity: 1;
                        }
                        100% {
                        opacity: 0;
                        }
                    }
                    @keyframes spin {
                        0% {
                        transform: rotate(0deg);
                        }
                        100% {
                        transform: rotate(360deg);
                        }
                    }
                    `}</style>
                </Box>
            </>
        );
    };

    return (
        <>
            {isLoading && <LoadingOverlay />}
            {alert?.severity !== null && (<AlertInfoWarningError severity={alert?.severity} message={alert?.message}/>)}
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
                            <FormTransaction setIsLoading={setIsLoading} setAlert={setAlert} />
                        </>
                    ) : (
                        <>
                            <Typography variant="h6" color="textSecondary">
                                Transaction History
                            </Typography>
                            <TransactionDataTable setIsLoading={setIsLoading} setAlert={setAlert} />
                        </>
                    )}
                </Box>
            </Box>
        </>
    );
};

export default TransactionPage;
