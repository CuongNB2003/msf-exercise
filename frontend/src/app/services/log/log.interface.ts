export interface Log {
    id: number,
    method: string,
    statusCode: number,
    url: string,
    clientIpAddress: string,
    userName: string,
    duration: number,
    createdAt: Date
}