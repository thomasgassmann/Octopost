import { Request } from './request.model';

export class CreatedResult extends Request {
    constructor() {
        super();
    }

    public entity: string | undefined = undefined;
    public createdId: number | undefined = undefined;
}
