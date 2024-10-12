import { RoleDto } from "../role/role.interface";

export interface UserResponse {
    id: number,
    name: string,
    email: string,
    avatar: string,
    createdAt: Date,
    roles: RoleDto[]
}

export interface InputCreateUser {
    email: string,
    avatar: string,
    roleIds: number[]
}

export interface InputUpdateUser {
    email: string,
    avatar: string,
    name: string,
    roleIds: number[]
}