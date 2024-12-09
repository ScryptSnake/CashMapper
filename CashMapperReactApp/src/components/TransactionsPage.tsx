
import { Transaction } from '../types/types.tsx';
import '../styles/Page.css';
import React, { useState, useEffect } from 'react';


const TransactionsPage = () => {

    const [transactions, getTransactions] = useState<Transaction[]>([]);

    // Fetch data when the component mounts
    useEffect(() => {
        fetch('http://localhost:5009/api/Transactions') // Replace with your actual API URL
            .then((response) => response.json()) // Convert response to JSON
            .then((data) => getTransactions(data)) // Set the fetched data into state
            .catch((error) => console.error('Error fetching data:', error)); // Handle errors
    }, []);


    return (
        <div className="Page">
        <h1>hello world</h1>
            <ul>
                {transactions.map((transaction,index) => (
                    <li key={index}>{transaction.id}</li> // Replace 'name' with the actual property of the transaction object
                ))}
            </ul>


        </div>
    );
};

export default TransactionsPage;
