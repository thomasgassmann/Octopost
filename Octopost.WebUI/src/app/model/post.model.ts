import { Comment } from './comment.model';
import { File } from './file.model';

export class Post {
  public id: number | undefined = undefined;
  public text: string | undefined = undefined;
  public topic: string | undefined = undefined;
  public voteCount: number | undefined = undefined;
  public created: Date | undefined = undefined;
  public longitude: number | undefined = undefined;
  public latitude: number | undefined = undefined;
  public locationName: string | undefined = undefined;
  public comments: Array<Comment> | undefined = undefined;
  public file: File | undefined = undefined;
}
