import '../styles/Settings.css';
import '../styles/Page.css';
import CashMapperDataProvider from '../data/CashMapperDataProvider'
import React, { useState, useEffect } from 'react';
import { NavLink } from 'react-router-dom';
import { TableComponent } from '../components/TableComponent';

/**
 * Sidebar
 * 
 * A reusable sidebar component that renders buttons provided by prop.
 * - buttons (array):  Buttons to be added. Structure:  {caption, navUrl (optional), imageSource (optional) }
 * - onButtonClick:  Callback when button is selected. Provides index of button selected. 
 * - style:  optional in-line style to override Sidebar default css.
 */
export const CategoriesSettings = ({}) => {
    // style:  an optional css style to override the sidebar style. 


    const [activeButton, setActiveButton] = useState(0);
    const [categories, setCategories] = useState([]);
    const [selectedCategory, setSelectedCategory] = useState();

    // Load categories on load.
    useEffect(() => {
        const data = async () => {
            try {
                const categoriesData = await CashMapperDataProvider.Categories.getAllAsync();
                setCategories(categoriesData)
            } catch (error) {
                console.error("Failed to grab categories list in CategoriesSettings", error);
            }
        };
        data();

        }, [])

    return (
        <div className="categories">
            <div className="Menu-Bar">
                <div className="h-filler"></div>
                <button className="btn secondary with-icon"
                    onClick={() => { setSelectedTransaction(null); setShowEdit(true) }}>
                    <img src="./icons/plus-6-16.png"></img>

                </button>
            </div>

            <TableComponent
                data={categories}
                hideKeys={["dateModified", "dateCreated"] }
                

            />


            {/*<div className="table-container">*/}
            {/*    <table>*/}
            {/*        <thead>*/}
            {/*            <tr>*/}
            {/*                <th>Id</th>*/}
            {/*                <th>Name</th>*/}
            {/*                <th>Type</th>*/}
            {/*                <th>Flag</th>*/}

            {/*            </tr>*/}
            {/*        </thead>*/}
            {/*        <tbody className="tbl-content">*/}
            {/*            {categories.map((category) => (*/}
            {/*                <tr className="tbl-row" key={category.id}*/}
            {/*                    onClick={() => { setSelectedCategory(category) }}*/}
            {/*                    >*/}
            {/*                    <td>{category.id}</td>*/}
            {/*                    <td>{category.name}</td>*/}
            {/*                    <td>{category.category_type || '[No Type]'}</td>*/}
            {/*                    <td>{category.flag}</td>*/}
             
            {/*                </tr>*/}
            {/*            ))}*/}
            {/*        </tbody>*/}
            {/*    </table>*/}
            {/*</div>*/}
        </div>
    );
};
