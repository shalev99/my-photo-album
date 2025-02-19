export interface FileData {
  id: string | number; // Allow both types
  name: string;
  description: string;
  fileName: string;
  fileSize: number;
  fileType: string;
  fileContent: string;
  uploadDate: string;
}
