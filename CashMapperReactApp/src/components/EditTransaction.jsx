import React, { useState, useEffect } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import '../styles/modal.css';

const EditTransaction = ({ showModal, closeModal, transaction }) => {
    if (!showModal) return null; // Don't render if showModal is false

    const [categories, setCategories] = useState([]);

    // Grab categories from API.
    useEffect(() => {
        fetch('http://localhost:5009/api/Categories') // Replace with your actual API URL
            .then((response) => response.json())
            .then((data) => {
                console.log(data);
                setCategoriesHandler(data); // Set the fetched data into state
            })
            .catch((error) => console.error('Error fetching data:', error));
    }, []);

    const setCategoriesHandler = (data) => {
        setCategories(data);
    }


    return (
        <div>
            {/* Backdrop overlay */}
            <div className="modal-backdrop fade show" style={{ backgroundColor: 'rgba(0, 0, 0, )' }}></div>

            <div className="modal fade show" tabIndex="-1" role="dialog" style={{ display: 'block' }}>
                <div className="modal-dialog modal-dialog-centered modal-lg">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title">Edit Transaction</h5>
                            <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close" onClick={closeModal}></button>
                        </div>
                        <div className="modal-body">
                            <form>
                                <div class="input-group">
                                    <span class="input-group-text">Transaction ID</span>
                                    <input type="text" for="transactionId" className="form-control-sm bg-light" />
                                </div>

                                {/* Load categories list*/}
                                <div class="mb-3">
                                    <label htmlFor="categorySelect" className="form-label">Category</label>
                                    <select className="form-select form-select-sm" id="categorySelect">
                                        {categories.map((category) => (
                                            <option key={category.id} value={category.id}>
                                                {category.name}
                                            </option>
                                        ))}
                                    </select>
                                </div>

               


                            </form>
                        </div>
                        <div className="modal-footer">
                            <button type="button" className="btn btn-primary">Save</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default EditTransaction;
