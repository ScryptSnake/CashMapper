
import '../styles/Sidebar.css';
import '../index.css';

interface SidebarProps {
    onButtonClick: (buttonName: string) => void;
}

const Sidebar = ({onButtonClick }: SidebarProps) => {

    return (
        <div className="Sidebar">
            <div className="Icon-Container">
                <img className="Icon" src=".\logo.png" alt="logo"></img>
            </div>

            <h1>CashMapper</h1>
            <button className="Sidebar-Button" onClick={()=> onButtonClick('Home') }>
                <img src=".\icons\home-4-32.png" alt="icon" className="Sidebar-Button-Icon" />
                <label>Home</label>
            </button>
            <button className="Sidebar-Button" onClick={() => onButtonClick('Transactions')}>
                <img src=".\icons\list-2-32.png" alt="icon" className="Sidebar-Button-Icon" />
                <label>Transactions</label>
            </button>
            <button className="Sidebar-Button" onClick={() => onButtonClick('Income')}>
                <img src=".\icons\money-2-32.png" alt="icon" className="Sidebar-Button-Icon" />
                <label>Income</label>
            </button>
            <button className="Sidebar-Button" onClick={() => onButtonClick('Cash Flow')} >
                <img src=".\icons\line-32.png" alt="icon" className="Sidebar-Button-Icon" />
                <label>Cash Flow</label>
            </button>
            <button className="Sidebar-Button" onClick={() => onButtonClick('Budget')}>
                <img src=".\icons\minus-6-32.png" alt="icon" className="Sidebar-Button-Icon" />
                <label>Budget</label>
            </button>
            <button className="Sidebar-Button" onClick={() => onButtonClick('Expenses')}>
                <img src=".\icons\negative-dynamic-32.png" alt="icon" className="Sidebar-Button-Icon" />
                <label>Expenses</label>
            </button>
        </div>
    );
};

export default Sidebar;
