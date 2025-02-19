import React from "react";
import { FileData } from '../types/types';

interface FileItemProps {
  file: FileData;
}

const FileItem: React.FC<FileItemProps> = ({ file }) => {
  return (
    <li className="border rounded-lg shadow-lg p-4">
      <h2 className="text-lg font-semibold">{file.name}</h2>
      <p className="text-sm text-gray-700">
        <strong>Description:</strong> {file.description}
      </p>
      <p className="text-sm">
        <strong>File Name:</strong> {file.fileName}
      </p>
      <p className="text-sm">
        <strong>File Size:</strong> {file.fileSize} bytes
      </p>
      <p className="text-sm">
        <strong>File Type:</strong> {file.fileType}
      </p>
      <p className="text-sm">
        <strong>Upload Date:</strong> {new Date(file.uploadDate).toLocaleString()}
      </p>
      <img
        src={`data:${file.fileType};base64,${file.fileContent}`}
        alt={file.name}
        className="mt-3 max-w-full h-auto rounded"
      />
    </li>
  );
};

export default FileItem;
