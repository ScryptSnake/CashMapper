
import { Transaction } from '../types/types.tsx';
import '../styles/Page.css';
import React, { useState, useEffect } from 'react';


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


 
    return (
        <div className="Page">
            <h1>Transactions List</h1>
            <table className="table">
                <thead>
                    <tr>
                        <th>Id</th>
                        <th>Value</th>
                        <th>Description</th>
                        <th>Source</th>
                        <th>Category</th>
                        <th>Note</th>
                    </tr>
                </thead>
                <tbody>
                {transactions.map((transaction) => (
                    <tr key={transaction.id}>
                        <td>{transaction.id}</td>
                        <td>{transaction.value}</td>
                        <td>{transaction.description}</td>
                        <td>{transaction.source ?? 'Empty'}</td>
                        <td>{transaction.categoryId}</td>
                    </tr>
                ))}
                </tbody>
            </table>
        </div>
    );

};

export default TransactionsPage;
