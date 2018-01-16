export class EmailMessage {
    fromEmail: string;
    toEmail: string;
    subject: string;
    body: string;
    attachmentPath: string;
    isBodyHtml: boolean;
    isImportant: boolean;
    timeStamp: Date;

    public constructor(
        fields?: {
            toEmail: string,
            fromEmail?: string,
            subject?: string,
            body?: string,
            attachmentPath?: string,
            isBodyHtml?: boolean,
            isImportant?: boolean,
            timeStamp?: Date
        }) {
        if (fields)
            Object.assign(this, fields);
    }
}