import { Post } from './';
import { FilterPostService } from '../services';

declare type ContainerRequestDelegate =
    (request: PostContainerRequest, page: number, pageSize: number, commentAmount: number) => Promise<Post[]>;

class PostContainerRequest {
    constructor(
        public fetchFunction: ContainerRequestDelegate,
        public data: { [id: string]: any },
        public filterPostService: FilterPostService) {
    }
}

export { PostContainerRequest, ContainerRequestDelegate };
