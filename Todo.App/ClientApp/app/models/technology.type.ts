export class Technology {
    technologyId: number;
    technologyName: string;

    public constructor(
        fields?: {
            technologyId: number,
            technologyName?: string
        }) {
        if (fields)
            Object.assign(this, fields);
    }
}