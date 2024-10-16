export interface PermissionResponse {
    id: number;
    permissionName: string;
    description: string | null;
    total: number;
    countRole: number;
    createdAt: Date;
}

export interface PermissionRoleResponse {
    id: number;
    permissionName: string;
    description: string | null;
}

export interface PermissionInput {
    permissionName: string,
    description: string
}


