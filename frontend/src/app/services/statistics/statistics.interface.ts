export default interface LogMethodStatistics {
    yearMonth: string; // hoặc kiểu dữ liệu khác nếu cần
    postCount: number;
    putCount: number;
    getCount: number;
    deleteCount: number;
}

export default interface RoleCountUserStatistics {
    roleName: string;
    totalUsers: number;
}