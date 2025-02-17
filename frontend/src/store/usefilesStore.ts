import { create } from 'zustand';

interface Files {
    id: number;
    name: string;
    description: string;
    fileName: string;
    fileSize: number; 
    fileType: string; 
    uploadDate: string | Date; 
    fileContent: string;
}

interface filesStore {
    files: Files[];
    fetchfiles: () => Promise<void>;
}

export const usefilesStore = create<filesStore>((set) => ({
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
            set({ files: await res.json() });
        } catch (error) {
            console.error('Fetch error:', error);
        }
    }
}));

