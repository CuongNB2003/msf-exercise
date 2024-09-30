export interface ResponseText {
    status: number,
    message: string
}

export interface ResponseObject<T> {
    status: number;
    message: string;
    data: T;
}

export interface ResponseArray<T> {
    status: number;
    message: string;
    data: T[];
}
