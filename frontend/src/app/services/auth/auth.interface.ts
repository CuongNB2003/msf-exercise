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

// dữ liệu trả  về
export interface MeResponse extends User {
    id: number;
}

export interface LoginResponse {
    token: TokenData;
    expiration: string;
    user: User;
}

export interface RefreshTokenResponse {
    accessToken: Token;
    refreshToken: Token;
}

// input
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


