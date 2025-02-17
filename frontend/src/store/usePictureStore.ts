import { create } from 'zustand';

interface Picture {
    id: number;
    name: string;
}

interface PictureStore {
    pictures: Picture[];
    fetchPictures: () => Promise<void>;
}

export const usePictureStore = create<PictureStore>((set) => ({
    pictures: [],
    fetchPictures: async () => {
        const res = await fetch('/api/pictures');
        set({ pictures: await res.json() });
    }
}));
