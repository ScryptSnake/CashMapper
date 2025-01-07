import '../styles/Page.css';
import '../styles/Table.css';
import React, { useState, useEffect } from 'react';
import CashMapperDataProvider from '../data/CashMapperDataProvider.js';
import { EditTransaction } from '../components/EditTransaction';
import { ImportTransactions } from '../components/ImportTransactions';
import moment from 'moment';

export const TableComponent = ({ data = [], headers = {}, sortOrder=[], onClick, onDoubleClick, style }) => {
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
                    {tableData.map((dataItem) => (
                        <tr className="tbl-row" key={dataItem.id}
                            onClick={() => { onClick(dataItem) }}
                            onDoubleClick={() => { onDoubleClick(dataItem) }}>
                            {Object.keys(dataItem).map((key, index) => (
                                <td key={key}>{dataItem[key]}</td>

                            ))}
                        </tr>
                    ))};
                </tbody>
            </table>
        </div>
    )

}
