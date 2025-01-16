const BASE_URL = "http://localhost:5001/api/v1"; // Ganti dengan URL API Anda

const TransactionServices = {
  createTransaction: async (transactionData) => {
    try {
      const response = await fetch(`${BASE_URL}/transaction`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(transactionData),
      });
      if (!response.ok) {
        throw new Error(`Error creating transaction: ${response.statusText}`);
      }
      return await response.json();
    } catch (error) {
      console.error(error);
      throw error;
    }
  },

  getTransactions: async (transactionId) => {
    try {
      const response = await fetch(`${BASE_URL}/transaction/${transactionId}`, {
        method: "GET",
      });
      if (!response.ok) {
        throw new Error(`Error Getting transaction: ${response.statusText}`);
      }
      return await response.json();
    } catch (error) {
      console.error(error);
      throw error;
    }
  },
};

export default TransactionServices;
