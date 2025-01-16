import { useEffect, useState } from 'react';
import { Snackbar, Alert } from '@mui/material';

const AlertInfoWarningError = ({ severity, message }) => {
  const [alerts, setAlerts] = useState([]);

  const handleClose = (event, reason, index) => {
    if (reason === 'clickaway') return;
    setAlerts((prevAlerts) => prevAlerts.filter((_, i) => i !== index));
  };

  useEffect(() => {
    if (severity && message) {
      setAlerts((prevAlerts) => [
        ...prevAlerts,
        { severity, message },
      ]);
    }
  }, [severity, message]);

  return (
    <>
      {alerts.map((alert, index) => (
        <Snackbar
          key={index}
          open={true}
          autoHideDuration={3000}
          anchorOrigin={{
            vertical: 'bottom',
            horizontal: 'right',
          }}
          onClose={(event, reason) => handleClose(event, reason, index)}
          sx={{
            marginBottom: index * 7,
            zIndex: 300 + index,
          }}
        >
          <Alert onClose={(event, reason) => handleClose(event, reason, index)} severity={alert.severity} sx={{ width: '100%' }}>
            {alert.message}
          </Alert>
        </Snackbar>
      ))}
    </>
  );
};

export default AlertInfoWarningError;
