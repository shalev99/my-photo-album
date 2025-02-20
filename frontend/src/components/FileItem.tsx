import React from "react";
import { FileData } from "../types/types";

interface FileItemProps {
  file: FileData;
}

const FileItem: React.FC<FileItemProps> = React.memo(({ file }) => {
  const imageSource = `data:${file.fileType};base64,${
    file.fileContent ? file.fileContent : file.fileContentBase64
  }`;

  return (
    <div className="bg-white rounded-lg shadow-lg p-4 transition-all hover:shadow-xl">
      <div className="flex justify-between items-center">
        <h2 className="text-lg font-semibold truncate">{file.name}</h2>
        <span className="bg-blue-500 text-white text-xs font-bold px-2 py-1 rounded-full">
          ID: {file.id}
        </span>
      </div>

      {imageSource ? (
        <img
          src={imageSource}
          alt={file.name}
          className="mt-3 w-full h-48 object-cover rounded-md border border-gray-200"
          onError={(e) => (e.currentTarget.src = "/placeholder-image.jpg")} // Fallback image
        />
      ) : (
        <p className="text-gray-500 mt-2 text-sm">No image available</p>
      )}
    </div>
  );
}, (prevProps, nextProps) => prevProps.file.id === nextProps.file.id);

export default FileItem;
