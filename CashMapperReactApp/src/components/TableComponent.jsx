import '../styles/Page.css';
import '../styles/Table.css';
import React, { useState, useEffect } from 'react';
import CashMapperDataProvider from '../data/CashMapperDataProvider.js';
import { EditTransaction } from '../components/EditTransaction';
import { ImportTransactions } from '../components/ImportTransactions';
import moment from 'moment';



// A components that displays data in a table.
// data:  the underlying array of data. [required]
// keyField: a unique identifer key for each object in the data array. [required]
// headers: change the name of columns. Optionally ignore columns by setting to null. Must be original column names in data array. [optional]
// columnOrder:  list of columns to be ordered. Must be original column names in data array. [optional]
// transform: modifies the value associated with the field. [optional]

//Ex:
//data = { transactionsFiltered }
//headers = {{ transactionDate: "Date", categoryId: "Category", dateCreated: null, dateModified: null }}
//columnOrder = { ["id", "transactionDate", "description", "note", "source", "categoryId", "flag", "value"]}
//onClick = { myClickHandler }
//onDoubleClick = {myDoubleClickHandler}
//transform = {{
//    transactionDate: (value) => moment(value).format('M/D/YY')

export const TableComponent = ({ data = [], keyField = "id", headers = {}, columnOrder = [], transform = {}, onClick, onDoubleClick}) => {
    const [columnNames, setColumnNames] = useState([])
    const [tableData, setTableData] = useState([])


    // This method sorts a JS object's properties (keys) based on a list of keys provided
    const sortObjectKeysWithArray = (obj, sortKeys) => {
        let sorted = {}
        let sortObject = {}
        sortKeys.forEach(value => {
            sortObject[value] = null;
        })
        sorted = Object.assign(sortObject, obj);
        return sorted;
    }


    const handleClick = (id) => {
        const record = data.find(record => record.id === id);
        console.log(id)
        onClick(record); //call the callback.
    }

    const handleDoubleClick = (id) => {
        const record = data.find(record => record.id === id);
        onDoubleClick(record); //call the callback.
    }

    useEffect(() => {
        // Sets the table data based on props that transform the data
        // Transforms include:  sorting, transforming the value, renaming or hiding columns 
        const transformData = data.map(item => {

            // sort the data based on column order.
            const newItem = sortObjectKeysWithArray(item, columnOrder) 
            const transformedItem = {}
            const properties = Object.keys(newItem); 

            // loop through propnames (keys)
            for (let i = 0; i < properties.length; i++) {
                const propertyName = properties[i];
                // Run value transforms if provided:
                if (propertyName in transform) {
                    newItem[propertyName] = transform[propertyName](newItem[propertyName])
                }
                // Rename or remove columns
                if (headers && propertyName in headers) {
                    const newHeader = headers[propertyName];
                    if (!newHeader) {
                        // if user passes null, it will "hide" the prop.
                        continue
                    } else {
                        // Otherwise, rename
                        transformedItem[newHeader] = newItem[propertyName];
                    }
                } else {
                    transformedItem[propertyName] = newItem[propertyName];
                }
            }
            return transformedItem; // Return the transformed item
        });


        // Perform a check:
        if (keyField in headers) {
            if (!headers[keyField]) {
                console.error("TableComponent: keyField provided is set to null in headers. Please provide a different unique key.")
            }
        }

        if (transformData.length > 0) {
            const filteredColumnNames = Object.keys(transformData[0]);
            setTableData(transformData); 
            setColumnNames(filteredColumnNames); 
        } else {
            setTableData([]); 
            setColumnNames([]); 
        }

    }, [data, headers]);


    return (
        <div className="table-container">
            <table>
                <thead>
                    <tr>
                        {columnNames.map((key) => (
                            <th>{key}</th>))}
                    </tr>
                </thead>

                <tbody className="tbl-content">
                    {tableData.map((dataItem, index) => (
                        <tr
                            className="tbl-row"
                            key={dataItem[keyField]}
                            onClick={() => {
                                const id = dataItem[keyField]; // the unique key id passed by prop
                                handleClick(id);
                            }}
                            onDoubleClick={() => {
                                const id = dataItem[keyField]; // the unique key id passed by prop
                                onDoubleClick(id); // Corrected handler for double-click
                            }}
                        >
                            {Object.keys(dataItem).map((key) => (
                                <td key={key}>{dataItem[key]}</td>
                            ))}
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    )

}
