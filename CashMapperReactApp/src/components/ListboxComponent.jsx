import '../styles/Page.css';
import '../styles/Table.css';
import React, { useState, useEffect } from 'react';
import CashMapperDataProvider from '../data/CashMapperDataProvider.js';
import { EditTransaction } from '../components/EditTransaction';
import { ImportTransactions } from '../components/ImportTransactions';
import { TableComponent } from '../components/TableComponent';
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

export const ListboxComponent = ({ data = [], keyField = "id", headers = {}, columnOrder = [], transform = {}, onClick, onDoubleClick}) => {


    const [activeRow, setActiveRow] = useState(null);



    const onClickHandler = (id) => {
        // store key field (id) in active row variable for highlighting
        setActiveRow(id)
        // call the parent's callback
        //onClick(id);

    }

    const onDoubleClickHandler = (id) => {
        setActiveRow(id)
       // onDoubleClick(id);
    }

    // a style to highlight the row
    const style = `
        tr.highlighted {
            background-color: #f1f1f1;
        }

        tr {
            cursor: pointer;
        }
        `;



    useEffect(() => {
        console.log("effect")
        if (activeRow) {
            console.log("active")
            const row = document.querySelector(`tr[data-key]='${activeRow}']`)
            if (row) {
                row.classList.add('highlighted'); // add the css style.
            }
        }
    },[activeRow])




    return (
        <div>
            <TableComponent 
                data={data}
                keyField={keyField}
                headers={headers}
                columnOrder={columnOrder}
                transform={transform}
                onClick={onClickHandler}
                onDoubleClick={onDoubleClickHandler }
            />





        </div>
    )

}
