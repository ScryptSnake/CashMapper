import React, { useState, useEffect } from 'react';
import '../styles/Modal.css';

const EditTransaction = ({ showModal, closeModal, transaction, updateTransactions}) => {

    if (!showModal) return null; // Don't render if showModal is false

    // States
    const [formData, setFormData] = useState(transaction);
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

    // Submit data
    const submitFormData = async (e) => {
        e.preventDefault(); //prevent app from re-rendering.

        let method = 'PUT';
        if (formData.Id == null) {
            method = 'POST';
        };

        try {
            const response = await fetch('http://localhost:5009/api/Transactions', {
                method: method, // Use 'POST' to create a new resource, 'PUT' to update
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(formData), // Convert form data to JSON
            });

            if (!response.ok) {
                throw new Error(`Error: ${response.status}`);
            }
            closeModal();
            updateTransactions(); // Re update transactions page after submit.
        } catch (error) {
            console.error('Error submitting form:', error);
            alert('Failed to update transaction.');
        }
    };

    // Grab categories from API to fill dropdown when form renders.
    useEffect(() => {
        fetch('http://localhost:5009/api/Categories') // Replace with your actual API URL
            .then((response) => response.json())
            .then((data) => {
                console.log(data);
                setCategoriesHandler(data); // Set the fetched data into state
            })
            .catch((error) => console.error('Error fetching data:', error));
    }, []);

    return (
        <div>
            {/* Backdrop overlay */}
            <div className="modal-backdrop" onClick={closeModal}></div>

            <div className="modal">
                <div className="modal-dialog">
                    <div>
                        <div className="modal-header">
                            <h2>Edit Transaction: [{formData.id}]</h2>
                            <button type="button" onClick={closeModal}>X</button>
                        </div>

                        <div>
                            <form onSubmit={submitFormData}>
                                <div className="input-group">
                                    <label className="input-group-label" htmlFor="description">Description</label>
                                    <input className="input-group-input" type="text" id="description" value={formData.description} onChange={handleFormChange} />
                                </div>

                                <div className="input-group">
                                    <label className="input-group-label" htmlFor="transactionDate">Date</label>
                                    <input className="input-group-input small" type="text" id="transactionDate" value={formData.transactionDate} onChange={handleFormChange} />
                                </div>

                                <div className="input-group">
                                    <label className="input-group-label" htmlFor="categoryId">Category</label>
                                    <select className="input-group-input medium" id="categoryId" value={formData.categoryId} onChange={handleFormChange}>
                                        {categories.map((category) => (
                                            <option key={category.id} value={category.id}>
                                                {category.name}
                                            </option>
                                        ))}
                                    </select>
                                </div>

                                <div className="input-group">
                                    <label className="input-group-label" htmlFor="value">Value</label>
                                    <input className="input-group-input small" type="number" id="value" value={formData.value} onChange={handleFormChange} />
                                </div>

                                <div className="input-group">
                                    <label className="input-group-label" htmlFor="note">Notes</label>
                                    <textarea className="input-group-input" id="note" value={formData.note} onChange={handleFormChange} />
                                </div>

                                <hr className="modal-divider" />

                                <div className="input-group">
                                    <label className="input-group-label" htmlFor="source">Source</label>
                                    <input className="input-group-input medium" type="text" id="source" value={formData.source} onChange={handleFormChange} />
                                </div>

                                <div className="input-group">
                                    <label className="input-group-label" htmlFor="dateCreated">Date Created</label>
                                    <input className="input-group-input small" type="text" id="dateCreated" value={formData.dateCreated} onChange={handleFormChange} disabled />
                                </div>

                                <div className="input-group">
                                    <label className="input-group-label" htmlFor="dateModified">Date Modified</label>
                                    <input className="input-group-input small" type="text" id="dateModified" value={formData.dateModified} onChange={handleFormChange} disabled />
                                </div>

                                <div className="modal-footer">
                                    <button type="button" className="btn-secondary" onClick={closeModal}>Cancel</button>
                                    <button type="submit" className="btn-primary">Save</button>
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
