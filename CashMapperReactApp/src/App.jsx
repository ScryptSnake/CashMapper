import './App.css';
import './styles/Page.css'
import Sidebar from './components/Sidebar';
import HomePage from './components/HomePage';
import EditTransaction from './components/EditTransaction';
import TransactionsPage from './components/TransactionsPage';
import React, { useState } from 'react';


function App() {

    const [content, setContent] = useState('Home');

    const handleSidebarClick = (contentName) => {
        setContent(contentName);
    };


    const [showModal, setModalShow] = useState(false);
    const openModal = () => {
        setModalShow(true);
    }
    // Function to close the modal
    const closeModal = () => {
        setShowModal(false); // Set showModal to false to hide the modal
    };


    return (
        <div className="Main-Window">
            <div className="Header">
                <button className="Header-Button">
                    <img src=".\icons\settings-4-32.png" className="Sidebar-Button-Icon" />
                </button>
            </div>

            <div className="Window">
                <Sidebar onButtonClick={handleSidebarClick} />
                <div className="Content">
                        {content === 'Home' && <HomePage />}
                        {content === 'Transactions' && <TransactionsPage />}
                        {content === 'Income' && <div>fuck</div>}
                        {content === 'Cash Flow' && <div>Cash Flow Content</div>}
                        {content === 'Budget' && <div>Budget Content</div>}
                        {content === 'Expenses' && <div>Expenses Content</div>}
                </div>

            </div>
            
            <footer className="Footer">
            </footer>
              
        </div>
    );
}

export default App;
