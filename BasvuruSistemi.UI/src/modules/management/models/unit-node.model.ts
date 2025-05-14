export interface Unit {
  id: string;
  parentId?: string;
  name: string;
  code?: string;
  children?: Unit[];
}
