import { MenuRoleResponse } from "@services/menu/menu.interface";
import { PermissionRoleResponse } from "@services/permission/permission.interface";

export interface RoleResponse {
    id: number;
    name: string;
    countUser: number;
    createdAt: Date;
    total: number; // Thêm trường total
    menus: MenuRoleResponse[]; // Thêm danh sách menus
    permissions: PermissionRoleResponse[]; // Thêm danh sách permissions
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