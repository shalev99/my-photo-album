import React, { useEffect } from "react";
import { useFilesStore } from "../store/usefilesStore";
import FileForm from "../components/fileForm";
import FileItem from "../components/FileItem";
import { FileData } from '../types/types';

const HomePage: React.FC = () => {
  const { files, fetchFiles, page, hasMore } = useFilesStore();

  useEffect(() => {
    fetchFiles(1);
  }, []);

  return (
    <div className="container mx-auto p-6">
      <h1 className="text-2xl font-bold mb-4">Picture Album</h1>

      {files.length > 0 ? (
        <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-6 mt-6">
      {files.map(({ file }: { file: FileData }) => (
            <FileItem key={file.id} file={file} />
          ))}
        </div>
      ) : (
        <p className="text-gray-500 mt-4">No files uploaded yet.</p>
      )}

      {hasMore && (
        <button
          onClick={() => fetchFiles(page + 1)}
          className="mt-6 px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-700"
        >
          Load More
        </button>
      )}

      <FileForm />
    </div>
  );
};

export default HomePage;
