export class AuthResult {
    accessToken: string;
    expiresIn: number;
    idToken: string;

    public constructor(
        fields?: {
            accessToken: string,
            expiresIn: number,
            idToken: string
        }) {
        if (fields)
            Object.assign(this, fields);
    }
}