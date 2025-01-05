import React, { useState, useEffect } from 'react';
import '../styles/Modal.css';
import CashMapperDataProvider from '../data/CashMapperDataProvider.js';
import moment from 'moment';
import CurrencyInput from 'react-currency-input-field'

const ImportTransactions = ({ showModal, closeModal, callback}) => {

    if (!showModal) return null; // Don't show if showModal is false

    const [formData, setFormData] = useState({ source: null, fileName: null});
    const [results, setResults] = useState([]);
    const [sources, setSources] = useState([]);


    // set the formDara state. Called from Input changed event. 
    const handleFormChange = (e) => {
        const fieldName = e.target.id; // prop name
        let fieldValue = e.target.value; 
        if (e.target.type === "file") { fieldValue = e.target.files[0].name }

        const newFormData = { ...formData } // copy original
        newFormData[fieldName] = fieldValue; 
        setFormData(newFormData);
    };

    useEffect(() => {
        const parse = async () => {
            const data = await CashMapperDataProvider.Imports.parseAsync(formData.source, formData.fileName);
            setResults(data);
        }
        if (!formData.fileName && !formData.source) {
            parse()
        }

    },[formData])


    const handleImportClick = () => {
        const execute = async () => {
            return await CashMapperDataProvider.Transactions.bulkInsertAsync(results);
        }
        execute();
        handleFormClose();
    }


    // Submit data from form
    const handleFormClose = () => {

        e.preventDefault(); // prevent app from re-render.

        setFormData([]);
        setResults([]);
        closeModal();  // HIDE form.
        callback(); // Update list on TransactionsPage
    };

    


    // Grab list of sources (determines the parser)
    useEffect(() => {
        // Note: wrap the DataFactory call inside async function (useEffect doesnt support async)
        const fetch = async () => {
            try {
                const data = CashMapperDataProvider.Imports.getSources();
                setSources(data); //update state
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
                            <form onSubmit={handleImportClick}>
                                <div className="input-group">
                                    <label className="input-group-label" htmlFor="source">Source Type</label>
                                    <select className="input-group-input small"
                                        id="source" value="" onChange={handleFormChange}>
                                        {sources.map((source) => (
                                            <option key={source} value={source}>
                                                {source}
                                            </option>
                                        ))}
                                    </select>
                                </div>
                                <div className="input-group">
                                    <label className="input-group-label">File</label>
                                    <input type="file" id="fileName" className="input-group-input medium" name="file" id="file" onChange={handleFormChange}></input>
                                </div>
     

                                <hr className="modal-divider" />
                                <div className="input-group">
                                    <label className="input-group-label" htmlFor="results">Results</label>
                                    <textarea className="input-group-input"
                                        id="results" onChange={handleFormChange} disabled={true} value={JSON.stringify(results)} />
                                </div>

                                <div className="modal-footer">
                                    <button type="button" className="btn-secondary" onClick={closeModal}>Cancel</button>
                                    <button type="submit" className="btn-primary">Import</button>
                                </div>
                            </form>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default ImportTransactions;
