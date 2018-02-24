export class CreatePost {
    constructor(text: string, latitude: number, longitude: number) {
        this.text = text;
        this.latitude = latitude;
        this.longitude = longitude;
    }

    public text: string | undefined = undefined;
    public latitude: number | undefined = undefined;
    public longitude: number | undefined = undefined;
    public fileId: number | undefined = undefined;
}
