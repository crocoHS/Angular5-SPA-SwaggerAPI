import { Technology } from "./technology.type";
import { Theme } from "./theme.type";

export class Customer {
    id: number;
    ownerId: string;
    firstName: string;
    lastName: string;
    email: string;
    gender: string;
    registrationDate: Date;
    technologyList: Technology[];
    isActive: boolean;

    set fullName(fullName: string) {
        this.fullName = fullName;
    }

    public constructor(
        fields?: {
            id?: number,
            ownerId?: string,
            firstName?: string,
            lastName?: string,
            fullName?: string,
            email?: string,
            gender?: string,
            registrationDate?: Date,
            technologies?: string[],
            isActive?: boolean
        }) {
        if (fields) {
            Object.assign(this, fields);
        }
    }
}