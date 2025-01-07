import '../styles/Page.css';
import '../styles/Table.css';
import React, { useState, useEffect } from 'react';
import CashMapperDataProvider from '../data/CashMapperDataProvider.js';
import { EditTransaction } from '../components/EditTransaction';
import { ImportTransactions } from '../components/ImportTransactions';
import moment from 'moment';

export const TableComponent = ({ data = [], hideKeys = [], headers, onClick, onDoubleClick, style }) => {
    const [columnNames, setColumnNames] = useState([])
    const [tableData, setTableData] = useState([])

    useEffect(() => {
        const newData = data.map(item => {
            const newItem = { ...item }; 
            const properties = Object.keys(item);
            console.log("PROPERTIES=" + properties);

            for (let i = 0; i < properties.length; i++) {
                const propertyName = properties[i]; 
                console.log("KEY IS====" + propertyName);

   
                if (hideKeys.includes(propertyName)) {
                    console.log("DELETING");
                    delete newItem[propertyName];
                    continue;
                }

                if (headers && headers.length > 0) {
                    if (headers.includes(propertyName)) {
                        const keyIndex = properties.indexOf(propertyName); 
                        const newHeader = headers[keyIndex]; 
                        newItem[newHeader] = newItem[propertyName]; 
                        delete newItem[propertyName]; 
                    }
                }
            }
            return newItem;
        });

        // Assuming filtered is an array of objects, get column names from the first object
        if (newData.length > 0) {
            const filteredColumnNames = Object.keys(newData[0]);
            setColumnNames(filteredColumnNames); // Set column names from the first item
        } else {
            setColumnNames([]); // If no data, reset column names
        }
        setTableData(newData); // Set the updated table data

    }, [data, hideKeys, headers]);




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
