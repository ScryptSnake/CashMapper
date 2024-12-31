import React, { useState, useEffect } from 'react';
import '../styles/Modal.css';
import CashMapperDataProvider from '../data/CashMapperDataProvider.js';

const EditTransaction = ({ showModal, closeModal, transaction, updateTransactions}) => {

    if (!showModal) return null; // Don't render if showModal is false

    // default transaction - Some fields are ignore by the endpoint (auto generated in backend).
    // However, the API requires them. 
    const defaultTransaction = {
        id: 0,
        dateCreated: "2024-12-30T05:04:30.421Z",
        dateModified: "2024-12-30T05:04:30.421Z",
        flag: null,
        description: "[No description]",
        source: "",
        categoryId: 1,
        note: "",
        value: 0.00,
        transactionDate: "2024-12-30T05:04:30.421Z"
    };

    // States
    const [editMode, setEditMode] = useState(false); // True = edit, false = new record.
    const [formData, setFormData] = useState(defaultTransaction);
    const [categories, setCategories] = useState([]);


    // Handlers
    const setCategoriesHandler = (data) => {
        setCategories(data);
    }
    
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

        // Validate the form data
        const formErrors = validateInputs();
        if (formErrors.length > 0) {
            alert(formErrors.join("\n"));
            return;
        }

        var method = CashMapperDataProvider.Transactions.addItem;
        if (editMode) {
            method = CashMapperDataProvider.Transactions.updateItem;
        }
        // Execute delegate.
        var data = await method(formData);

        closeModal();  // HIDE form.
        updateTransactions(); // Update list on TransactionsPage
    };

    // Validate form data
    const validateInputs = () => {

        var errors = [];

        //if (formData.note == undefined || formData.note == "" || formData.note == null ) {
        //    errors.push("note required.");
        //    console.log('note is empty');
        //}

        return errors;
    }

    // Grab categories from data factory.
    useEffect(() => {
        // Note: wrap the DataFactory call inside async function (useEffect doesnt support async)
        const fetch = async () => {
            try {
                const data = await CashMapperDataProvider.Categories.getAll();
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
                            <h2>{editMode ? `Edit Transaction [${formData.id}]` : 'New Transaction'}</h2>
                            <button type="button" onClick={closeModal}>X</button>
                        </div>

                        <div>
                            <form onSubmit={submitFormData}>
                                <div className="input-group">
                                    <label className="input-group-label" htmlFor="description">Description</label>
                                    <input className="input-group-input" type="text"
                                        id="description" value={formData.description} onChange={handleFormChange} />
                                </div>

                                <div className="input-group">
                                    <label className="input-group-label" htmlFor="transactionDate">Date</label>
                                    <input className="input-group-input small" type="text"
                                        id="transactionDate" value={formData.transactionDate} onChange={handleFormChange} />
                                </div>

                                <div className="input-group">
                                    <label className="input-group-label" htmlFor="categoryId">Category</label>
                                    <select className="input-group-input medium"
                                        id="categoryId" value={formData.categoryId} onChange={handleFormChange}>
                                        {categories.map((category) => (
                                            <option key={category.id} value={category.id}>
                                                {category.name}
                                            </option>
                                        ))}
                                    </select>
                                </div>

                                <div className="input-group">
                                    <label className="input-group-label" htmlFor="value">Value</label>
                                    <input className="input-group-input small" type="number"
                                        id="value" value={formData.value} onChange={handleFormChange} />
                                </div>

                                <div className="input-group">
                                    <label className="input-group-label" htmlFor="note">Notes</label>
                                    <textarea className="input-group-input"
                                        id="note" value={formData.note} onChange={handleFormChange} />
                                </div>

                                <hr className="modal-divider" />

                                <div className="input-group">
                                    <label className="input-group-label" htmlFor="source">Source</label>
                                    <input className="input-group-input medium" type="text"
                                        id="source" value={formData.source} onChange={handleFormChange} />
                                </div>

                                <div className="input-group">
                                    <label className="input-group-label" htmlFor="source">Flag</label>
                                    <input className="input-group-input small" type="text"
                                        id="flag" value={formData.flag} onChange={handleFormChange} />
                                </div>

                                <div className="input-group">
                                    <label className="input-group-label" htmlFor="dateCreated">Date Created</label>
                                    <input className="input-group-input small" type="text"
                                        id="dateCreated" value={formData.dateCreated} onChange={handleFormChange} disabled />
                                </div>

                                <div className="input-group">
                                    <label className="input-group-label" htmlFor="dateModified">Date Modified</label>
                                    <input className="input-group-input small" type="text"
                                        id="dateModified" value={formData.dateModified} onChange={handleFormChange} disabled />
                                </div>

                                <div className="modal-footer">
                                    <button type="button" className="btn-secondary" onClick={closeModal}>Cancel</button>
                                    <button type="submit" className="btn-primary">{editMode ? 'Save' : 'Add'}</button>
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
