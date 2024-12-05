
import '../styles/Page.css';
import React, { useState, useEffect } from 'react';

const TransactionsPage = () => {

    const [transactions, getTransactions] = useState([]);

    // Fetch data when the component mounts
    useEffect(() => {
        fetch('api url') // Replace with your actual API URL
            .then((response) => response.json()) // Convert response to JSON
            .then((data) => getTransactions(data)) // Set the fetched data into state
            .catch((error) => console.error('Error fetching data:', error)); // Handle errors
    }, []);



    return (
        <div className="Page">
            <ul>
                {/* Render the transactions */}
                {transactions.map((transaction, index) => (
                    <li key={index}>{transaction.name}</li> // Replace 'name' with the actual property of the transaction object
                ))}
            </ul>


        </div>
    );
};

export default HomePage;
