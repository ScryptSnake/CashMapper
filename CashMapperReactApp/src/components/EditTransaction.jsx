import React from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';

const EditTransaction = () => (
    <div className="modal">
        <div className="modal-dialog modal-dialog-centered modal-lg">
            <div className="modal-content">
                <div className="modal-header">
                    <h5 className="modal-title">Modal title</h5>
                    <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div className="modal-body">
                    <form>
                        <div className="mb-3">
                            <label htmlFor="email" className="form-label">Email</label>
                            <input type="email" className="form-control" id="email" />
                            <div className="form-text">We'll never share your email.</div>
                        </div>
                        <div className="mb-3">
                            <label htmlFor="password" className="form-label">Password</label>
                            <input type="password" className="form-control" id="password" />
                        </div>
                        <div className="form-check">
                            <input type="checkbox" className="form-check-input" id="check" />
                            <label className="form-check-label" htmlFor="check">Check me out</label>
                        </div>
                        <button type="submit" className="btn btn-primary mt-2">Submit</button>
                    </form>
                </div>
                <div className="modal-footer">
                    <button type="button" className="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                    <button type="button" className="btn btn-primary">Save</button>
                </div>
            </div>
        </div>
    </div>
);

export default EditTransaction;
