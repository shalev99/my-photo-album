import { create } from 'zustand';

export interface Files {
  id: number;
  name: string;
  description: string;
  fileName: string;
  fileSize: number; 
  fileType: string; 
  uploadDate: string | Date; 
  fileContent: string;
  fileContentBase64: string;
}
interface FilesStore {
  files: Files[];
  fetchfiles: () => Promise<void>;
  addFile: (file: Files) => void;
}

export const usefilesStore = create<FilesStore>((set) => ({
  files: [],
  fetchfiles: async () => {
    try {
      const res = await fetch('https://localhost:7061/api/files', {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
        },
        mode: 'cors', // Explicitly set to CORS
      });
      if (!res.ok) {
        throw new Error('Failed to fetch files');
      }
      const data = await res.json();
      set({ files: data });
    } catch (error) {
      console.error('Fetch error:', error);
    }
  },
  addFile: (file: Files) =>
    set((state) => ({ files: [...state.files, file] })),
}));
