import React, { useState, useEffect } from 'react';
import { Sidebar } from '../components/Sidebar';
import '../styles/Modal.css';
import '../styles/Page.css'
import '../styles/Settings.css'


export const SettingsDialog    = ({ showModal, closeModal, message, callback }) => {
    // callback = fired when YES is clicked.
    if (!showModal) return null; // Don't show if showModal is false

    // prop to pass to Sidebar. 
    const sidebarButtons = [
        { caption: "Categories", navUrl: null, imageSource: "./icons/home-4-32.png" },
        { caption: "Data", navUrl: null, imageSource: "./icons/list-2-32.png" },
        { caption: "User", navUrl: null, imageSource: "./icons/list-2-32.png" },
    ]

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
                        <Sidebar
                            buttons={sidebarButtons}
                        />
                        <div className="Page">
                            <h1>hello world</h1>
                        </div>
                    </div>


                    
                </div>
            </div>
        </div>
    );
};

