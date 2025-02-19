import React, { useEffect } from "react";
import { usefilesStore } from "../store/usefilesStore";
import FileForm from '../components/fileForm';
import FileItem from '../components/FileItem';
import { FileData } from '../types/types';

const HomePage: React.FC = () => {
  const { files, fetchfiles } = usefilesStore();

  useEffect(() => {
    fetchfiles();
  }, [fetchfiles]);

  return (
    <div className="container mx-auto p-6">
      <h1 className="text-2xl font-bold mb-4">Picture Album</h1>
      <FileForm />
      {files.length > 0 ? (
        <ul>
        {files.map((file) => (
          <FileItem
            key={String(file.id)}
            file={{ ...file, id: String(file.id), uploadDate: String(file.uploadDate) }}
          />
        ))}
      </ul>
      
      ) : (
        <p className="text-gray-500 mt-4">No files uploaded yet.</p>
      )}
    </div>
  );
};

export default HomePage;
