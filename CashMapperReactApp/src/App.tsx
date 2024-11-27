import './App.css';

function App() {
    return (
        <div className="Main-Window">
            <div className="Header">
                <button className="Header-Button">
                    <img src=".\icons\settings-4-32.png" className="Sidebar-Button-Icon" />
                </button>
            </div>
            <div className="Window">
                <div className="Sidebar">
                    <div className="Icon-Container">
                        <img className="Icon" src=".\logo.png" alt="logo"></img>
                    </div>

                    <h1>CashMapper</h1>
                    <button className="Sidebar-Button">
                        <img src=".\icons\home-4-32.png" alt="icon" className="Sidebar-Button-Icon"/>
                        <label>Home</label>
                    </button>
                    <button className="Sidebar-Button">
                        <img src=".\icons\list-2-32.png" alt="icon" className="Sidebar-Button-Icon"/>
                        <label>Transactions</label>
                    </button>
                    <button className="Sidebar-Button">
                        <img src=".\icons\money-2-32.png" alt="icon" className="Sidebar-Button-Icon"/>
                        <label>Income</label>
                    </button>
                    <button className="Sidebar-Button">
                        <img src=".\icons\line-32.png" alt="icon" className="Sidebar-Button-Icon"/>
                        <label>Cash Flow</label>
                    </button>
                    <button className="Sidebar-Button">
                        <img src=".\icons\minus-6-32.png" alt="icon" className="Sidebar-Button-Icon" />
                        <label>Budget</label>
                    </button>
                    <button className="Sidebar-Button">
                        <img src=".\icons\negative-dynamic-32.png" alt="icon" className="Sidebar-Button-Icon" />
                        <label>Expenses</label>
                    </button>


 
                </div>
                <div className="Content">
                    <div className="Page">
                        <h1>Income Profiles</h1>
                        <div className="Separator"></div>
                        <input type="text" className="Textbox" placeholder="Type something..." />
                    </div>
                </div>
            </div>
            <footer className="Footer">

            </footer>
              
        </div>
    );
}

export default App;
