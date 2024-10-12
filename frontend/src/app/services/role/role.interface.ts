import { MenuResponse } from "@services/menu/menu.interface";
import { PermissionResponse } from "@services/permission/permission.interface";

export interface RoleResponse {
    id: number;
    name: string;
    countUser: number;
    createdAt: Date;
    total: number; // Thêm trường total
    menus: MenuResponse[]; // Thêm danh sách menus
    permissions: PermissionResponse[]; // Thêm danh sách permissions
}


export interface RoleDto {
    id: number;
    name: string;
}

export interface RoleInput {
    name: string;
    description: string;
    menuIds: number[];
    permissionIds: number[];
}