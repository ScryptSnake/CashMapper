const CategoriesApi = {
    getAll: async () => {
        try {
            const response = await fetch('http://localhost:5009/api/Categories');
            if (!response.ok) {
                throw new Error(`Failed to fetch categories. Status: ${response.status}`);
            }
            const data = await response.json();
            return data;
        } catch (error) {
            console.error('Error fetching all categories:', error);
            throw error;
        }
    },

    getSingle: async (idOrName) => {
        // idOrName: int id, or string name of category.
        try {
            const response = await fetch(`http://localhost:5009/api/Categories/${idOrName}`);
            if (!response.ok) {
                throw new Error(`Failed to fetch category: ${idOrName}. Status: ${response.status}`);
            }
            const category = await response.json();
            return category;
        } catch (error) {
            console.error(`Error fetching category: ${idOrName}`, error);
            throw error;
        }
    },

    addItem: async (data) => {
        try {
            const response = await fetch('http://localhost:5009/api/Categories/', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'accept': 'text/plain',
                },
                body: JSON.stringify(data),
            });

            if (!response.ok) {
                throw new Error('Failed to add category. Status: ' + response.status);
            }

            const result = await response.json();
            return result;
        } catch (error) {
            console.error('Error adding category:', error);
            throw error;
        }
    },

    updateItem: async (data) => {
        try {
            const response = await fetch('http://localhost:5009/api/Categories/', {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    'accept': 'text/plain',
                },
                body: JSON.stringify(data),
            });

            if (!response.ok) {
                throw new Error('Failed to update category. Status: ' + response.status);
            }

            const result = await response.json();
            return result;
        } catch (error) {
            console.error('Error updating category:', error);
            throw error;
        }
    },
};

export default CategoriesApi;
