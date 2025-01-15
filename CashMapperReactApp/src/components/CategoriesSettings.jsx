import '../styles/Settings.css';
import '../styles/Page.css';
import CashMapperDataProvider from '../data/CashMapperDataProvider'
import React, { useState, useEffect } from 'react';
import { NavLink } from 'react-router-dom';
import { TableComponent } from '../components/TableComponent';
import { ListboxComponent } from '../components/ListboxComponent';
import moment from 'moment';


export const CategoriesSettings = ({}) => {
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

            <ListboxComponent
                data={categories}
                headers={{categoryType: "Type", dateCreated: "Created", dateModified: null, flag: null, id: null }}
                sortOrder={["id","name", "categoryType", "dateCreated"]}
                transform={{ dateCreated: (value) => moment(value).format('M/D/YY hh:mm A') }}
                keyField={"name"}
            />

            {/*<TableComponent*/}
            {/*    data={categories}*/}
            {/*    headers={{ categoryType: "Type", dateCreated: "Created", dateModified: null, flag: null, id: null }}*/}
            {/*    sortOrder={["name", "categoryType", "dateCreated"]}*/}
            {/*    transform={{ dateCreated: (value) => moment(value).format('M/D/YY hh:mm A')} }*/}
            {/*/>*/}

        </div>
    );
};
