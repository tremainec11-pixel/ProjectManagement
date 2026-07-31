export interface Project {
  id: number;
  name: string;
  description: string;
  status: string;
  startDate: string;
  dueDate: string;
  createdAt: string;
  ownerId: number;
  ownerName: string;
  memberCount: number;
}