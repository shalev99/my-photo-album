import { create } from "zustand";
import { FileData } from "../types/types";
import { isValidDate } from "../utils/utils";

export interface FilesStore {
  files: FileData[];
  page: number;
  hasMore: boolean;
  fetchFiles: (page?: number) => Promise<void>;
  addFile: (file: FileData) => void;
}

export const useFilesStore = create<FilesStore>((set, get) => ({
  files: [],
  page: 1,
  hasMore: true,

  fetchFiles: async (page = 1) => {
    try {
      const res = await fetch(
        `https://localhost:7061/api/files?page=${page}&pageSize=10`,
        {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
          },
          mode: "cors",
        }
      );

      if (!res.ok) {
        throw new Error("Failed to fetch files");
      }

      const data: FileData[] = await res.json();

      const formattedData: FileData[] = data.map((file) => ({
        ...file,
        uploadDate: isValidDate(file.uploadDate)
          ? file.uploadDate.toISOString()
          : String(file.uploadDate),
      }));

      set((state) => ({
        files: page === 1 ? formattedData : [...state.files, ...formattedData],
        page,
        hasMore: data.length === 10,
      }));
    } catch (error) {
      console.error("Fetch error:", error);
    }
  },

  addFile: (file: FileData) =>
    set((state) => ({ files: [...state.files, file] })),
}));
