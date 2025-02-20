import React, { useEffect } from "react";
import { usefilesStore } from "../store/usefilesStore";
import FileForm from '../components/fileForm';
import FileItem from '../components/FileItem';
import { FileData } from '../types/types';

const HomePage: React.FC = () => {
  // Get files and the fetchfiles function from the store
  const { files, fetchfiles } = usefilesStore();

  // Fetch files from the DB when the component mounts
  useEffect(() => {
    fetchfiles();
  }, []);

  return (
    <div className="container mx-auto p-6">
      {/* Page title */}
      <h1 className="text-2xl font-bold mb-4">Picture Album</h1>
           
      {/* Display files in a responsive grid */}
      {files.length > 0 ? (
        <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-6 mt-6">
          {files.map((file) => {
            // Format file data to ensure id and uploadDate are strings
            const formattedFile: FileData = {
              ...file,
              id: String(file.id),
              uploadDate: file.uploadDate instanceof Date
                ? file.uploadDate.toISOString()
                : String(file.uploadDate),
                fileContentBase64: file.fileContentBase64
            };

            return <FileItem key={formattedFile.id} file={formattedFile} />;
          })}
        </div>
      ) : (
        <p className="text-gray-500 mt-4">No files uploaded yet.</p>
      )}

      {/* File upload form */}
      <FileForm />

    </div>
  );
};

export default HomePage;
