export interface Project {

  id: number;

  name: string;

  description: string;

  status: string;

  startDate: string;

  dueDate: string | null;

  createdAt: string;

  ownerId: number;

  ownerName: string;

  memberCount: number;

  progress: number;
}