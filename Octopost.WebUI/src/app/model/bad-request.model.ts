import { Request } from './request.model';

class ErrorDefinition {
    public attemptedValue: string | undefined = undefined;
    public property: string | undefined = undefined;
    public message: string | undefined = undefined;
}

class BadRequest extends Request {
    errors: ErrorDefinition[] | undefined = undefined;
}

export { BadRequest, ErrorDefinition };
