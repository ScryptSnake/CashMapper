import '../styles/Page.css';
import CashMapperDataProvider from '../data/CashMapperDataProvider.js';
import React, { useState, useEffect } from 'react';
import EditTransaction from '../components/EditTransaction';
import '../styles/Table.css';
import moment from 'moment';

const TransactionsPage = () => {
    const [transactions, setTransactions] = useState([]); // From database.
    const [transactionsFiltered, setTransactionsFiltered] = useState([]); // Displayed transactions
    const [filter, setFilter] = useState(CashMapperDataProvider.Transactions.createFilter()); // the active filter on the transactions
    const [refresh, setRefresh] = useState(false); // boolean whether the Transactions state should be re-fetched from db.
    const [showEdit, setShowEdit] = useState(false);
    const [closeEdit, setCloseEdit] = useState(true);
    const [selectedTransaction, setSelectedTransaction] = useState(null); // The active transaction in the table
    const [categories, setCategories] = useState([]); // cache categories from db for search filter dropdown. 

    const openEditHandler = () => {
        setCloseEdit(true);
        setShowEdit(true);
    }

    const closeEditHandler = () => {
        setShowEdit(false);
        setCloseEdit(true);
    }

    const handleFilterChange = (e) => {
        const fieldName = e.target.id; // prop name
        const fieldValue = e.target.value; // value of prop

        let updatedFilter;

        // Copy existing filter to variable
         updatedFilter = { ...filter }; 

        // Update specified field with value
        console.log(`"${fieldName} = ${fieldValue}"`)
        updatedFilter[fieldName] = fieldValue;
        setFilter(updatedFilter)

    };



    // This is also a callback from the EditTransaction page.
    useEffect(() => {
        const loadTransactions = async () => {
            try {

                //console.log("Filter state = " + filter.categoryId) //this is late

                let data;

                if (transactions.length === 0 || refresh) {

                    // Cache categories for filter dropdown box
                    let cats = await CashMapperDataProvider.Categories.getAll();
                    
                    setCategories(cats);

                    data = await CashMapperDataProvider.Transactions.getAll();
                    setTransactions(data); // Update state with all transactions
                } else {
                    data = transactions; // Use already loaded transactions
                }

                // Filter the transactions
                const filteredData = await CashMapperDataProvider.Transactions.filterItems(data, filter);
                setRefresh(false); // Turn off refresh
                setTransactionsFiltered(filteredData);
            } catch (error) {
                console.error('Error loading transactions:', error);
            }
        };

        loadTransactions();
    }, [transactions, refresh, filter]); // Dependencies: triggers whenever these change



    // Find category name from categoryId
    const findCategoryName = (categoryId) => {
        const category = categories.find((cat) => cat.id === categoryId);
        return category ? category.name : 'Unknown'; // Return 'Unknown' if no match is found
    };

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
                    <button className="btn-secondary menu">Import</button>
                    <button className="btn-secondary menu">Export</button>
                    <button className="btn-secondary"
                        onClick={() => { setSelectedTransaction(null); openEditHandler() }}>Add
                    </button>
                </div>
            </div>
            <div className="Filter-Menu">
                    <label htmlFor="description">Search:</label>
                <input
                    className="input-group-input"
                        type="text" id="descriptionAndNote"
                        value={filter.descriptionAndNote || ""}
                        onChange={handleFilterChange} />

                <select
                    className="input-group-input small"
                    type="text" id="categoryId"
                    value={filter.categoryId || "[ Category ]"}
                    onChange={handleFilterChange}>
                    <option key="" value="">
                        [  Category ]
                    </option>
                    {categories.map((category) => (
                        <option key={category.id} value={category.id}>
                            {category.name}
                        </option>
                    ))}
                </select>
                <input
                    className="input-group-input small"
                    type="date" id="dateMin"
                    value={filter.dateMin || ""}
                    onChange={handleFilterChange} />
                <label> to </label>
                <input
                    className="input-group-input small"
                    type="date" id="dateMax"
                    value={filter.dateMax || ""}
                    onChange={handleFilterChange} />
                </div>

                  
            <div className="table">
                <table>
                    <thead>
                        <tr>
                            <th>Id</th>
                            <th>Date</th>
                            <th>Description</th>
                            <th>Source</th>
                            <th>Note</th>
                            <th>Category Id</th>
                            <th>Value</th>
                        </tr>
                    </thead>
                    <tbody className="tbl-content">
                        {transactionsFiltered.map((transaction) => (
                            <tr className="tbl-row" key={transaction.id}
                                onClick={() => { setSelectedTransaction(transaction) }}
                                onDoubleClick={openEditHandler}>
                                <td>{transaction.id}</td>
                                <td>{moment(transaction.transactionDate).format('M/D/YY')}</td>
                                <td>{transaction.description}</td>
                                <td>{transaction.source}</td>
                                <td>{transaction.note}</td>
                                <td>{findCategoryName(transaction.categoryId)}</td>
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
                callback={() => {setRefresh(true) }} //The callback is set to SetRefresh. When triggered, reload transactions from DB. See submitForm method in EditTransaction. 
            />

        </div>
    );
};

export default TransactionsPage;
