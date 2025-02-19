import React from "react";
import { FileData } from '../types/types';

interface FileItemProps {
  file: FileData;
}

const FileItem: React.FC<FileItemProps> = ({ file }) => {
  // Determine the image source based on available data
  const imageSource = file.src
  ? file.src
  : (file.fileType && file.fileContent)
  ? `data:${file.fileType};base64,${file.fileContent}`
  : "";


  return (
    <div className="bg-white rounded-lg shadow-md p-4">
      <h2 className="text-lg font-semibold">{file.name}</h2>
      {imageSource ? (
        <img
          src={imageSource}
          alt={file.name}
          className="mt-3 w-full h-48 object-cover rounded-md"
        />
      ) : (
        <p>No image available</p>
      )}
    </div>
  );
};

export default FileItem;
