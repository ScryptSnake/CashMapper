import CategoriesApi from './CategoriesApi.js';
import TransactionsApi from './TransactionsApi.js'


const CashMapperDataProvider = {
    Categories: CategoriesApi,
    Transactions: TransactionsApi

};

export default CashMapperDataProvider;
