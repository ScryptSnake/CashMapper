import React, { useState, useEffect } from 'react';

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
                            <h1 className="modal-title">Edit Transaction: [{transaction.id}]</h1>
                            <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close" onClick={closeModal}></button>
                        </div>

                        <div className="modal-body">
                            <form>
                                <div class="input-group input-group-sm mb-3 w-100">
                                    <span class="input-group-text" id="description">Description</span>
                                    <input type="text" class="form-control" value={transaction.description} />
                                </div>

                                {/* Load categories list*/}
                                <div className="mb-3 w-50">
                                    <div className="input-group input-group-sm">
                                        <label htmlFor="categorySelect" className="input-group-text">Category</label>
                                        <select className="form-select" id="categorySelect">
                                            {categories.map((category) => (
                                                <option key={category.id} value={category.id}>
                                                    {category.name}
                                                </option>
                                            ))}
                                        </select>
                                    </div>

                                </div>
                                <div class="input-group input-group-sm mb-3 w-25">
                                    <span class="input-group-text" id="source">Source</span>
                                    <input type="text" class="form-control" value={transaction.source} />
                                </div>


                                <div class="input-group input-group-sm mb-3 w-100">
                                    <span class="input-group-text" id="notes">Notes</span>
                                    <input type="text" class="form-control" value={transaction.note} />
                                </div>

                                <div class="input-group input-group-sm mb-3 w-25">
                                    <span class="input-group-text" id="value">Value</span>
                                    <input type="text" class="form-control" value={transaction.value.toLocaleString("en-US", { style: "currency", currency: "USD", signDisplay: "always", })} />
                                </div>


                                 

                            </form>
                        </div>
                        <div className="modal-footer">
                            <button type="button" className="btn btn-primary">Save</button> {/* Button aligned to the right */}
                        </div>


                    </div>
                </div>
            </div>
        </div>
    );
};

export default EditTransaction;
