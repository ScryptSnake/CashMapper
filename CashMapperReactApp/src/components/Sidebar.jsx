import '../styles/Sidebar.css';
import '../index.css';

const Sidebar = ({ onButtonClick }) => {
    return (
        <div className="Sidebar">
            <div className="Icon-Container">
                <img className="Icon" src="./logo.png" alt="logo" />
            </div>
            <h1>CashMapper</h1>
            <Link to="/home" className="Sidebar-Button" onClick={() => onButtonClick('Home')}>
                <img src="./icons/home-4-32.png" alt="icon" className="Sidebar-Button-Icon" />
                <label>Home</label>
            </Link>
            <Link to="transaction" className="Sidebar-Button" onClick={() => onButtonClick('Transactions')}>
                <img src="./icons/list-2-32.png" alt="icon" className="Sidebar-Button-Icon" />
                <label>Transactions</label>
            </Link>
            <Link to="income" className="Sidebar-Button" onClick={() => onButtonClick('Income')}>
                <img src="./icons/money-2-32.png" alt="icon" className="Sidebar-Button-Icon" />
                <label>Income</label>
            </Link>
            <Link to="cashflow" className="Sidebar-Button" onClick={() => onButtonClick('Cash Flow')}>
                <img src="./icons/line-32.png" alt="icon" className="Sidebar-Button-Icon" />
                <label>Cash Flow</label>
            </>
            <Link to="budget" className="Sidebar-Button" onClick={() => onButtonClick('Budget')}>
                <img src="./icons/minus-6-32.png" alt="icon" className="Sidebar-Button-Icon" />
                <label>Budget</label>
            </Link>
            <Link to="expenses" className="Sidebar-Button" onClick={() => onButtonClick('Expenses')}>
                <img src="./icons/negative-dynamic-32.png" alt="icon" className="Sidebar-Button-Icon" />
                <label>Expenses</label>
            </Link>
        </div>
    );
};

export default Sidebar;