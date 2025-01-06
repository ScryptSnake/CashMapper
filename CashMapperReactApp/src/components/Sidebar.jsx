import '../styles/Sidebar.css';
import React, { useState } from 'react';
import { NavLink } from 'react-router-dom';


export const Sidebar = ({buttons, onButtonClick }) => {

    if (!buttons) {
        console.error("No button options passed to sidebar component.",buttons)
    }
    console.log("BUTTONS = " + JSON.stringify(buttons))
    const [activeButton, setActiveButton] = useState(0);

    const handleButtonClick = (index) => {
        setActiveButton(index);
        onButtonClick(index);
    }
    return (
        <div className="Sidebar">
            {buttons.map((button, index) => (

                button.navUrl ? (

                <NavLink
                    className="Sidebar-Button"
                    to={button.navUrl || "#"}
                    key={index}
                    activeClassName="input-group-input"
                    onClick={() => { handleButtonClick(index) }}
                >
                    <img src={button.imageSource} className="Sidebar-Button-Icon"></img>
                    <label>{button.caption}</label>
                    {console.log("RENDERING: " + button.caption)}
                </NavLink>
                ) : (

                        <
                )
            ))}
        </div>
    );
};
