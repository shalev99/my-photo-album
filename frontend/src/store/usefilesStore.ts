import { create } from 'zustand';

interface Files {
    id: number;
    name: string;
}

interface filesStore {
    files: Files[];
    fetchfiles: () => Promise<void>;
}

export const usefilesStore = create<filesStore>((set) => ({
    files: [],
    fetchfiles: async () => {
        const res = await fetch('http://localhost:7061/api/files');
        set({ files: await res.json() });
    }
}));
