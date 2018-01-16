export class PlainText {
    id: number;
    text: string;

    public constructor(
        fields?: {
            id?: number,
            text: string
        }) {
        if (fields) Object.assign(this, fields);
    }
}