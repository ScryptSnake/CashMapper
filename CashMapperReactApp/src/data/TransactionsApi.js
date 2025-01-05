import { json2csv} from 'json-2-csv';


const TransactionsApi = {
    getAllAsync: async () => {
        try {
            const response = await fetch('http://localhost:5009/api/Transactions');
            if (!response.ok) {
                throw new Error(`Failed to fetch transactions. Status: ${response.status}`);
            }
            const data = await response.json();
            console.log('getAll transactions', data);
            return data;


        } catch (error) {
            console.error('Error fetching all transactions:', error);
            throw error;
        }
    },

    getSingleAsync: async (id) => {
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

    getMultipleAsync: async (filterParams) => {
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
            console.error(`Failed to get transactions with filter: ${filterParams.ToString()}`, error);
        }
    },


    addItemAsync: async (data) => {
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

    // Insert multiple transactions, like from an import operation. 
    bulkInsertAsync: async (data) => {

        return
    },



    updateItemAsync: async (data) => {
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

    // Filters a local array of transactions
    // data: the array
    // filter: the filter object
    filterItemsAsync: async (data, filter) => {
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
            if (filter.dateMin) {
                results = results.filter(transaction => {
                    return new Date(transaction.transactionDate) >= new Date(filter.dateMin)
                });
            }
            if (filter.dateMax) {
                results = results.filter(transaction => {
                    const maxDate = new Date(filter.dateMax);
                    // Add 1 day to Date. No idea why it won't find values matching. Its NOT an hours issue.
                    maxDate.setDate(maxDate.getDate() + 1);
                    return new Date(transaction.transactionDate) <= maxDate
                });
            }
            if (filter.valueMin) {
                results = results.filter(transaction => {
                    return Number(transaction.value) >= Number(filter.valueMin)
                });
            }
            if (filter.valueMax) {
                results = results.filter(transaction => {
                    return Number(transaction.value) <= Number(filter.valueMax)
                });
            }
        }
        //sort by date descending
        results.sort((a, b) => new Date(b.transactionDate) - new Date(a.transactionDate))
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
    },

    // Returns a blob of data in csv format
    convertCsvAsync: async (data) => {
        // get field names:
        if (data) {
            const csv = await json2csv(data);

            const file = new Blob([csv], { type: 'text/csv' });

            return file;
        }





    }


};





export default TransactionsApi;
