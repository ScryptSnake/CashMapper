
const TransactionsApi = {
    getAll: async () => {
        try {
            const response = await fetch('http://localhost:5009/api/Transactions');
            if (!response.ok) {
                throw new Error(`Failed to fetch transactions. Status: ${response.status}`);
            }
            const data = await response.json();
            console.log('getAll transactions',data);
            return data;


        } catch (error) {
            console.error('Error fetching all transactions:', error);
            throw error;
        }
    },

    getSingle: async (id) => {
        // id is int.
        try {
            const response = await fetch(`http://localhost:5009/api/transactions/${id}`);
            if (!response.ok) {
                throw new Error(`Failed to fetch transaction: ${idOrName}. Status: ${response.status}`);
            }
            const transaction = await response.json();
            return transaction;
        } catch (error) {
            console.error(`Error fetching transaction: ${idOrName}`, error);
            throw error;
        }
    },

    getMultiple: async (filterParams) => {
        // filterParams is a kvp.
        try {
            const baseUrl = 'http://localhost:5009/api/transactions/filter';
            const url = new URL(baseUrl);
            const params = new URLSearchParams();

            // Add each kvp to params object.
            for (const [key, value] of Object.entries(params)) {
                if (value !== undefined && value !== null) {
                    params.append(key, value);
                }
            }
            url.search = params.ToString();

            const response = await fetch(url.ToString());
            if (!response.ok) {
                throw new Error(`Server response was invalid: ${idOrName}. Status: ${response.status}`);
            }
            const transactions = await response.json();
            return transactions;
        } catch (error) {
            console.error(`Failed to get transactions with filter: ${filterParams.ToString()}`,error);
        }
    },



    addItem: async (data) => {
        try {
            const response = await fetch('http://localhost:5009/api/transactions/', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'accept': 'text/plain',
                },
                body: JSON.stringify(data),
            });

            if (!response.ok) {
                throw new Error('Failed to add transaction. Status: ' + response.status);
            }

            const result = await response.json();
            return result;
        } catch (error) {
            console.error('Error adding transaction:', error);
            throw error;
        }
    },

    updateItem: async (data) => {
        try {
            const response = await fetch('http://localhost:5009/api/transactions/', {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    'accept': 'text/plain',
                },
                body: JSON.stringify(data),
            });

            if (!response.ok) {
                throw new Error('Failed to update transaction. Status: ' + response.status);
            }

            const result = await response.json();
            return result;
        } catch (error) {
            console.error('Error updating transaction:', error);
            throw error;
        }
    },
};

export default TransactionsApi;
