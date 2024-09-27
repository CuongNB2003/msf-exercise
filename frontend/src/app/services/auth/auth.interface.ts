export interface Token {
    token: string;
    expires: string;
}

export interface TokenData {
    accessToken: Token;
    refreshToken: Token;
}

export interface Role {
    id: number;
    name: string;
}

export interface User {
    name: string;
    email: string;
    roleId: number;
    avatar: string;
    role: Role;
}

export interface LoginResponse {
    status: number;
    message: string;
    data: {
        token: TokenData;
        expiration: string;
        user: User;
    };
}

export interface RegisterInput {
    name: string,
    email: string,
    passWord: string,
    avatar: string,
}

export interface LoginInput {
    email: string,
    passWord: string,
    reCaptchaToken: string
}

export interface ResponseText {
    status: number,
    message: string
}
