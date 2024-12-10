
import { Transaction } from '../types/types.tsx';
import '../styles/Page.css';
import React, { useState, useEffect } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';


const TransactionsPage = () => {
    const [transactions, getTransactions] = useState<Transaction[]>([]);

    // Fetch data when the component mounts
    useEffect(() => {
        fetch('http://localhost:5009/api/Transactions') // Replace with your actual API URL
            .then((response) => response.json()) // Convert response to JSON
            .then((data) => {
                console.log(data);
                getTransactions(data);     // Set the fetched data into state
            })
            .catch((error) => console.error('Error fetching data:', error)); // Handle errors
    }, []);

    /* Create a formatter for currency. */
    const formatter = new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD'
    });


    return (
        <div className="Page">
            <h1>Transactions</h1>
            <table className="table table-hover table-custom">
                <thead>
                    <tr className="fs-5 table-bordered">
                        <th>Id</th>
                        <th>Description</th>
                        <th>Source</th>
                        <th>Note</th>
                        <th>Category</th>
                        <th>Value</th>
                    </tr>
                </thead>
                <tbody>
                {transactions.map((transaction) => (
                    <tr key={transaction.id}>
                        <td>{transaction.id}</td>
                        <td>{transaction.description}</td>
                        <td>{transaction.source ?? 'Empty'}</td>
                        <td>{transaction.note}</td>
                        <td>{transaction.categoryId}</td>
                        <td>{formatter.format(transaction.value)}</td>
                    </tr>
                ))}
                </tbody>
            </table>
        </div>
    );

};

export default TransactionsPage;
