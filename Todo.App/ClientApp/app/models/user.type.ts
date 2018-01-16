export class User {
    userId: string = "";
    email : string = "";
    password : string = "";
    rememberMe: boolean = false;

    public constructor(fields?: {
        userId?: string,
        email?: string,
        password?: string,
        rememberMe?: boolean
    }) {
        if (fields) {
            Object.assign(this, fields);
        }
    }
}
