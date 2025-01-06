import './styles/App.css';
import './styles/Index.css';
import {Sidebar} from './components/Sidebar';
import HomePage from './components/HomePage';
import { TransactionsPage } from './components/TransactionsPage';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom'; 
import { SettingsDialog } from './components/SettingsDialog';
import React, { useState, useEffect } from 'react';
function App() {

    // prop to pass to Sidebar. 
    const sidebarButtons = [
        { caption: "Home", navUrl: "/home", imageSource: "./icons/home-4-32.png" },
        { caption: "Transactions", navUrl: "/transactions", imageSource: "./icons/list-2-32.png" },
        { caption: "Income", navUrl: "/income", imageSource: "./icons/money-2-32.png" },
        { caption: "Cash Flow", navUrl: "/cashflow", imageSource: "./icons/line-32.png" },
        { caption: "Budget", navUrl: "/budget", imageSource: "./icons/minus-6-32.png" },
        { caption: "Expenses", navUrl: "/expenses", imageSource: "./icons/negative-dynamic-32.png" },
    ]
    const [openSettings, setOpenSettings] = useState(false); // Open settings form

    return (
        <Router> {/* Wrap everything in Router to enable routing */}
            <div className="Main-Window">
                <div className="Header">
                    <button className="Header-Button">
                        <img src=".\icons\settings-4-32.png" className="Sidebar-Button-Icon" onClick={() => {setOpenSettings(true) } } />
                    </button>
                </div>

                <div className="Window">
                    <div className="Left-Content">
                                <div className="Icon-Container">
                            <img className="Icon" src="./logo.png" alt="logo"></img>
                        </div>
                        <label className="label-secondary">CashMapper</label>
                        <Sidebar buttons={sidebarButtons} /> {/* Sidebar with navigation links */}
                        <h5>v1.0 ScryptSnake @ Github.com</h5>
                    </div>

                    <div className="Right-Content">
                        <Routes>
                            {/* Define your Routes here */}
                            <Route path="/home" element={<HomePage />} />
                            <Route path="/transactions" element={<TransactionsPage />} />
                            <Route path="/income" element={<div>Income Content</div>} />
                            <Route path="/cashflow" element={<div>Cash Flow Content</div>} />
                            <Route path="/budget" element={<div>Budget Content</div>} />
                            <Route path="/expenses" element={<div>Expenses Content</div>} />
                            {/* Default route */}
                            <Route path="/" element={<HomePage />} />
                        </Routes>
                    </div>
                </div>

                <footer className="Footer"></footer>
            </div>
            <SettingsDialog
                showModal={openSettings}
                closeModal={() => {setOpenSettings(false)} }
            />



        </Router>
    );
}

export default App;
