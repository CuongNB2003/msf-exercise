import { RoleDto } from "../role/role.interface";

export interface UserResponse {
    id: number,
    name: string,
    email: string,
    roleId: number,
    avatar: string,
    createdAt: Date,
    role: RoleDto
}

export interface InputCreateUser {
    email: string,
    role: number,
    avatar: string
}

export interface InputUpdateUser {
    email: string,
    roleI: number,
    avatar: string,
    name: string
}