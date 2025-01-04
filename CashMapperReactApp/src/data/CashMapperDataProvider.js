import CategoriesApi from './CategoriesApi.js';
import TransactionsApi from './TransactionsApi.js'
import ImportsApi from './ImportsApi.js'


const CashMapperDataProvider = {
    Categories: CategoriesApi,
    Transactions: TransactionsApi,
    Imports: ImportsApi

};

export default CashMapperDataProvider;
