export interface Task {
    id: number;
    title: string;
    description?: string;
    dueDate: string; // נשמר כמחרוזת תאריך (פורמט ISO או "YYYY-MM-DD")
  }