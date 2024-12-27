import '../styles/Page.css';
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
    const fetchTransactions = () => {
        fetch('http://localhost:5009/api/Transactions') // Replace with your actual API URL
            .then((response) => response.json())
            .then((data) => {
                console.log(data);
                setTransactionsHandler(data); // Set the fetched data into state
            })
            .catch((error) => console.error('Error fetching data:', error));
    };


    // Fetch data when the component mounts.
    useEffect(() => {
        fetchTransactions();
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
                <div className="Menu-Bar-Items">
                    <button className="btn-primary menu">Import</button>
                    <hr></hr>
                    <button className="btn-primary">Add</button>
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
                updateTransactions={fetchTransactions}

            />


        </div>
    );
};

export default TransactionsPage;
