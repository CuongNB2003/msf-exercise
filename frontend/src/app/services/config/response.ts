export interface ResponseText {
    status: number,
    message: string
}

export interface ResponseObject<T> {
    status: number;
    message: string;
    data: T;
}

export interface PagedResult<T> {
    totalRecords: number;
    page: number;
    limit: number;
    data: T[];
}
