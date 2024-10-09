export interface RoleResponse {
    id: number;
    name: string;
    countUser: number;
    createdAt: Date;
}

export interface RoleDto {
    id: number;
    name: string;
}

export interface InputRole {
    name: string;
}