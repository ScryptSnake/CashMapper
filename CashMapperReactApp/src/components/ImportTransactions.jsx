import React, { useState, useEffect } from 'react';
import '../styles/Modal.css';
import CashMapperDataProvider from '../data/CashMapperDataProvider.js';
import moment from 'moment';
import CurrencyInput from 'react-currency-input-field'

const ImportTransactions = ({ showModal, closeModal, callback}) => {

    if (!showModal) return null; // Don't show if showModal is false

    const [formData, setFormData] = useState(defaultTransaction);
    const [sources, setSources] = useState([]);


    // set the formDara state. Called from Input changed event. 
    const handleFormChange = (e) => {
        const fieldName = e.target.id; // prop name
        const fieldValue = e.target.value; // value of prop
        const updatedTransaction = { ...formData }; // copy all properties in current state

        // Update specified field with value
        updatedTransaction[fieldName] = fieldValue;

        // Commit to state.
        setFormData(updatedTransaction);
    };

    // Check if user is editing or adding, set form data accordingly.
    useEffect(() => {
        if (transaction) {
            setEditMode(true);
            setFormData(transaction);
        } else {
            setEditMode(false);
            setFormData(defaultTransaction);
        }
    }, [transaction]);


    // Submit data from form
    const submitFormData = async (e) => {

        e.preventDefault(); // prevent app from re-render.

        closeModal();  // HIDE form.
        callback(); // Update list on TransactionsPage
    };


    // Grab list of sources (determines the parser)
    useEffect(() => {
        // Note: wrap the DataFactory call inside async function (useEffect doesnt support async)
        const fetch = async () => {
            try {
                const data = CashMapperDataProvider.Imports.getSources();
                setCategoriesHandler(data); //update state
                console.log(data); 
            } catch (error) {
                console.error('Error fetching categories:', error);
            }
        }
        // Call async function
        fetch();
    }, []);


    return (
        <div>
            {/* Backdrop overlay */}
            <div className="modal-backdrop" onClick={closeModal}></div>

            <div className="modal">
                <div className="modal-dialog">
                    <div>
                        <div className="modal-header">
                            <h2>Import Transactions</h2>
                            <button type="button" onClick={closeModal}>X</button>
                        </div>

                        <div>
                            <form onSubmit={submitFormData}>
                                <div className="input-group">
                                    <label className="input-group-label" htmlFor="source">Source Type</label>
                                    <select className="input-group-input medium"
                                        id="source" value="" onChange={handleFormChange}>
                                        {sources.map((source) => (
                                            <option key={source} value={source}>
                                                {source}
                                            </option>
                                        ))}
                                    </select>
                                </div>
                                <div className="input-group">
                                    <label className="input-group-label">File: </label>
                                    <input className="input-group-input medium"></input>
                                    <button className="btn-secondary">...</button>
                                </div>

                                <hr className="modal-divider" />
                                <div className="input-group">
                                    <label className="input-group-label" htmlFor="results">Results</label>
                                    <textarea className="input-group-input"
                                        id="results" value="" onChange={handleFormChange} disabled={true} />
                                </div>

                                <div className="modal-footer">
                                    <button type="button" className="btn-secondary" onClick={closeModal}>Cancel</button>
                                    <button type="submit" className="btn-primary">Import}</button>
                                </div>
                            </form>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default EditTransaction;
