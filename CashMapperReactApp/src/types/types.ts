export interface Transaction {

    id: number;
    description: string | null;
    source: string | null;
    categoryId: number;
    note: string | null;
    value: number;
    date: Date;
}