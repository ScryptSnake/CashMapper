import React, { useState, useEffect } from 'react';
import '../styles/Modal.css';
import '../styles/Page.css'
import '../styles/Settings.css'


export const SettingsDialog    = ({ showModal, closeModal, message, callback }) => {
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
            <div className="modal">
                <div className="modal-dialog">
                    <div className="modal-header">
                        <h2>Settings</h2>
                        <div className="h-filler"></div>
                        <button type="button" onClick={closeModal}>X</button>
                    </div>
                    <div className="settings-layout">
                        <div className="side-panel">
                            <button className="btn secondary">Categories</button>
                            <button className="btn secondary">Data</button>
                            <button className="btn secondary">Security</button>
                        </div>
                        <div className="Page">
                            <h1>hello world</h1>
                        </div>
                    </div>


                    
                </div>
            </div>
        </div>
    );
};

