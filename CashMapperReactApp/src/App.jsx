import './styles/App.css';
import './styles/Page.css';
import Sidebar from './components/Sidebar';
import HomePage from './components/HomePage';
import TransactionsPage from './components/TransactionsPage';
import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom'; 

function App() {
    return (
        <Router> {/* Wrap everything in Router to enable routing */}
            <div className="Main-Window">
                <div className="Header">
                    <button className="Header-Button">
                        <img src=".\icons\settings-4-32.png" className="Sidebar-Button-Icon" />
                    </button>
                </div>

                <div className="Window">
                    <Sidebar /> {/* Sidebar with navigation links */}
                    <div className="Content">
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

                <footer className="Footer">
                </footer>
            </div>
        </Router>
    );
}

export default App;
