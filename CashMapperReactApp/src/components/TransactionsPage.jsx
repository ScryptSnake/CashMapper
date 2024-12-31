import '../styles/Page.css';
import CashMapperDataProvider from '../data/CashMapperDataProvider.js';
import React, { useState, useEffect } from 'react';
import EditTransaction from '../components/EditTransaction';
import '../styles/Table.css';


const TransactionsPage = () => {
    const [transactions, setTransactions] = useState([]);
    const [showEdit, setShowEdit] = useState(false);
    const [closeEdit, setCloseEdit] = useState(true);
    const [selectedTransaction, setSelectedTransaction] = useState(null);

    // State setter functions.
    // Note: if these contain arguments, they no longer are referenced as a pointer, but update state and cause overflow.
    const setTransactionsHandler = (data) => {
        setTransactions(data)
    }

    const openEditHandler = () => {
        setCloseEdit(true);
        setShowEdit(true);
    }

    const closeEditHandler = () => {
        setShowEdit(false);
        setCloseEdit(true);
    }

    const setSelectedTransactionHandler = (transaction) => {
        setSelectedTransaction(transaction);
    };


    // This is also a callback from the EditTransaction page.
    // filter params - optional kvp to filter
    const loadTransactions = async (filterParams) => {
        try {
            let data;
            if (filterParams) {
                data = await CashMapperDataProvider.Transactions.getMultiple(filterParams);
            } else {
                data = await CashMapperDataProvider.Transactions.getAll();
            }
            setTransactionsHandler(data);
        }
        catch (error) {
            console.log('fetchTransactions failed. ', error);
        }
    };


    // Fetch data when the component mounts.
    useEffect(() => {
        const load = async () => {
            loadTransactions();
        }
        load();
    }, []);


    // Create a formatter for currency.
    const formatter = new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD',
    });


    return (
        <div className="Page">
            <div className="Menu-Bar">
                <h1>Transactions</h1>
                <div className="input-group">
                    <label className="input-group-label" htmlFor="description">Search:</label>
                    <input className="input-group-input large" type="text" id="description" />
                </div>
                <div className="Menu-Bar-Items">
                    <button className="btn-secondary menu">Import</button>
                    <hr />
                    <button className="btn-secondary"
                        onClick={() => { setSelectedTransactionHandler(null); openEditHandler(); }}>Add
                    </button>
                </div>

            </div>
            <div className="tbl-header">
                <table>
                    <thead>
                        <tr>
                            <th>Id</th>
                            <th>Description</th>
                            <th>Source</th>
                            <th>Note</th>
                            <th>Category Id</th>
                            <th>Value</th>
                        </tr>
                    </thead>
                </table>
            </div>

            <div className="tbl-content">
                <table className="table-custom">
                <tbody>
                    {transactions.map((transaction) => (
                        <tr className="tbl-row" key={transaction.id}
                            onClick={() => {setSelectedTransactionHandler(transaction)}}
                            onDoubleClick={openEditHandler}>
                            <td>{transaction.id}</td>
                            <td>{transaction.description}</td>
                            <td>{transaction.source || 'Empty'}</td>
                            <td>{transaction.note}</td>
                            <td>{transaction.categoryId}</td>
                            <td>{formatter.format(transaction.value)}</td>
                        </tr>
                    ))}
                    </tbody>
                </table>
            </div>


            {/* Render EditTransaction if showEdit is true. */}
            <EditTransaction
                showModal={showEdit}
                closeModal={closeEditHandler}
                transaction={selectedTransaction}
                updateTransactions={loadTransactions}
            />

        </div>
    );
};

export default TransactionsPage;
