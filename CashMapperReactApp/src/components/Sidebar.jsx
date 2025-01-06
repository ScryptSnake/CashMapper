import '../styles/Sidebar.css';
import React, { useState } from 'react';
import { NavLink } from 'react-router-dom';
/**
 * Sidebar
 * 
 * A reusable sidebar component that renders buttons provided by prop.
 * - buttons (array):  Buttons to be added. Structure:  {caption, navUrl (optional), imageSource (optional) }
 * - onButtonClick:  Callback when button is selected. Provides index of button selected. 
 * - style:  optional in-line style to override Sidebar default css.
 */
export const Sidebar = ({buttons, onButtonClick, style }) => {
    // style:  an optional css style to override the sidebar style. 
    if (!buttons) {
        console.error("No button options passed to sidebar component.",buttons)
    }

    const [activeButton, setActiveButton] = useState(0);

    const handleButtonClick = (index) => {
        setActiveButton(index);
        onButtonClick(index);
    }

    return (
        <div className="Sidebar" style={style}>
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
                </NavLink>
                ) : (
                    //No navUrl provided, use buttons instead
                        <button
                            className="Sidebar-Button"
                            key={index}
                            activeClassName="input-group-input"
                            onClick={() => { handleButtonClick(index) }}
                        >
                            <img src={button.imageSource} className="Sidebar-Button-Icon"></img>
                            <label>{button.caption}</label>
                        </button>
                )
            ))}
        </div>
    );
};
