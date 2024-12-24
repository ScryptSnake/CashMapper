import React, { useState, useEffect } from 'react';

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
            <div className="modal-backdrop fade show" style={{ backgroundColor: 'rgba(0, 0, 0, )' }}></div>

            <div className="modal fade show" tabIndex="-1" role="dialog" style={{ display: 'block' }}>
                <div className="modal-dialog modal-dialog-centered modal-lg">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h1 className="modal-title">Edit Transaction: [{formData.id}]</h1>
                            <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close" onClick={closeModal}></button>
                        </div>

                        <div className="modal-body">
                            <form onSubmit={submitFormData}>
                                <div class="input-group input-group-sm mb-3 w-100">
                                    <span class="input-group-text">Description</span>
                                    <input type="text" class="form-control" id="description"
                                        value={formData.description} onChange={handleFormChange} />
                                </div>


                                <div className="input-group input-group-sm mb-3 w-50">
                                    <span class="input-group-text">Date</span>
                                    <input type="text" class="form-control" id="source"
                                        value={formData.transactionDate} onChange={handleFormChange} />
                                </div>

                                <div className="mb-3 w-50">
                                    <div className="input-group input-group-sm">
                                        <label htmlFor="categorySelect" className="input-group-text">Category</label>
                                        <select className="form-select" id="categoryId"
                                            value={formData.categoryId} onChange={handleFormChange}>
                                            {categories.map((category) => (
                                                <option key={category.id} value={category.id}>
                                                    {category.name}
                                                </option>
                                            ))}
                                        </select>
                                    </div>
                                </div>

                                <div className="input-group input-group-sm mb-3 w-25">
                                    <span class="input-group-text">Source</span>
                                    <input type="text" class="form-control" id="source"
                                        value={formData.source} onChange={handleFormChange} />
                                </div>

                                <div className="input-group input-group-sm mb-3 w-25">
                                    <span class="input-group-text">Value</span>
                                    <input type="number" class="form-control" id="value"
                                        value={formData.value}
                                        onChange={handleFormChange}
                                    />
                                </div>

                                <div className="input-group input-group-sm mb-3 w-100">
                                    <span class="input-group-text">Notes</span>
                                    <input type="text" class="form-control" id="note"
                                        value={formData.note} onChange={handleFormChange} />
                                </div>

                                <hr></hr>
                                <div className="input-group input-group-sm mb-3 w-50">
                                    <span class="input-group-text">Date Created</span>
                                    <input type="text" class="form-control" id="dateCreated"
                                        value={formData.dateCreated} onChange={handleFormChange} disabled />
                                </div>
                                <div className="input-group input-group-sm mb-3 w-50">
                                    <span class="input-group-text">Date Modified</span>
                                    <input type="text" class="form-control" id="dateModified"
                                        value={formData.dateModified} onChange={handleFormChange} disabled />
                                </div>

                                <div className="modal-footer">
                                    <button type="cancel" className="btn btn-secondary" onClick={closeModal}>Cancel</button>
                                    <button type="submit" className="btn btn-primary">Save</button>
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
