import { Token } from "../config/response";
import { RoleDto } from "../role/role.interface";


export interface TokenLogin {
    accessToken: Token;
    refreshToken: Token;
}

export interface UserLogin {
    name: string;
    email: string;
    roleId: number;
    avatar: string;
    role: RoleDto;
}

export interface LoginResponse {
    token: TokenLogin;
    expiration: string;
    user: UserLogin;
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


