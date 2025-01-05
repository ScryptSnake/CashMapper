import '../styles/Page.css';
import React, { useState, useEffect } from 'react';
import CashMapperDataProvider from '../data/CashMapperDataProvider.js';
import EditTransaction from '../components/EditTransaction';
import ImportTransactions from '../components/ImportTransactions';
import '../styles/Table.css';
import moment from 'moment';

const TransactionsPage = () => {
    const [transactions, setTransactions] = useState([]); // From database.
    const [transactionsFiltered, setTransactionsFiltered] = useState([]); // Displayed transactions
    const [filter, setFilter] = useState(CashMapperDataProvider.Transactions.createFilter()); // the active filter on the transactions
    const [refresh, setRefresh] = useState(false); // boolean whether the Transactions state should be re-fetched from db.
    const [showEdit, setShowEdit] = useState(false);
    const [showImport, setShowImport] = useState(false);



    const [selectedTransaction, setSelectedTransaction] = useState(null); // The active transaction in the table
    const [categories, setCategories] = useState([]); // cache categories from db for search filter dropdown.


    const handleImportClick = (visible) => {
        setShowImport(visible);
    }

    const handleEditClick = (visible) => {
        setShowEdit(visible);
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

    const handleDownload = async () => {
        const file = await CashMapperDataProvider.Transactions.convertCsvAsync(transactionsFiltered);
        const url = URL.createObjectURL(file);
        const fileName = `Transactions_Export_${moment().format('YYYY-MM-DD').toString()}.csv`;

        // temporary anchor element. 
        const a = document.createElement('a')

        a.href = url;
        a.download = fileName;
        a.click(); // Programatically click.

        URL.revokeObjectURL(url);
    }

    // This is also a callback from the EditTransaction page.
    useEffect(() => {
        const loadTransactions = async () => {
            try {

                //console.log("Filter state = " + filter.categoryId) //this is late

                let data;

                if (transactions.length === 0 || refresh) {

                    // Cache categories for filter dropdown box
                    let cats = await CashMapperDataProvider.Categories.getAllAsync();
                    
                    setCategories(cats);

                    data = await CashMapperDataProvider.Transactions.getAllAsync();
                    setTransactions(data); // Update state with all transactions
                } else {
                    data = transactions; // Use already loaded transactions
                }

                // Filter the transactions
                const filteredData = await CashMapperDataProvider.Transactions.filterItemsAsync(data, filter);
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
                    <button className="btn secondary with-icon" onClick={handleDownload}>
                        <img src="./icons/download-16.png"></img>
                        Download
                    </button>
                    <button className="btn secondary with-icon" onClick={() => { handleImportClick(true) }}>
                        <img src="./icons/upload-3-16.png"></img>
                        Import
                    </button>
                    <button className="btn secondary with-icon"
                        onClick={() => { setSelectedTransaction(null); handleEditClick(true) }}>
                        <img src="./icons/plus-6-16.png"></img>
                        
                    </button>
                </div>
            </div>
            <div className="Filter-Menu">
                    <label htmlFor="description">Filter:</label>
                <input
                    className="input-group-input"
                    placeholder="Search transactions..."
                    type="text"
                    id="descriptionAndNote"
                    value={filter.descriptionAndNote || ""}
                    onChange={handleFilterChange} />
                <select
                    className="input-group-input small"
                    type="text"
                    id="categoryId"
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
                <input
                    className="input-group-input small"
                    placeholder="Min Value"
                    type="number" id="valueMin"
                    value={filter.valueMin || ""}
                    onChange={handleFilterChange} />
                <label> to </label>
                <input
                    className="input-group-input small"
                    placeholder="Max Value"
                    type="number" id="valueMax"
                    value={filter.valueMax || ""}
                    onChange={handleFilterChange} />
            </div>


                  
            <div className="table-container">
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
                                onDoubleClick={() => { setShowEdit(true) }}>
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
                closeModal={() => { handleEditClick(false) }}
                transaction={selectedTransaction}
                callback={() => {setRefresh(true) }} //The callback is set to SetRefresh. When triggered, reload transactions from DB. See submitForm method in EditTransaction. 
            />

            <ImportTransactions
                showModal={showImport}
                closeModal={() => { handleImportClick(false) }}
                callback={() => {null } }
            />
                
         
        </div>
    );
};

export default TransactionsPage;
