export class TodoItem {
    id: number;
    name: string;
    isComplete: boolean;
    ownerId: string;

    public constructor(fields?: {
        id?: number,
        name?: string,
        isComplete?: boolean,
        ownerId?:string
    }) {
        if (fields) {
            Object.assign(this, fields);
        }
    }
}