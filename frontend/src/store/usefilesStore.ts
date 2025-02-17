import { create } from 'zustand';

interface Files {
    id: number;
    name: string;
}

interface filesStore {
    pictures: Files[];
    fetchPictures: () => Promise<void>;
}

export const usefilesStore = create<filesStore>((set) => ({
    pictures: [],
    fetchPictures: async () => {
        const res = await fetch('/api/files');
        set({ pictures: await res.json() });
    }
}));
