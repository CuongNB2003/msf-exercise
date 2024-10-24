export default interface LogMethodByYear {
    yearMonth: string; // hoặc kiểu dữ liệu khác nếu cần
    postCount: number;
    putCount: number;
    getCount: number;
    deleteCount: number;
}

export default interface LogMethodByMonth {
    accessDate: string; // hoặc kiểu dữ liệu khác nếu cần
    postCount: number;
    putCount: number;
    getCount: number;
    deleteCount: number;
}

export default interface RoleCountUser {
    roleName: string;
    totalUsers: number;
}