export interface FileData {
  id: string | number; // Allow both types
  name: string;
  description: string;
  fileName: string;
  fileSize: number;
  fileType: string;
  fileContent: string;
  fileContentBase64: string;
  uploadDate: string;
}
