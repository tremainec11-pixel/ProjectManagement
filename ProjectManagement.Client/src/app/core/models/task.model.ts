export interface Task {
id: number;
title: string;
description: string;
status: string;
priority: string;
dueDate: string | null;
createdAt: string;
projectId: number;
assignedToId: number | null;
assignedToName: string | null;
}

export interface CreateTask {
title: string;
description: string;
status: string;
priority: string;
dueDate: string | null;
projectId: number;
assignedToId: number | null;
}

export interface UpdateTask {
title: string;
description: string;
status: string;
priority: string;
dueDate: string | null;
projectId: number;
assignedToId: number | null;
}
