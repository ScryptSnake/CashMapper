import '../styles/Page.css';
import '../styles/Table.css';
import React, { useState, useEffect } from 'react';
import CashMapperDataProvider from '../data/CashMapperDataProvider.js';
import { EditTransaction } from '../components/EditTransaction';
import { ImportTransactions } from '../components/ImportTransactions';
import moment from 'moment';

export const TableComponent = ({ data = [], keyField = "id", headers = {}, sortOrder = [], transform = {}, onClick, onDoubleClick}) => {
    const [columnNames, setColumnNames] = useState([])
    const [tableData, setTableData] = useState([])


    // Sort data provided.
    // obj = object to sort keys
    // sort = array of string values represents keys in each object in data.
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
        //console.log("", record)
        onClick(record); //call the callback.
    }

    const handleDoubleClick = (id) => {
        const record = data.find(record => record.id === id);
        console.log(record) //this works fine
        onDoubleClick(record); //call the callback.
    }


    useEffect(() => {
        const transformData = data.map(item => {
            const newItem = sortObjectKeysWithArray(item, sortOrder) 
            const transformedItem = {}
            const properties = Object.keys(newItem); 

            // loop through propnames.
            for (let i = 0; i < properties.length; i++) {
                const propertyName = properties[i];

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
