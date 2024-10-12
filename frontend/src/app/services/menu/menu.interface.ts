export interface MenuResponse {
    id: number;
    displayName: string;
    url: string | null;
    iconName: string | null;
    status: boolean;
    createdAt: Date;
    total: number;
    countRole: number;
}

export interface MenuCreateInput {
    displayName: string,
    url: string,
    iconName: string
}

export interface MenuUpdateInput extends MenuCreateInput {
    status: boolean;
}