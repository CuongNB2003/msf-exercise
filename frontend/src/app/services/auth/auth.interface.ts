export interface User {
    id: number;
    fullName: string;
    email: string;
    password: string | null;
    avatar: string;
    role: string;
}

export interface LoginResponse {
    statusCode: number;
    message: string;
    data: {
        data: User;
        accessToken: string;
    };
}