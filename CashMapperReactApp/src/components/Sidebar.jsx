import '../styles/Sidebar.css';
import React, { useState } from 'react';
import { NavLink } from 'react-router-dom';


const Sidebar = ({ onButtonClick }) => {

    const [activeButton, setActiveButton] = useState('');

    const handleButtonClick = (buttonName) => {
        setActiveButton(buttonName);
        onButtonClick(buttonName);
    }

    return (
        <div className="Sidebar">
            <div className="Icon-Container">
                <img className="Icon" src="./logo.png" alt="logo" />
            </div>
            <h1>CashMapper</h1>
            <NavLink to="/home" className="Sidebar-Button" activeClassName="input-group-input"
                onClick={() => handleButtonClick('Home')}>
                <img src="./icons/home-4-32.png" alt="icon" className="Sidebar-Button-Icon" />
                <label>Home</label>
            </NavLink>
            <NavLink to="/transactions" className="Sidebar-Button" activeClassName="active"
                onClick={() => handleButtonClick('Transactions')}>
                <img src="./icons/list-2-32.png" alt="icon" className="Sidebar-Button-Icon" />
                <label>Transactions</label>
            </NavLink>
            <NavLink to="/income" className="Sidebar-Button" activeClassName="active"
                onClick={() => handleButtonClick('Income')}>
                <img src="./icons/money-2-32.png" alt="icon" className="Sidebar-Button-Icon" />
                <label>Income</label>
            </NavLink>
            <NavLink to="/cashflow" className="Sidebar-Button" activeClassName="active"
                onClick={() => handleButtonClick('Cash Flow')}>
                <img src="./icons/line-32.png" alt="icon" className="Sidebar-Button-Icon" />
                <label>Cash Flow</label>
            </NavLink>
            <NavLink to="/budget" className="Sidebar-Button" activeClassName="active"
                onClick={() => handleButtonClick('Budget')}>
                <img src="./icons/minus-6-32.png" alt="icon" className="Sidebar-Button-Icon" />
                <label>Budget</label>
            </NavLink>
            <NavLink to="/expenses" className="Sidebar-Button" activeClassName="active"
                onClick={() => handleButtonClick('Expenses')}>
                <img src="./icons/negative-dynamic-32.png" alt="icon" className="Sidebar-Button-Icon" />
                <label>Expenses</label>
            </NavLink>
        </div>
    );
};

export default Sidebar;