import React from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import '../styles/modal.css';

const EditTransaction = ({ showModal, closeModal, transaction }) => {
    if (!showModal) return null; // Don't render if showModal is false

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
                                <div className="input-group mb-3">
                                    <input type="text" className="form-control-sm bg-light" placeholder="Transaction ID"
                                        aria-label="Recipient's username" aria-describedby="basic-addon2" />
                                </div>

                                <div className="input-group mb-3">
                                    <input type="text" className="form-control-sm" placeholder="Transaction ID"
                                        aria-label="Recipient's username" aria-describedby="basic-addon2" />
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
