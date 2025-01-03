
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
        // SEE filter for local filtering.
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


    filterItems: async (data, filter) => {
        // Filters a local array of transactions
        // data: the array
        // filter: the filter object

        // Make a copy
        let results = data.slice();

        if (filter) {
            if (filter.descriptionAndNote) {
                results = results.filter(transaction => {
                    return new String(transaction.description).match(new RegExp(`${filter.descriptionAndNote}.*`, 'i')) ||
                        new String(transaction.note).match(new RegExp(`${filter.descriptionAndNote}.*`, 'i')); 

                });
            }

            if (filter.categoryId) {
                console.log("filter API found filter.categoryId = " + filter.categoryId)
                results = results.filter(transaction => {
                    return Number(transaction.categoryId) === Number(filter.categoryId)
                });
            }

            //if (filter.dateMin) {
            //    results = results.filter(transaction => {
            //        return new Date(transaction.transactionDate) >= new Date(filter.dateMin)
            //    });
            //}

            //if (filter.dateMax) {
            //    results = results.filter(transaction => {
            //        return new Date(transaction.transactionDate) <= new Date(filter.dateMax)
            //    });
            //}

            //if (filter.valueMin) {
            //    results = results.filter(transaction => {
            //        return transaction.value >= filter.valueMin
            //    });
            //}

            //if (filter.valueMax) {
            //    results = results.filter(transaction => {
            //        return transaction.value <= filter.valueMax
            //    });
            //}

        }
        //sort by date ascending
        results.sort((a, b) => new Date(a.transactionDate) - new Date(b.transactionDate)) ||
        results.sort((a, b) => new Date(a.id) - new Date(b.id));
        return results;


    },

    // A template function for a filter object to be used with the filterItems function.
    createFilter: (descriptionAndNote, categoryId, dateMin, dateMax, valueMin, valueMax) => {
        return {
            descriptionAndNote: descriptionAndNote,
            categoryId: categoryId,
            dateMin: dateMin,
            dateMax: dateMax,
            valueMin: valueMin,
            valueMax: valueMax
        }
    }




};





export default TransactionsApi;
