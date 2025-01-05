import React, { useState, useEffect } from 'react';
import '../styles/Modal.css';


export const ConfirmationPrompt = ({ showModal, closeModal, message, callback}) => {
    // callback = fired when YES is clicked.
    if (!showModal) return null; // Don't show if showModal is false


    const handleFormChange = (e) => {
        closeModal();
        if (e.target.id === "confirm") {
            callback();
        }
    };

    return (
        <div>
            {/* Backdrop overlay */}
            <div className="modal-backdrop" onClick={closeModal}></div>

            <div className="modal small">

                <div className="modal-dialog">

                    <div className="modal-header">
                        <div className="h-filler"></div>
                        <button type="button" onClick={closeModal}>X</button>
                    </div>
                    <h2>{message}</h2>
                    <hr className="modal-divider" />
                    <div className="input-group">
                        <div className="h-filler"></div>
                        <button className="btn secondary" onClick={closeModal}>Cancel</button>
                        <button id="confirm" className="btn primary" onClick={handleFormChange}>Yes</button>
                    </div>
                </div>
            </div>
        </div>
    );
};

