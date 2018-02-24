import { PostFilterProvider } from "../providers/index";
import { Post } from "./post.model";

export class FetchRequest {
    private _filterFunc: (service: PostFilterProvider, data: any) => Promise<Post[]>;
    private _data: any;

    constructor(filterFunc: (service: PostFilterProvider, data: any) => Promise<Post[]>, data: any) {
        this._data = data;
        this._filterFunc = filterFunc;
    }

    public get filterFunc(): (service: PostFilterProvider, data: any) => Promise<Post[]> {
        return this._filterFunc;
    }

    public get data(): any {
        return this._data;
    }
}