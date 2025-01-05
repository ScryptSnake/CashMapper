
const ImportsApi = {

    // List of import sources.
    getSources: () => {

        return ["AUTO", "PSECU-CHECKING", "PSECU-SAVINGS"]
    },

    // Call parser depending on sorce

    parseAsync: async (sourceId, data) => {

        return ["Fake","Data","Here"]
    }



}
    

export default ImportsApi;
